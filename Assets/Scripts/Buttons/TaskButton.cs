using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskButton : BehaviourButton
{
    private TaskManager taskManager = null;
    private Task task;

   [SerializeField] private GoalPanelScroll goalPanelScroll = null;

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
        task = GetComponent<Task>();
    }


    protected override void OnTouch()
    {
        if (!Application.isEditor)
        {
            goalPanelScroll.FeedClickPosition(Input.GetTouch(0).position);
        }
        else
        {
            goalPanelScroll.FeedClickPosition(Input.mousePosition);
        }
    }

    protected override void OnRelease()
    {
       Invoke(nameof( CheckIfDragging),Time.deltaTime);
    }

    private void CheckIfDragging()
    {

        if (goalPanelScroll.allowInteraction)
        {
            AppManager.TaskMenuOpened();
            taskManager.EditTask(task.GetTaskData());
            
        }
    }

}
