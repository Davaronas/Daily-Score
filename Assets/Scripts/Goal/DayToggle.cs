using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DayToggle : MonoBehaviour
{
    [SerializeField] private DayOfWeek day;
    [SerializeField] private Toggle toggle;

    private TaskManager taskManager;

    private bool ignoreSendMessage = false;

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    public void RemoteCall_SetDay()
    {
        if (!ignoreSendMessage)
        {
            taskManager.SetActiveDays(day, toggle.isOn);
        }
        else
        {
            ignoreSendMessage = false;
        }
    }

    public void TurnOff()
    {
        toggle.isOn = false;
        ignoreSendMessage = true;
    }

    public void TurnOn()
    {
        toggle.isOn = true;
    }

    public DayOfWeek GetDay()
    {
        return day;
    }
}
