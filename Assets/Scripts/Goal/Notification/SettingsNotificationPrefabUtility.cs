using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsNotificationPrefabUtility : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;


    private TaskData task = null;

    private GoalManager goalManager = null;
    private TaskManager taskManager = null;


    private string taskName = "";
    private int everyThDay = -1;
    private int hour = 12;
    private int minute = 0;
    private DayOfWeek day = DayOfWeek.Monday;

    private void Awake()
    {
        AppManager.OnLanguageChanged += UpdateText;
       
       
    }

    private void OnDestroy()
    {

        AppManager.OnLanguageChanged -= UpdateText;
    }




    private void UpdateText(AppManager.Languages _l)
    {
        if(everyThDay == -1)
        {
            SetData(taskName, day, hour, minute, goalManager, taskManager);
        }
        else
        {
            SetData(taskName, everyThDay, hour, minute, goalManager, taskManager);
        }
    }

    public void SetData(string _name, int _everyThDay, int _hour, int _minute, GoalManager _goalManager, TaskManager _taskManager)
    {
        taskName = _name;
        everyThDay = _everyThDay;
        hour = _hour;
        minute = _minute;

        goalManager = _goalManager;
        taskManager = _taskManager;

        string _h = _hour.ToString().Length == 1 ? "0" + _hour : _hour.ToString();
        string _m = _minute.ToString().Length == 1 ? "0" + _minute : _minute.ToString();

        infoText.text = _name + ": " + _h + ":" + _m + RuntimeTranslator.TranslateEveryWord() + " " + _everyThDay + ". " + RuntimeTranslator.TranslateDayWord();
    }

    public void SetData(string _name, DayOfWeek _day, int _hour, int _minute, GoalManager _goalManager, TaskManager _taskManager)
    {
        taskName = _name;
        day = _day;
        hour = _hour;
        minute = _minute;
        everyThDay = -1;

        goalManager = _goalManager;
        taskManager = _taskManager;

        string _h = _hour.ToString().Length == 1 ? "0" + _hour : _hour.ToString();
        string _m = _minute.ToString().Length == 1 ? "0" + _minute : _minute.ToString();

        infoText.text = _name + ": " + _h + ":" + _m + " " + RuntimeTranslator.TranslateDayOfWeek(_day);
    }



    public void EditTask()
    {
        TaskData _td = null;

       if( goalManager.SearchTaskByName(taskName,out _td))
        {
            goalManager.ClearCurrentlySelectedGoal();
            AppManager.TaskMenuOpened();
            taskManager.EditTaskFromSettingsNotification(_td);
        }
    }
   

}
