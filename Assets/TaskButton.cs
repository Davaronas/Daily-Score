using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskButton : BehaviourButton
{
    private TaskManager taskManager = null;
    private Task task;

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
        task = GetComponent<Task>();
    }

    protected override void OnRelease()
    {
        taskManager.EditTask(task.GetTaskData());
        
    }

}
