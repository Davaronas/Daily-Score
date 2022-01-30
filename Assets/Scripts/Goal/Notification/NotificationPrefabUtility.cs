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

    public int hourNumber = 12;
    public int minuteNumber = 0;

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
        daySelected = DateTime.Today.DayOfWeek;
        dayText.text = 
            RuntimeTranslator.TranslateDayOfWeek(DateTime.Today.DayOfWeek) + "\n + " + RuntimeTranslator.TranslateEveryWord() + " " + _day + ". " + RuntimeTranslator.TranslateDayWord();
    }

    public void SetDayPlusIntervalDays(DayOfWeek _day, int _reset)
    {
        daySelected = _day;
        dayText.text = 
            RuntimeTranslator.TranslateDayOfWeek(_day) + "\n + " + RuntimeTranslator.TranslateEveryWord() +" " + _reset +". " + RuntimeTranslator.TranslateDayWord();
    }


    public void RemoteCall_EditHour()
    {
        if(hour.text != "")
        {
            hourNumber = int.Parse(hour.text);
            if(hourNumber > 23 || hourNumber < 0)
            {
                hour.text = "";
                hourNumber = 12;
                AppManager.ErrorHappened(ErrorMessages.EnterRealisticNumbers());
            }
        }
        else
        {
            hourNumber = 12;
        }
    }

    public void RemoteCall_EditMinute()
    {
        if(minute.text != "")
        {
            

            minuteNumber = int.Parse(minute.text);
            if(minuteNumber > 59 || minuteNumber < 0)
            {
                minute.text = "";
                minuteNumber = 0;
                AppManager.ErrorHappened(ErrorMessages.EnterRealisticNumbers());
            }
        }
        else
        {
            minuteNumber = 0;
        }
    }

    public DayOfWeek GetData(out int _hour, out int _minute)
    {
        _hour = hourNumber;
        _minute = minuteNumber;
        return daySelected;
    }

    public void SetHourAndMinute(int _hour, int _minute)
    {
        hour.text = _hour.ToString();
        minute.text = _minute.ToString();

        hourNumber = _hour;
        minuteNumber = _minute;
    }

    public void RemoteCall_DeleteNotification()
    {
        notificationHolder.DeleteNotification(daySelected);

        SoundManager.PlaySound6();
      
    }

}
