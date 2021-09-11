using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTypeSelectButton : BehaviourButton
{
    [SerializeField] private AppManager.TaskType taskType;

    private TaskManager taskManager = null;
    private CreateTaskPanelBroadcaster broadcasterScroll = null;
    private TaskTypeSelectButton[] taskTypeSelectButtons;

   [SerializeField] private Color selectedColor = new Color(200, 200, 200);

    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
        broadcasterScroll = FindObjectOfType<CreateTaskPanelBroadcaster>();

        taskTypeSelectButtons = FindObjectsOfType<TaskTypeSelectButton>();
    }

    public AppManager.TaskType GetTaskType()
    {
        return taskType;
    }


    protected override void OnRelease()
    {
        if(!broadcasterScroll.isBeingDragged)
        {
            taskManager.DisplayTaskTypeText(taskType);

            // TEMPORARY
            GetComponent<Image>().color = selectedColor;

            foreach(TaskTypeSelectButton _b in taskTypeSelectButtons)
            {
                if(_b != this)
                _b.GetComponent<Image>().color = Color.white;
            }

            SoundManager.PlaySound2();
        }
    }

    private void OnDisable()
    {
        // TEMPORARY
        GetComponent<Image>().color = Color.white;
    }
}
