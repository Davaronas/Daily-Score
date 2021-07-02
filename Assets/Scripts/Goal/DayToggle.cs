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

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    public void RemoteCall_SetDay()
    {
        taskManager.SetActiveDays(day, toggle.isOn);
    }

    public void TurnOff()
    {
        toggle.isOn = false;
    }
}
