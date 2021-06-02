using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTypeSelectButton : BehaviourButton
{
    [SerializeField] private AppManager.TaskType taskType;

    private TaskManager taskManager = null;

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    public AppManager.TaskType GetTaskType()
    {
        return taskType;
    }

    protected override void OnTouch()
    {
        taskManager.DisplayTaskTypeText(taskType);
    }
}
