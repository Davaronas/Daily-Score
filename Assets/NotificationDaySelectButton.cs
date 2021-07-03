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
        taskManager = FindObjectOfType<TaskManager>();
        intervalHolder = FindObjectOfType<IntervalHolder>();
    }

    public void SetData(DayOfWeek _day)
    {
        day = _day;
        dayText.text = RuntimeTranslator.TranslateDayOfWeek(day);
    }

    protected override void OnTouch()
    {
       
    }




}
