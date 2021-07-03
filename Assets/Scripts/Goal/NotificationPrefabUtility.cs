using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NotificationPrefabUtility : MonoBehaviour
{
    public TMP_Text dayText;
    public TMP_InputField hour;
    public TMP_InputField minute;

    private int hourNumber;
    private int minuteNumber;

    public DayOfWeek daySelected;

    NotificationHolder notificationHolder = null;
    private void Awake()
    {
        notificationHolder = FindObjectOfType<NotificationHolder>();
    }

    public void SetSelectedDay(DayOfWeek _day)
    {
        daySelected = _day;
        dayText.text = RuntimeTranslator.TranslateDayOfWeek(_day);
    }

    public void SetSelectedDay(int _day)
    {
        dayText.text = _day + "." + RuntimeTranslator.TranslateDayOfWeek(DateTime.Today.DayOfWeek);
    }


    public void EditHour()
    {
        if(hour.text != "")
        {
            hourNumber = int.Parse(hour.text);
        }
    }

    public void EditMinute()
    {
        if(minute.text != "")
        {
            minuteNumber = int.Parse(minute.text);
        }
    }

    public DayOfWeek GetData(out int _hour, out int _minute)
    {
        _hour = hourNumber;
        _minute = minuteNumber;
        return daySelected;
    }

}
