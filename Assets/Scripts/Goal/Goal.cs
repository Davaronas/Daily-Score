using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText = null;
    [SerializeField] private TMP_Text scoreText = null;
    [SerializeField] private Image imageToApplyColorTo = null;

    public bool isPrefab = false;

    private const byte defaultTransparency = 255;


    public string goalName { get; private set; } = "Default name";
    public Color32 goalColor { get; private set; } = new Color32(0, 0, 0, defaultTransparency);
    public int symbolId { get; private set; } = -1;

    private GoalData goalData = new GoalData();


    
    public void SetData(GoalData _goalData)
    {

        if (nameText == null || scoreText == null || imageToApplyColorTo == null)
        {
            Debug.LogError("Goal not setup properly, please ensure you set all components from the editor");
            return;
        }

        goalData = _goalData;

        goalName = _goalData.name;
        goalColor = (Color32)_goalData.color;
        symbolId = _goalData.spriteId;
     //   tasks = _goalData.tasks;

        Initialize();
    }

    private void Initialize()
    {
        nameText.text = goalName;
        scoreText.text = "0" + " Points";
        imageToApplyColorTo.color = goalColor;

        // set up sprites
    }

    public void AddTask(TaskData _task)
    {
        print("Add Task " + _task.name);
        

        goalData.tasks.Add(_task);

        foreach (TaskData _d in goalData.tasks)
        {
            print(_d.name);
        }
    }

    public void UpdateTask(TaskData _taskToUpdate)
    {

    }


    public TaskData[] GetTasks()
    {
        

        return goalData.tasks.ToArray();
    }

    public GoalData GetGoalData()
    {
        return goalData;
    }
}
