using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private RectTransform tasksScrollContentRectTransform = null;
    [SerializeField] private ScrollRect taskMenuScroll = null;
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
    [Space]
    [SerializeField] private GameObject infoPanel = null;
    [SerializeField] private TMP_Text infoPanelText = null;
    private bool taskTypeSelected = false;

    private string enteredName = "default";
    private AppManager.TaskType taskType = 0;

   [SerializeField] private TMP_InputField taskNameField = null;

    private List<Task> currentTasks = new List<Task>();

    private TaskData currentlySelectedTask = null;

    [HideInInspector] public GoalManager goalManager = null;
    private TaskTypeComponents taskTypeComponents = null;

    private List<DayOfWeek> selectedActiveDays = new List<DayOfWeek>();
    private int everyThDay = 0;



    private VerticalLayoutGroup tasksContentLayoutGroup = null;

    // aid variables
    private float taskPrefab_Y_Size_;


    public Action<string> TaskNameUpdateNeeded;
    public Action<string> MetricUpdateNeeded;
    public Action<string> TargetValueUpdateNeeded;

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
        HideInfoPanel();
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
        print(_data.name + " " + _data.type);

        currentlySelectedTask = _data;

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



        /*
        for (int i = 0; i < _data.activeOnDays.Count; i++)
        {
            selectedActiveDays.Add((DayOfWeek)_data.activeOnDays[i]);
        }
        */

        everyThDay = _data.activeEveryThDay;


        TaskNameUpdateNeeded?.Invoke(_data.name);

        // show task data type texts
        switch (_data.type)
        {
            case AppManager.TaskType.Maximum:
                taskTypeComponents.EditMode_SetMaxDataComponents(_data as MaximumTaskData);
                maximumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(taskTypeComponents.GetTargetValue());
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Minimum:
                taskTypeComponents.EditMode_SetMinDataComponents(_data as MinimumTaskData);
                minimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(taskTypeComponents.GetTargetValue());
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Boolean:
                taskTypeComponents.EditMode_SetBoolDataComponents(_data as BooleanTaskData);
                booleanTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(taskTypeComponents.GetTargetValue());
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Optimum:
                taskTypeComponents.EditMode_SetOptimumDataComponents(_data as OptimumTaskData);
                optimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(taskTypeComponents.GetTargetValue());
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Interval:
                taskTypeComponents.EditMode_SetIntervalDataComponents(_data as IntervalTaskData);
                intervalTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(true);

                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetIntervalMetric());
                break;
        }


        // turn on active day toggles / every th day input field


        if (_data.activeOnDays.Count > 0)
        {

            for (int i = 0; i < dayToggles.Length; i++)
            {
                for (int j = 0; j < _data.activeOnDays.Count; j++)
                {
                    if (dayToggles[i].GetDay() == (DayOfWeek)_data.activeOnDays[j])
                    {
                        dayToggles[i].TurnOn();
                        //  selectedActiveDays.Add((DayOfWeek)_data.activeOnDays[j]);
                        break;
                    }
                }
            }

            for (int k = 0; k < _data.notificationAttachedActiveDay.Count; k++)
            {

                NotificationManager.NotificationData _nd;


                if (NotificationManager.GetNotificationData((DayOfWeek)_data.notificationAttachedActiveDay[k], _data.name, out _nd))
                {
                    NotificationPrefabUtility _npu = notificationHolder.CreateNewNotification((DayOfWeek)_data.notificationAttachedActiveDay[k]);
                    _npu.SetHourAndMinute(Convert.ToDateTime(_nd.fireTime).Hour, Convert.ToDateTime(_nd.fireTime).Minute);
                }

            }


        }
        else
        {

            NotificationManager.NotificationData _nd;

            if (NotificationManager.GetNotificationData_NoDayCheck(_data.name, out _nd))
            {
                NotificationPrefabUtility _npu = notificationHolder.CreateNewNotification((DayOfWeek)_data.notificationAttachedActiveDay[0]);
                _npu.SetDayPlusIntervalDays((DayOfWeek)_data.notificationAttachedActiveDay[0], _nd.resetIntervalDays);
                _npu.SetHourAndMinute(Convert.ToDateTime(_nd.fireTime).Hour, Convert.ToDateTime(_nd.fireTime).Minute);
            }

            everyThDayInputField.text = _data.activeEveryThDay.ToString();
        }


    


       // SoundManager.PlaySound2();
      

        /*
        foreach (DayOfWeek _day in selectedActiveDays)
        {
            print(_day);
        }
        */


        // display notifications attached to this task


        // feed task type components and day holders or everyThDay input field
        // notifications


        // switch _data.activeOnDays with selected actice days
    }


  

    public void EditTaskFromSettingsNotification(TaskData _data)
    {
        OnDisable();

        taskMenuScroll.verticalNormalizedPosition = 0;

        currentlySelectedTask = _data;

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



        /*
        for (int i = 0; i < _data.activeOnDays.Count; i++)
        {
            selectedActiveDays.Add((DayOfWeek)_data.activeOnDays[i]);
        }
        */

        everyThDay = _data.activeEveryThDay;


        TaskNameUpdateNeeded?.Invoke(_data.name);

        // show task data type texts
        switch (_data.type)
        {
            case AppManager.TaskType.Maximum:
                taskTypeComponents.EditMode_SetMaxDataComponents(_data as MaximumTaskData);
                maximumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(taskTypeComponents.GetTargetValue());
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Minimum:
                taskTypeComponents.EditMode_SetMinDataComponents(_data as MinimumTaskData);
                minimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(taskTypeComponents.GetTargetValue());
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Boolean:
                taskTypeComponents.EditMode_SetBoolDataComponents(_data as BooleanTaskData);
                booleanTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(taskTypeComponents.GetTargetValue());
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Optimum:
                taskTypeComponents.EditMode_SetOptimumDataComponents(_data as OptimumTaskData);
                optimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(taskTypeComponents.GetTargetValue());
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Interval:
                taskTypeComponents.EditMode_SetIntervalDataComponents(_data as IntervalTaskData);
                intervalTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(true);

                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetIntervalMetric());
                break;
        }


        // turn on active day toggles / every th day input field


        if (_data.activeOnDays.Count > 0)
        {

            for (int i = 0; i < dayToggles.Length; i++)
            {
                for (int j = 0; j < _data.activeOnDays.Count; j++)
                {
                    if (dayToggles[i].GetDay() == (DayOfWeek)_data.activeOnDays[j])
                    {
                        dayToggles[i].TurnOn();
                        //  selectedActiveDays.Add((DayOfWeek)_data.activeOnDays[j]);
                        break;
                    }
                }
            }

            for (int k = 0; k < _data.notificationAttachedActiveDay.Count; k++)
            {

                NotificationManager.NotificationData _nd;


                if (NotificationManager.GetNotificationData((DayOfWeek)_data.notificationAttachedActiveDay[k], _data.name, out _nd))
                {
                    NotificationPrefabUtility _npu = notificationHolder.CreateNewNotification((DayOfWeek)_data.notificationAttachedActiveDay[k]);
                    _npu.SetHourAndMinute(Convert.ToDateTime(_nd.fireTime).Hour, Convert.ToDateTime(_nd.fireTime).Minute);
                }

            }


        }
        else
        {

            NotificationManager.NotificationData _nd;

            if (NotificationManager.GetNotificationData_NoDayCheck(_data.name, out _nd))
            {
                NotificationPrefabUtility _npu = notificationHolder.CreateNewNotification((DayOfWeek)_data.notificationAttachedActiveDay[0]);
                _npu.SetDayPlusIntervalDays((DayOfWeek)_data.notificationAttachedActiveDay[0], _nd.resetIntervalDays);
                _npu.SetHourAndMinute(Convert.ToDateTime(_nd.fireTime).Hour, Convert.ToDateTime(_nd.fireTime).Minute);
            }

            everyThDayInputField.text = _data.activeEveryThDay.ToString();
        }

        if (goalManager.GetCurrentlySelectedGoal() == null)
        {
            goalManager.SetCurrentlySelectedGoal(goalManager.SearchGoalByName(currentlySelectedTask.owner));
        }




        // SoundManager.PlaySound2();


        /*
        foreach (DayOfWeek _day in selectedActiveDays)
        {
            print(_day);
        }
        */


        // display notifications attached to this task


        // feed task type components and day holders or everyThDay input field
        // notifications


        // switch _data.activeOnDays with selected actice days
    }


    public void RemoteCall_SelectEveryDay()
    {
        for (int i = 0; i < dayToggles.Length; i++)
        {
            dayToggles[i].TurnOn();
        }
    }

    public void RemoteCall_ConfirmEditChanges()
    {
        if(currentlySelectedTask == null)
        {
            Debug.LogError("No task is selected");
            return;
        }

     

        if (selectedActiveDays.Count == 0 && everyThDay == 0) { AppManager.ErrorHappened(ErrorMessages.DaysNotSelected_CreateTaskPanel()); return; }

        /*
        foreach (DayOfWeek _day in selectedActiveDays)
        {
            print(_day);
        }
        */



        currentlySelectedTask.isEditedToday = false;
        currentlySelectedTask.activeOnDays.Clear();
        currentlySelectedTask.activeEveryThDay = 0;

        switch (currentlySelectedTask.type)
        {
            case AppManager.TaskType.Maximum:
                MaximumTaskData _new_mxtd = taskTypeComponents.GetData(currentlySelectedTask.type) as MaximumTaskData;
                if(_new_mxtd == null) { return; }
                ((MaximumTaskData)currentlySelectedTask).pointsGainedPerOne = _new_mxtd.pointsGainedPerOne;
                ((MaximumTaskData)currentlySelectedTask).targetValue = _new_mxtd.targetValue;
                ((MaximumTaskData)currentlySelectedTask).overachievePercentBonus = _new_mxtd.overachievePercentBonus;
                ((MaximumTaskData)currentlySelectedTask).metric = _new_mxtd.metric;
                ((MaximumTaskData)currentlySelectedTask).customMetricName = _new_mxtd.customMetricName;
                 ((MaximumTaskData)currentlySelectedTask).streakStartsAfterDays = _new_mxtd.streakStartsAfterDays;
                ((MaximumTaskData)currentlySelectedTask).current = 0;


              


                break;
            case AppManager.TaskType.Minimum:
                MinimumTaskData _new_mntd = taskTypeComponents.GetData(currentlySelectedTask.type) as MinimumTaskData;
                if (_new_mntd == null) { return; }
                ((MinimumTaskData)currentlySelectedTask).targetValue = _new_mntd.targetValue;
                ((MinimumTaskData)currentlySelectedTask).pointsLostPerOne = _new_mntd.pointsLostPerOne;
                ((MinimumTaskData)currentlySelectedTask).pointsForStayingUnderTargetValue = _new_mntd.pointsForStayingUnderTargetValue;
                ((MinimumTaskData)currentlySelectedTask).underTargetValuePercentBonus = _new_mntd.underTargetValuePercentBonus;
                ((MinimumTaskData)currentlySelectedTask).streakStartsAfterDays = _new_mntd.streakStartsAfterDays;
                ((MinimumTaskData)currentlySelectedTask).metric = _new_mntd.metric;
                ((MinimumTaskData)currentlySelectedTask).customMetricName = _new_mntd.customMetricName;
                ((MinimumTaskData)currentlySelectedTask).current = 0;
                break;
            case AppManager.TaskType.Boolean:
                BooleanTaskData _new_btd = taskTypeComponents.GetData(currentlySelectedTask.type) as BooleanTaskData;
                if (_new_btd == null) { return; }
                ((BooleanTaskData)currentlySelectedTask).pointsGained = _new_btd.pointsGained;
                ((BooleanTaskData)currentlySelectedTask).isDone = false;
                

                break;
            case AppManager.TaskType.Optimum:
                OptimumTaskData _new_otd = taskTypeComponents.GetData(currentlySelectedTask.type) as OptimumTaskData;
                if (_new_otd == null) { return; }
                ((OptimumTaskData)currentlySelectedTask).metric = _new_otd.metric;
                ((OptimumTaskData)currentlySelectedTask).customMetricName = _new_otd.customMetricName;
               ((OptimumTaskData)currentlySelectedTask).pointsForOptimum = _new_otd.pointsForOptimum;
                ((OptimumTaskData)currentlySelectedTask).pointsLostPerOneDifference = _new_otd.pointsLostPerOneDifference;
                ((OptimumTaskData)currentlySelectedTask).targetValue = _new_otd.targetValue;
                ((OptimumTaskData)currentlySelectedTask).current = 0;
                ((OptimumTaskData)currentlySelectedTask).streakStartsAfterDays = _new_otd.streakStartsAfterDays;


                break;
            case AppManager.TaskType.Interval:
                IntervalTaskData _new_itd = taskTypeComponents.GetData(currentlySelectedTask.type) as IntervalTaskData;
                if (_new_itd == null) { return; }
                ((IntervalTaskData)currentlySelectedTask).intervals = _new_itd.intervals;
                ((IntervalTaskData)currentlySelectedTask).current = 0;
                ((IntervalTaskData)currentlySelectedTask).metric = _new_itd.metric;
                ((IntervalTaskData)currentlySelectedTask).customMetricName = _new_itd.customMetricName;
                break;
        }

        if (everyThDay > 0)
        {
            currentlySelectedTask.activeEveryThDay = everyThDay;
            currentlySelectedTask.beingActiveType = TaskData.ActiveType.EveryThDay;
            currentlySelectedTask.isActiveToday = true;
        }
        else
        {
            currentlySelectedTask.beingActiveType = TaskData.ActiveType.DayOfWeek;
            for (int i = 0; i < selectedActiveDays.Count; i++)
            {
                if(selectedActiveDays[i] == DateTime.Today.DayOfWeek)
                {
                    currentlySelectedTask.isActiveToday = true;
                }
                else
                {
                    currentlySelectedTask.isActiveToday = false;
                }
                currentlySelectedTask.activeOnDays.Add((int)selectedActiveDays[i]);
            }
        }



        foreach(int _day in currentlySelectedTask.notificationAttachedActiveDay)
        {
            NotificationManager.NotificationData _nd;
            if(NotificationManager.GetNotificationData((DayOfWeek)_day,currentlySelectedTask.name,out _nd))
            {
                NotificationManager.DeleteNotification(_nd);
            }

           
        }


        if (selectedActiveDays.Count > 0)
        {
            currentlySelectedTask.notificationAttachedActiveDay.Clear();


          

            for (int i = 0; i < currentlySelectedTask.activeOnDays.Count; i++)
            {
                if ((int)DateTime.Now.DayOfWeek == currentlySelectedTask.activeOnDays[i])
                {
                    currentlySelectedTask.isActiveToday = true;
                }
            }

            NotificationPrefabUtility[] _npus = notificationHolder.GetNotifications().ToArray();
            for (int i = 0; i < _npus.Length; i++)
            {
                currentlySelectedTask.notificationAttachedActiveDay.Add((int)_npus[i].daySelected);
                int daysUntilNextDay = ((int)_npus[i].daySelected - (int)DateTime.Today.DayOfWeek + 7) % 7;
                DateTime _fireTime = DateTime.Today;
                _fireTime = _fireTime.AddDays(daysUntilNextDay);
                _fireTime = _fireTime.AddHours(_npus[i].hourNumber);
                _fireTime = _fireTime.AddMinutes(_npus[i].minuteNumber);

                string _hour = _fireTime.Hour.ToString().Length == 1 ? "0" + _fireTime.Hour : _fireTime.Hour.ToString();
                string _minute = _fireTime.Minute.ToString().Length == 1 ? "0" + _fireTime.Minute : _fireTime.Minute.ToString();

                print(_fireTime);

               

                NotificationManager.SendNotification(goalManager.GetCurrentlySelectedGoal().GetGoalData().name + 
                    " notification", "Don't forget " + currentlySelectedTask.name + 
                    "! " + _hour + ":" + _minute, _fireTime, 7, currentlySelectedTask.name, goalManager.GetCurrentlySelectedGoal().GetGoalData().spriteId.ToString());
            }


        }
        else if (everyThDay > 0)
        {

            currentlySelectedTask.activeEveryThDay = everyThDay;
            currentlySelectedTask.isActiveToday = true;
            currentlySelectedTask.nextActiveDay = CalculateNextActiveDay(currentlySelectedTask).ToString();



            NotificationManager.NotificationData _nd;

            if (NotificationManager.GetNotificationData_NoDayCheck(currentlySelectedTask.name, out _nd))
            {
                NotificationManager.DeleteNotification(_nd);
                NotificationPrefabUtility[] _npus = notificationHolder.GetNotifications().ToArray();
                if (_npus.Length > 0)
                {
                    if(_npus[0].daySelected != Convert.ToDateTime( _nd.fireTime).DayOfWeek)
                    {

                        currentlySelectedTask.notificationAttachedActiveDay.Add((int)_npus[0].daySelected);
                     //   int daysUntilNextDay = ((int)(Convert.ToDateTime(_nd.fireTime)).DayOfWeek - (int)DateTime.Today.DayOfWeek + 7) % 7;
                        DateTime _fireTime = DateTime.Today.AddDays(everyThDay);
                        _fireTime = _fireTime.AddHours(_npus[0].hourNumber);
                        _fireTime = _fireTime.AddMinutes(_npus[0].minuteNumber);

                        string _hour = _fireTime.Hour.ToString().Length == 1 ? "0" + _fireTime.Hour : _fireTime.Hour.ToString();
                        string _minute = _fireTime.Minute.ToString().Length == 1 ? "0" + _fireTime.Minute : _fireTime.Minute.ToString();

                       

                        print(_fireTime + " " + _fireTime.DayOfWeek);
                        NotificationManager.SendNotification(goalManager.GetCurrentlySelectedGoal().GetGoalData().name
                            + " notification", "Don't forget " + currentlySelectedTask.name +
                            "! " + _hour + ":" + _minute, _fireTime, everyThDay, currentlySelectedTask.name, goalManager.GetCurrentlySelectedGoal().GetGoalData().spriteId.ToString());
                    }
                    else
                    {
                        DateTime _fireTime = Convert.ToDateTime( _nd.fireTime);
                        _fireTime.AddHours(-Convert.ToDateTime(_nd.fireTime).Hour);
                        _fireTime.AddMinutes(-Convert.ToDateTime(_nd.fireTime).Minute);
                        _fireTime = _fireTime.AddHours(_npus[0].hourNumber);
                        _fireTime = _fireTime.AddMinutes(_npus[0].minuteNumber);

                        string _hour = _fireTime.Hour.ToString().Length == 1 ? "0" + _fireTime.Hour : _fireTime.Hour.ToString();
                        string _minute = _fireTime.Minute.ToString().Length == 1 ? "0" + _fireTime.Minute : _fireTime.Minute.ToString();

                       

                        NotificationManager.SendNotification(goalManager.GetCurrentlySelectedGoal().GetGoalData().name
                           + " notification", "Don't forget " + currentlySelectedTask.name +
                           "! " + _hour + ":" + _minute, _fireTime, everyThDay, currentlySelectedTask.name, goalManager.GetCurrentlySelectedGoal().GetGoalData().spriteId.ToString());
                    }
                }
            }
            else
            {


               
            }
        }




        AppManager.TaskEdited(currentlySelectedTask);


        SoundManager.PlaySound1();
    }

    public void DisplayTaskTypeText(AppManager.TaskType _taskType)
    {
        taskType = _taskType;
        DisableTypeTexts();



        taskTypeSelected = true;

        if (enteredName == "")
        {
            TaskNameUpdateNeeded?.Invoke("?");
        }
        else
        {
            TaskNameUpdateNeeded?.Invoke(enteredName);
        }

        string _targetValue = taskTypeComponents.GetTargetValue();
        if(_targetValue == "")
        {
            _targetValue = "?";
        }

        switch (_taskType)
        {
            case AppManager.TaskType.Maximum:
                maximumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(_targetValue);
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Minimum:
                minimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(_targetValue);
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Boolean:
                booleanTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(_targetValue);
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Optimum:
                optimumTexts.SetActive(true);
                targetValueTexts.SetActive(true);
                intervalMeasureTexts.SetActive(false);

                TargetValueUpdateNeeded?.Invoke(_targetValue);
                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetNonIntervalMetric());
                break;
            case AppManager.TaskType.Interval:
                intervalTexts.SetActive(true);
                targetValueTexts.SetActive(false);
                intervalMeasureTexts.SetActive(true);

                MetricUpdateNeeded?.Invoke(taskTypeComponents.GetIntervalMetric());
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

        if (enteredName.Length > AppManager.MAXNAMESIZE)
        { AppManager.ErrorHappened(ErrorMessages.NameTooLong()); return; }

        if (taskTypeSelected == false) { AppManager.ErrorHappened(ErrorMessages.TaskTypeNotSelected_CreateTaskPanel());  return; }

        TaskData _data = taskTypeComponents.GetData(taskType);
        if (_data == null) { return; } // error message is inside GetData() function

        if (selectedActiveDays.Count == 0 && everyThDay == 0) { AppManager.ErrorHappened(ErrorMessages.DaysNotSelected_CreateTaskPanel()); return; }

        if (goalManager.IsNameAlreadyTaken(enteredName))
        { AppManager.ErrorHappened(ErrorMessages.NameIsTaken()); return; }



      


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
                DateTime _fireTime = DateTime.Today;
                _fireTime = _fireTime.AddDays(daysUntilNextDay);
                _fireTime = _fireTime.AddHours(_npus[i].hourNumber);
                _fireTime = _fireTime.AddMinutes(_npus[i].minuteNumber);

                string _hour = _fireTime.Hour.ToString().Length == 1 ? "0" + _fireTime.Hour : _fireTime.Hour.ToString();
                string _minute = _fireTime.Minute.ToString().Length == 1 ? "0" + _fireTime.Minute : _fireTime.Minute.ToString();

                print(_fireTime);
                NotificationManager.SendNotification(goalManager.GetCurrentlySelectedGoal().GetGoalData().name
                    + " notification", "Don't forget " + _data.name + 
                    "! " + _hour + ":" + _minute, _fireTime, 7, _data.name, goalManager.GetCurrentlySelectedGoal().GetGoalData().spriteId.ToString());
            }


        }
        else if(everyThDay > 0)
        {
            _data.beingActiveType = TaskData.ActiveType.EveryThDay;
            _data.activeEveryThDay = everyThDay;
            _data.isActiveToday = true;
            _data.nextActiveDay = CalculateNextActiveDay(_data).ToString();


            NotificationPrefabUtility[] _npus = notificationHolder.GetNotifications().ToArray();
            if (_npus.Length > 0)
            {

                _data.notificationAttachedActiveDay.Add((int)DateTime.Today.DayOfWeek);

                DateTime _fireTime = DateTime.Today.AddDays(everyThDay);
                _fireTime = _fireTime.AddHours(_npus[0].hourNumber);
                _fireTime = _fireTime.AddMinutes(_npus[0].minuteNumber);

                string _hour = _fireTime.Hour.ToString().Length == 1 ? "0" + _fireTime.Hour : _fireTime.Hour.ToString();
                string _minute = _fireTime.Minute.ToString().Length == 1 ? "0" + _fireTime.Minute : _fireTime.Minute.ToString();

                print(_fireTime + " " + _fireTime.DayOfWeek);
                NotificationManager.SendNotification(goalManager.GetCurrentlySelectedGoal().GetGoalData().name
                    + " notification", "Don't forget " + _data.name +
                    "! " + _hour + ":" + _minute, _fireTime, everyThDay, _data.name, goalManager.GetCurrentlySelectedGoal().GetGoalData().spriteId.ToString());
            }
            
        }



        notificationHolder.Clear();

        

        goalManager.AssignTaskToCurrentGoal(_data);
       

        AppManager.NewTaskAdded();

        SoundManager.PlaySound4();

    //    print(_data.name + " " + _data.activeOnDays.Count);
    }

    private DateTime CalculateNextActiveDay(TaskData _data)
    {
       return DateTime.Now.Date.AddDays(_data.activeEveryThDay);
    }

    public void RemoteCall_SetSelectedName()
    {
        enteredName = taskNameField.text;
        TaskNameUpdateNeeded?.Invoke(enteredName);
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
              //  print(_day);
                everyThDayInputField.text = "";
                everyThDay = 0;

                SoundManager.PlaySound2();
            }

        }
        else
        {
            if (selectedActiveDays.Contains(_day))
            {
                selectedActiveDays.Remove(_day);
                if(notificationHolder.HasDay(_day))
                {
                    notificationHolder.DeleteNotification(_day);
                }

                SoundManager.PlaySound3();
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

        SoundManager.PlaySound2();
    }

    public void NotificationDaySelected(DayOfWeek _day)
    {
        notificationHolder.CreateNewNotification(_day);
        for (int i = 0; i < notificationDaySelectorWindow.transform.childCount; i++)
        {
           Destroy(notificationDaySelectorWindow.transform.GetChild(i).gameObject);
        }
        notificationDaySelectorPanel.SetActive(false);

        SoundManager.PlaySound2();
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




    public bool TaskExists(string _task)
    {
        GoalData[] _datas = goalManager.GetExistingGoals();
        for (int i = 0; i < _datas.Length; i++)
        {
            if(_datas[i].TaskExists(_task))
            {
                return true;
            }
        }

        return false;
    }

   


    public void DeleteTask()
    {
        NotificationManager.DeleteNotificationAttachedToTask(currentlySelectedTask.name);
        goalManager.GetCurrentlySelectedGoal().GetGoalData().tasks.Remove(currentlySelectedTask);

        AppManager.TaskEdited(currentlySelectedTask);
        SoundManager.PlaySound5();
    }

    public void ResetTask()
    {
        List<GoalChange> _modificationsOfResetTask = new List<GoalChange>();
        GoalData _currentGoal = goalManager.GetCurrentlySelectedGoal().GetGoalData();
        for (int i = 0; i < _currentGoal.modifications.Count; i++)
        {
           if(_currentGoal.modifications[i].taskName == currentlySelectedTask.name)
           {
                _modificationsOfResetTask.Add(_currentGoal.modifications[i]);
                _currentGoal.modifications.Remove(_currentGoal.modifications[i]);
           }
        }

        for (int j = 0; j < _currentGoal.dailyScores.Count; j++)
        {
            for (int k = 0; k < _modificationsOfResetTask.Count; k++)
            {
                if (_currentGoal.dailyScores[j].GetDateTime() == _modificationsOfResetTask[k].GetDateTime())
                {
                    _currentGoal.dailyScores[j] = new ScorePerDay(_currentGoal.dailyScores[j].amount - _modificationsOfResetTask[k].amount, _currentGoal.dailyScores[j].GetDateTime(),_currentGoal.dailyScores[j].targetsReached);
                }
            }
        }


        switch (currentlySelectedTask.type)
        {
            case AppManager.TaskType.Maximum:
                ((MaximumTaskData)currentlySelectedTask).current = 0;
                break;
            case AppManager.TaskType.Minimum:
                ((MinimumTaskData)currentlySelectedTask).current = 0;
                break;
            case AppManager.TaskType.Boolean:
                ((BooleanTaskData)currentlySelectedTask).isDone = false;
                break;
            case AppManager.TaskType.Optimum:
                ((OptimumTaskData)currentlySelectedTask).current = 0;
                break;
            case AppManager.TaskType.Interval:
                ((IntervalTaskData)currentlySelectedTask).current = 0;
                break;
        }

        currentlySelectedTask.isEditedToday = false;


        AppManager.TaskEdited(currentlySelectedTask);

        SoundManager.PlaySound1();

        // remove from daily score where modification task equals current (could also remove those modifications)
    }



    public void ShowInfoPanel(string _text)
    {
        infoPanelText.text = _text;
        infoPanel.SetActive(true);
    }

    public void RemoteCall_HideInfoPanel()
    {
        HideInfoPanel();

        SoundManager.PlaySound3();
    }

    private void HideInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    private void LanguageChangedCallback(AppManager.Languages _lang)
    {
        Invoke( nameof(ChangeMetricDropdownLanguage), 0.2f);

    }

    private void ChangeMetricDropdownLanguage()
    {

        intervalMetricDropdown.ClearOptions();
        valueMetricDropdown.ClearOptions();

        List<string> _metrics = new List<string>();
        for (int i = 0; i < (int)AppManager.TaskMetricType.ENUM_END; i++)
        {
            _metrics.Add(RuntimeTranslator.TranslateTaskMetricType((AppManager.TaskMetricType)i));
        }
        valueMetricDropdown.AddOptions(_metrics);
        intervalMetricDropdown.AddOptions(_metrics);
    }

}
