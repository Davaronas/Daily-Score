using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private RectTransform tasksScrollContentRectTransform = null;
    [SerializeField] private GameObject taskPrefab = null;
    [Space]
    [SerializeField] private TMP_Dropdown valueMetricDropdown = null;
    [Space]
    [Tooltip("Type texts")]
    [SerializeField] private GameObject targetValueTexts = null;
    [SerializeField] private GameObject maximumTexts = null;
    [SerializeField] private GameObject minimumTexts = null;
    [SerializeField] private GameObject booleanTexts = null;
    [SerializeField] private GameObject optimumTexts = null;

    private string enteredName = "default";
    private AppManager.TaskType taskType = 0;

   [SerializeField] private TMP_InputField taskNameField = null;

    private List<Task> currentTasks = new List<Task>();


    private GoalManager goalManager = null;
    private TaskTypeComponents taskTypeComponents = null;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        taskTypeComponents = FindObjectOfType<TaskTypeComponents>();

        List<string> _metrics = new List<string>();
        for(int i = 0; i < (int)AppManager.TaskMetricType.ENUM_END ;i++)
        {
            _metrics.Add(RuntimeTranslator.TranslateTaskMetricType((AppManager.TaskMetricType)i));
        }
        valueMetricDropdown.AddOptions(_metrics);

        AppManager.OnLanguageChanged += LanguageChangedCallback;

        if(maximumTexts == null || minimumTexts == null || booleanTexts == null || optimumTexts == null) { Debug.LogError("Task texts not set up in the inspector"); return; }

    }

    private void OnDisable()
    {
        enteredName = "";
        taskNameField.text = "";
        DestroyTasks();
        currentTasks.Clear();
        DisableTypeTexts();
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
            _newTask.FeedData(_tasks[i]);
            _newTask.isPrefab = false;
            currentTasks.Add(_newTask);
        }

        // resize task scroll to fit tasks
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

        maximumTexts.SetActive(false);
        minimumTexts.SetActive(false);
        booleanTexts.SetActive(false);
        optimumTexts.SetActive(false);
    }

    public void DisplayTaskTypeText(AppManager.TaskType _taskType)
    {
        taskType = _taskType;
        DisableTypeTexts();

        switch(_taskType)
        {
            case AppManager.TaskType.Maximum:
                maximumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                break;
            case AppManager.TaskType.Minimum:
                minimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                break;
            case AppManager.TaskType.Boolean:
                booleanTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                break;
            case AppManager.TaskType.Optimum:
                optimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                break;
            case AppManager.TaskType.Interval:

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

        if (enteredName == "") { return; }



       TaskData _data = taskTypeComponents.GetData(taskType);
        if(_data == null) { return; }



        _data.name = enteredName;
        goalManager.AssignTaskToCurrentGoal(_data);
       

        AppManager.NewTaskAdded();
    }


    public void RemoteCall_SetSelectedName()
    {
        enteredName = taskNameField.text;
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
