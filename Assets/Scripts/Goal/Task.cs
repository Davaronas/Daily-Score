using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Task : MonoBehaviour
{
    [SerializeField]private TMP_Text nameText = null;
    [SerializeField] private TMP_Text scoreText = null;

    public bool isPrefab = false;

    private TaskData taskData;

    public void FeedData(TaskData _data)
    {

        taskData = _data;
        nameText.text = taskData.name;
        scoreText.text = "0 / 0";
    }

    public TaskData GetTaskData()
    {
        return taskData;
    }
}
