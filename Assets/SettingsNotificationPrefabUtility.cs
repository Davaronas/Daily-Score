using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsNotificationPrefabUtility : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;

    private TaskData task = null;


    private string name = "";
    private int everyThDay = -1;
    private int hour = 12;
    private int minute = 0;
    private DayOfWeek day = DayOfWeek.Monday;

    private void Awake()
    {
        AppManager.OnLanguageChanged += UpdateTextCallback;
        AppManager.OnNewTaskAdded += UpdateTextCallback;
        AppManager.OnTaskEdited += UpdateTextCallback;
    }

    private void UpdateTextCallback(AppManager.Languages _l)
    {

    }

    private void UpdateTextCallback()
    {

    }

    private void UpdateText()
    {
        if(everyThDay == -1)
        {

        }
        else
        {

        }
    }

    public void SetData(string _name, int _everyThDay, int _hour, int _minute)
    {
        name = _name;
        everyThDay = _everyThDay;
        hour = _hour;
        minute = _minute;
    }

    public void SetData(string _name, DayOfWeek _day, int _hour, int _minute)
    {
        name = _name;
        day = _day;
        hour = _hour;
        minute = _minute;
        everyThDay = -1;
    }
        
}
