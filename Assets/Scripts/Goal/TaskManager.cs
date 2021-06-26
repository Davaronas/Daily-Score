using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private RectTransform tasksScrollContentRectTransform = null;
    [SerializeField] private GameObject taskPrefab = null;
    [Space]
    [SerializeField] private TMP_Dropdown valueMetricDropdown = null;
    [SerializeField] private TMP_Dropdown intervalMetricDropdown = null;
    [SerializeField] private DayToggle[] dayToggles;
    [SerializeField] private TMP_InputField everyThDayInputField = null;
    [Space]
    [Tooltip("Type texts")]
    [SerializeField] private GameObject targetValueTexts = null;
    [SerializeField] private GameObject intervalMeasureTexts = null;
    [SerializeField] private GameObject maximumTexts = null;
    [SerializeField] private GameObject minimumTexts = null;
    [SerializeField] private GameObject booleanTexts = null;
    [SerializeField] private GameObject optimumTexts = null;
    [SerializeField] private GameObject intervalTexts = null;
    private bool taskTypeSelected = false;

    private string enteredName = "default";
    private AppManager.TaskType taskType = 0;

   [SerializeField] private TMP_InputField taskNameField = null;

    private List<Task> currentTasks = new List<Task>();


    [HideInInspector] public GoalManager goalManager = null;
    private TaskTypeComponents taskTypeComponents = null;

    private List<DayOfWeek> selectedActiveDays = new List<DayOfWeek>();
    private int everyThDay = 0;


    private VerticalLayoutGroup tasksContentLayoutGroup = null;

    // aid variables
    private float taskPrefab_Y_Size_;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        taskTypeComponents = FindObjectOfType<TaskTypeComponents>();
        taskPrefab_Y_Size_ = taskPrefab.GetComponent<RectTransform>().sizeDelta.y;
        tasksContentLayoutGroup = tasksScrollContentRectTransform.GetComponent<VerticalLayoutGroup>();

        List<string> _metrics = new List<string>();
        for(int i = 0; i < (int)AppManager.TaskMetricType.ENUM_END ;i++)
        {
            _metrics.Add(RuntimeTranslator.TranslateTaskMetricType((AppManager.TaskMetricType)i));
        }
        valueMetricDropdown.AddOptions(_metrics);
        intervalMetricDropdown.AddOptions(_metrics);

        AppManager.OnLanguageChanged += LanguageChangedCallback;



        if(maximumTexts == null || minimumTexts == null || booleanTexts == null || optimumTexts == null) { Debug.LogError("Task texts not set up in the inspector"); return; }

    }

    private void Start()
    {
        DisableTypeTexts();
    }

    private void OnDisable()
    {
        enteredName = "";
        taskNameField.text = "";
        DestroyTasks();
        currentTasks.Clear();
        DisableTypeTexts();
        ClearDays();
        taskTypeSelected = false;
    }

    private void OnDestroy()
    {
        AppManager.OnLanguageChanged -= LanguageChangedCallback;
    }

    public void DisplayTasks(TaskData[] _tasks)
    {
        for(int i = 0; i < _tasks.Length;i++)
        {
            Task _newTask =
                Instantiate(taskPrefab, Vector3.zero, Quaternion.identity, tasksScrollContentRectTransform.transform).GetComponent<Task>();
            _newTask.FeedData(_tasks[i], goalManager);
            _newTask.isPrefab = false;
            currentTasks.Add(_newTask);
        }

        // resize task scroll to fit tasks
        ScrollSizer.Resize(tasksScrollContentRectTransform, taskPrefab_Y_Size_ + (tasksContentLayoutGroup.spacing * 2),currentTasks.Count);
    }



    public void DestroyTasks()
    {

       
        for (int i = 0; i < currentTasks.Count;i++)
        {
            if (!currentTasks[i].isPrefab)
            {
                Destroy(currentTasks[i].gameObject);
            }
        }
    }

    private void DisableTypeTexts()
    {
        targetValueTexts.SetActive(false);
        intervalMeasureTexts.SetActive(false);

        maximumTexts.SetActive(false);
        minimumTexts.SetActive(false);
        booleanTexts.SetActive(false);
        optimumTexts.SetActive(false);
        intervalTexts.SetActive(false);
    }



    public void DisplayTaskTypeText(AppManager.TaskType _taskType)
    {
        taskType = _taskType;
        DisableTypeTexts();

        taskTypeSelected = true;

        switch(_taskType)
        {
            case AppManager.TaskType.Maximum:
                maximumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);
                break;
            case AppManager.TaskType.Minimum:
                minimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);
                break;
            case AppManager.TaskType.Boolean:
                booleanTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(false);
                break;
            case AppManager.TaskType.Optimum:
                optimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);
                break;
            case AppManager.TaskType.Interval:
                intervalTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(true);
                break;
        }
    }

    public void RemoteCall_CreateNewTask()
    {
        if (goalManager.GetCurrentlySelectedGoal() == null)
        {
            Debug.LogError("Trying to create a task without a selected goal");
            return;
        }

        if (enteredName == "") { AppManager.ErrorHappened(ErrorMessages.NameNotEntered()); return; }

        if(taskTypeSelected == false) { AppManager.ErrorHappened(ErrorMessages.TaskTypeNotSelected_CreateTaskPanel());  return; }

        TaskData _data = taskTypeComponents.GetData(taskType);
        if (_data == null) { return; } // error message is inside GetData() function

        if (selectedActiveDays.Count == 0 && everyThDay == 0) { AppManager.ErrorHappened(ErrorMessages.DaysNotSelected_CreateTaskPanel()); return; }



      


        _data.name = enteredName;

        if(selectedActiveDays.Count > 0)
        {
            _data.beingActiveType = TaskData.ActiveType.DayOfWeek;
            _data.activeOnDays = selectedActiveDays;

            for (int i = 0; i < _data.activeOnDays.Count; i++)
            {
                if (DateTime.Now.DayOfWeek == _data.activeOnDays[i])
                {
                    _data.isActiveToday = true;
                }
            }
            

        }
        else if(everyThDay > 0)
        {
            _data.beingActiveType = TaskData.ActiveType.EveryThDay;
            _data.activeEveryThDay = everyThDay;
            _data.isActiveToday = true;
            _data.lastActiveDay = DateTime.Now;
            _data.nextActiveDay = CalculateNextActiveDay(_data);
        }

        

        goalManager.AssignTaskToCurrentGoal(_data);
       

        AppManager.NewTaskAdded();
    }

    private DateTime CalculateNextActiveDay(TaskData _data)
    {
       return  _data.lastActiveDay.AddDays(_data.activeEveryThDay);
    }

    public void RemoteCall_SetSelectedName()
    {
        enteredName = taskNameField.text;
    }

    public void SetActiveDays(DayOfWeek _day, bool _add)
    {
        

        if (_add)
        {
            if (!selectedActiveDays.Contains(_day))
            {
                print("Added");
                selectedActiveDays.Add(_day);
                everyThDayInputField.text = "";
                everyThDay = 0;
            }
        }
        else
        {
            if (selectedActiveDays.Contains(_day))
            {
                print("Removed");
                selectedActiveDays.Remove(_day);
            }
        }
    }

    public void RemoteCall_SetEveryThDay()
    {
        if (everyThDayInputField.text != "")
        {
            if(everyThDayInputField.text == "0")
            {
                everyThDayInputField.text = "";
                // error message, number cannot be 0
                return;
            }

            everyThDay = int.Parse(everyThDayInputField.text);
            selectedActiveDays.Clear();
            TurnOffDayToggles();
        }
    }

    private void ClearDays()
    {
        selectedActiveDays.Clear();
        everyThDay = 0;
        everyThDayInputField.text = "";

        TurnOffDayToggles();
    }

    private void TurnOffDayToggles()
    {
        for (int i = 0; i < dayToggles.Length; i++)
        {
            dayToggles[i].TurnOff();
        }
    }



    private void LanguageChangedCallback(AppManager.Languages _lang)
    {
        valueMetricDropdown.ClearOptions();

        List<string> _metrics = new List<string>();
        for (int i = 0; i < (int)AppManager.TaskMetricType.ENUM_END; i++)
        {
            _metrics.Add(RuntimeTranslator.TranslateTaskMetricType((AppManager.TaskMetricType)i));
        }
        valueMetricDropdown.AddOptions(_metrics);
    }

}
