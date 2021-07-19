using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetTaskButton : BehaviourButton
{
    private TaskManager taskManager = null;
    [SerializeField] private Image imageToFill = null;
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
        if (isFilling)
        {
            imageToFill.fillAmount += fillSpeed;

            if (imageToFill.fillAmount >= 1)
            {
                taskManager.ResetTask();
                isFilling = false;
                imageToFill.fillAmount = 0;
            }
        }
    }

    protected override void OnPointerExit()
    {
        isFilling = false;
        imageToFill.fillAmount = 0;
    }
}