using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private RectTransform tasksScrollContentRectTransform = null;
    [SerializeField] private GameObject taskPrefab = null;

    private string enteredName = "default";
    private AppManager.TaskType taskType = 0;

   [SerializeField] private TMP_InputField taskNameField = null;

    private List<Task> currentTasks = new List<Task>();


    private GoalManager goalManager = null;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
    }

    private void OnDisable()
    {
        enteredName = "";
        taskNameField.text = "";
        DestroyTasks();
        currentTasks.Clear();
    }

    public void DisplayTasks(TaskData[] _tasks)
    {
        for(int i = 0; i < _tasks.Length;i++)
        {
            Task _newTask =
                Instantiate(taskPrefab, Vector3.zero, Quaternion.identity, tasksScrollContentRectTransform.transform).GetComponent<Task>();
            _newTask.FeedData(_tasks[i]);
            _newTask.isPrefab = false;
            currentTasks.Add(_newTask);
        }

        // resize task scroll to fit tasks
    }

    public void DestroyTasks()
    {

       
        for (int i = 0; i < currentTasks.Count;i++)
        {
            if (!currentTasks[i].isPrefab)
            {
                print(i);
                Destroy(currentTasks[i].gameObject);
            }
        }
    }

    public void RemoteCall_CreateNewTask()
    {
        if (goalManager.GetCurrentlySelectedGoal() == null)
        {
            Debug.LogError("Trying to create a task without a selected goal");
            return;
        }

        if (enteredName == "") { return; }

       TaskData _data = new TaskData(enteredName, (int)taskType, 0, 0);
        goalManager.AssignTaskToCurrentGoal(_data);
       

        AppManager.NewTaskAdded();
    }


    public void RemoteCall_SetSelectedName()
    {
        enteredName = taskNameField.text;
    }


}
