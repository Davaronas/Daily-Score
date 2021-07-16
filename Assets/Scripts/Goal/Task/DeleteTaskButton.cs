using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteTaskButton : BehaviourButton
{
    private TaskManager taskManager = null;
    [SerializeField] private Image deleteFill = null;
    private bool isFilling = false;

    [SerializeField] private float fillSpeed = 0.02f;

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    protected override void OnTouch()
    {
        isFilling = true;
    }

    private void Update()
    {
        if(isFilling)
        {
            deleteFill.fillAmount += fillSpeed;

            if(deleteFill.fillAmount >= 1)
            {
                taskManager.DeleteTask();
                isFilling = false;
                deleteFill.fillAmount = 0;
            }
        }
    }

    protected override void OnPointerExit()
    {
        isFilling = false;
        deleteFill.fillAmount = 0;
    }
}
