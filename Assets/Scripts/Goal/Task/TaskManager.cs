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
    [Space]
    [SerializeField] private GameObject taskNameField_GO = null;
    [SerializeField] private GameObject taskTypeButtons = null;
    [SerializeField] private GameObject createNewTaskButton = null;
    [SerializeField] private TMP_Text taskNameText_EditMode = null;
    [SerializeField] private TMP_Text taskTypeInfoText_EditMode = null;
    [SerializeField] private TMP_Text taskTypeText_EditMode = null;
    [SerializeField] private GameObject confirmChangesButton_EditMode = null;
    [SerializeField] private GameObject deleteTaskButton_EditMode = null;
    [SerializeField] private GameObject resetTaskButton_EditMode = null;
    [Space]
    [SerializeField] private GameObject notificationDaySelectorPanel = null;
    [SerializeField] private GameObject notificationDaySelectorWindow = null;
    [SerializeField] private GameObject notificationDayButtonPrefab = null;
    [SerializeField] private NotificationHolder notificationHolder = null;
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
        OnDisable();
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
        notificationDaySelectorPanel.SetActive(false);
        notificationHolder.Clear();
        DisableEditModeObjectsAndEnableCreateModeObjects();
    }

    private void ClearDays()
    {
        selectedActiveDays.Clear();
        everyThDay = 0;
        everyThDayInputField.text = "";

        TurnOffDayToggles();
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

    private void DisableEditModeObjectsAndEnableCreateModeObjects()
    {
        taskNameText_EditMode.gameObject.SetActive(false);
        taskTypeInfoText_EditMode.gameObject.SetActive(false);
        taskTypeText_EditMode.gameObject.SetActive(false);
        confirmChangesButton_EditMode.SetActive(false);
        deleteTaskButton_EditMode.SetActive(false);
        resetTaskButton_EditMode.SetActive(false);

        taskNameField_GO.SetActive(true);
        taskTypeButtons.SetActive(true);
        createNewTaskButton.SetActive(true);
    }

    public void EditTask(TaskData _data)
    {
        taskNameText_EditMode.text = _data.name;
        taskTypeText_EditMode.text = RuntimeTranslator.TranslateTaskType(_data.type);

        taskNameText_EditMode.gameObject.SetActive(true);
        taskTypeInfoText_EditMode.gameObject.SetActive(true);
        taskTypeText_EditMode.gameObject.SetActive(true);
        confirmChangesButton_EditMode.SetActive(true);
        deleteTaskButton_EditMode.SetActive(true);
        resetTaskButton_EditMode.SetActive(true);

        taskNameField_GO.SetActive(false);
        taskTypeButtons.SetActive(false);
        createNewTaskButton.SetActive(false);


        for (int i = 0; i < _data.activeOnDays.Count; i++)
        {
            selectedActiveDays.Add((DayOfWeek)_data.activeOnDays[i]);
        }

        everyThDay = _data.activeEveryThDay;

        switch (_data.type)
        {
            case AppManager.TaskType.Maximum:
                taskTypeComponents.EditMode_SetMaxDataComponents(_data as MaximumTaskData);
                maximumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);
                break;
            case AppManager.TaskType.Minimum:
                taskTypeComponents.EditMode_SetMinDataComponents(_data as MinimumTaskData);
                minimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);
                break;
            case AppManager.TaskType.Boolean:
                taskTypeComponents.EditMode_SetBoolDataComponents(_data as BooleanTaskData);
                booleanTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(false);
                break;
            case AppManager.TaskType.Optimum:
                taskTypeComponents.EditMode_SetOptimumDataComponents(_data as OptimumTaskData);
                optimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);
                break;
            case AppManager.TaskType.Interval:
                taskTypeComponents.EditMode_SetIntervalDataComponents(_data as IntervalTaskData);
                intervalTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(true);
                break;
        }

        for (int i = 0; i < dayToggles.Length; i++)
        {

            if (_data.activeOnDays.Count > 0)
            {
                for (int j = 0; j < _data.activeOnDays.Count; j++)
                {
                    if (dayToggles[i].GetDay() == (DayOfWeek)_data.activeOnDays[j])
                    {
                        dayToggles[i].TurnOn();
                        break;
                    }
                }
            }
            else
            {
                everyThDayInputField.text = _data.activeEveryThDay.ToString();
            }
        }

        for (int i = 0; i < _data.notificationAttachedActiveDay.Count; i++)
        {
            NotificationManager.NotificationData _nd;
            NotificationManager.GetNotificationData((DayOfWeek)_data.notificationAttachedActiveDay[i], _data.name, out _nd);
           NotificationPrefabUtility _npu =  notificationHolder.CreateNewNotification((DayOfWeek)_data.notificationAttachedActiveDay[i]);
            _npu.hour.text = Convert.ToDateTime(_nd.fireTime).Hour.ToString();
            _npu.minute.text = Convert.ToDateTime(_nd.fireTime).Minute.ToString();
        }

        // feed task type components and day holders or everyThDay input field
        // notifications
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
        _data.lastChangedValue = DateTime.MinValue.ToString();
        _data.activeOnDays = new List<int>();

        if(selectedActiveDays.Count > 0)
        {
            _data.beingActiveType = TaskData.ActiveType.DayOfWeek;

            for (int i = 0; i < selectedActiveDays.Count; i++)
            {
                _data.activeOnDays.Add((int)selectedActiveDays[i]);
            }

            for (int i = 0; i < _data.activeOnDays.Count; i++)
            {
                if ((int)DateTime.Now.DayOfWeek == _data.activeOnDays[i])
                {
                    _data.isActiveToday = true;
                }
            }

            NotificationPrefabUtility[] _npus = notificationHolder.GetNotifications().ToArray();
            for (int i = 0; i < _npus.Length; i++)
            {
                _data.notificationAttachedActiveDay.Add((int)_npus[i].daySelected);
                int daysUntilNextDay = ((int)_npus[i].daySelected - (int)DateTime.Today.DayOfWeek + 7) % 7;
                print("Days until " + _npus[i].daySelected + " " + daysUntilNextDay);
                DateTime _fireTime = DateTime.Today;
                _fireTime = _fireTime.AddDays(daysUntilNextDay);
                // _fireTime.AddHours(-_fireTime.Hour);
                _fireTime = _fireTime.AddHours(_npus[i].hourNumber);
                _fireTime = _fireTime.AddMinutes(_npus[i].minuteNumber);
                print(_fireTime);
                NotificationManager.SendNotification(goalManager.GetCurrentlySelectedGoal().GetGoalData().name + " notification", "Don't forget " + _data.name + "! " + _fireTime.Hour + ":" + _fireTime.Minute, _fireTime, 7, _data.name);
            }


        }
        else if(everyThDay > 0)
        {
            _data.beingActiveType = TaskData.ActiveType.EveryThDay;
            _data.activeEveryThDay = everyThDay;
            _data.isActiveToday = true;
            _data.nextActiveDay = CalculateNextActiveDay(_data).ToString();


            NotificationPrefabUtility[] _npus = notificationHolder.GetNotifications().ToArray();
            for (int i = 0; i < _npus.Length; i++)
            {
                _data.notificationAttachedActiveDay.Add((int)_npus[i].daySelected);
             //   int daysUntilNextDay = ((int)_npus[i].daySelected - (int)DateTime.Today.DayOfWeek + 7) % 7;
               // print("Days until " + _npus[i].daySelected + " " + daysUntilNextDay);
                DateTime _fireTime = DateTime.Today.AddDays(everyThDay);
                // _fireTime.AddHours(-_fireTime.Hour);
                _fireTime = _fireTime.AddHours(_npus[i].hourNumber);
                _fireTime = _fireTime.AddMinutes(_npus[i].minuteNumber);

                print(_fireTime);
                NotificationManager.SendNotification(goalManager.GetCurrentlySelectedGoal().GetGoalData().name + " notification", "Don't forget " + _data.name + "! " + _fireTime.Hour + ":" + _fireTime.Minute, _fireTime, everyThDay, _data.name);
            }
        }



        notificationHolder.Clear();

        

        goalManager.AssignTaskToCurrentGoal(_data);
       

        AppManager.NewTaskAdded();
    }

    private DateTime CalculateNextActiveDay(TaskData _data)
    {
       return DateTime.Now.Date.AddDays(_data.activeEveryThDay);
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


                if(everyThDay != 0)
                {
                    notificationHolder.Clear();
                }

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
                if(notificationHolder.HasDay(_day))
                {
                    notificationHolder.DeleteNotification(_day);
                }
            }
        }
    }

    public void RemoteCall_SetEveryThDay()
    {
        if (everyThDayInputField.text != "")
        {
            if (everyThDayInputField.text == "0")
            {
                everyThDayInputField.text = "";
                AppManager.ErrorHappened(ErrorMessages.EnterRealisticNumbers());
                return;
            }
            else
            {
                everyThDay = int.Parse(everyThDayInputField.text);
                selectedActiveDays.Clear();
                TurnOffDayToggles();
            }

            notificationHolder.Clear();
        }
        else
        {
            everyThDay = 0;

            if (selectedActiveDays.Count == 0)
            {
                notificationHolder.Clear();
            }
        }
    }

    public void RemoteCall_CreateNewNotification()
    {
        if(selectedActiveDays.Count == 0 && everyThDay == 0)
        {
            AppManager.ErrorHappened(ErrorMessages.DaysNotSelected_CreateTaskPanel());
            return;
        }

        bool _foundSelectable = false;
        if (selectedActiveDays.Count > 0)
        {
            for (int i = 0; i < selectedActiveDays.Count; i++)
            {
                if (!notificationHolder.HasDay(selectedActiveDays[i]))
                {
                    NotificationDaySelectButton _newButton = Instantiate(notificationDayButtonPrefab, transform.position, Quaternion.identity, notificationDaySelectorWindow.transform).GetComponent<NotificationDaySelectButton>();
                    _newButton.SetData(selectedActiveDays[i], this);
                    _foundSelectable = true;
                }
            }

            if (_foundSelectable)
            {
                notificationDaySelectorPanel.SetActive(true);
            }
            else
            {
                AppManager.ErrorHappened(ErrorMessages.EverySelectedDayHasNotifications());
            }
        }
        else if (everyThDay > 0)
        {
            if (!notificationHolder.HasDay(DateTime.Today.DayOfWeek) && notificationHolder.GetNotificationAmount() < 1)
            {
                NotificationEveryThDaySelected(everyThDay);
            }
            else
            {
                AppManager.ErrorHappened(ErrorMessages.EverySelectedDayHasNotifications());
            }
                
        }
    }

    public void NotificationDaySelected(DayOfWeek _day)
    {
        notificationHolder.CreateNewNotification(_day);
        for (int i = 0; i < notificationDaySelectorWindow.transform.childCount; i++)
        {
           Destroy(notificationDaySelectorWindow.transform.GetChild(i).gameObject);
        }
        notificationDaySelectorPanel.SetActive(false);
    }

    public void NotificationEveryThDaySelected(int _resetDay)
    {
        notificationHolder.CreateNewNotification(_resetDay);
        for (int i = 0; i < notificationDaySelectorWindow.transform.childCount; i++)
        {
            Destroy(notificationDaySelectorWindow.transform.GetChild(i).gameObject);
        }
        notificationDaySelectorPanel.SetActive(false);
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
