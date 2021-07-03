using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class NotificationDaySelectButton : BehaviourButton
{
    public DayOfWeek day;
    [SerializeField] private TMP_Text dayText;
    private IntervalHolder intervalHolder;

    private TaskManager taskManager;

    private void Awake()
    {
        
        intervalHolder = FindObjectOfType<IntervalHolder>();
    }

    public void SetData(DayOfWeek _day, TaskManager _taskManager)
    {
        day = _day;
        dayText.text = RuntimeTranslator.TranslateDayOfWeek(day);
        taskManager = _taskManager;
    }

    protected override void OnTouch()
    {
        taskManager.NotificationDaySelected(day);
    }




}
