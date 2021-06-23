using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText = null;
    [SerializeField] private TMP_Text scoreText = null;

    [SerializeField] private TMP_InputField currentValueInputField = null;
    [SerializeField] private TMP_Text targetValueText = null;
    [SerializeField] private Toggle booleanTaskTypeToggle = null;

    public bool isPrefab = false;

    private TaskData taskData;

    private MaximumTaskData mxtd;
    private MinimumTaskData mntd;
    private BooleanTaskData btd;
    private OptimumTaskData otd;
    private IntervalTaskData itd;

    private int currentPoint;


    public void FeedData(TaskData _data)
    {

        currentValueInputField.gameObject.SetActive(false);
        targetValueText.gameObject.SetActive(false);
        booleanTaskTypeToggle.isOn = false;
        booleanTaskTypeToggle.gameObject.SetActive(false);

        switch (_data.type)
        {
            case AppManager.TaskType.Maximum:
                HandleMaxDataType(_data);
                break;
            case AppManager.TaskType.Minimum:
                HandleMinDataType(_data);
                break;
            case AppManager.TaskType.Boolean:
                HandleBoolDataType(_data);
                break;
            case AppManager.TaskType.Optimum:
                HandleOptimumDataType(_data);
                break;
            case AppManager.TaskType.Interval:
                HandleIntervalDataType(_data);
                break;

            default:
                Debug.LogError("No datatype has been discovered");
                break;
        }

        taskData = _data;
        nameText.text = taskData.name;
        scoreText.text = "-"; // TaskPointCalculator getpoint - handle this at switch

    }

    private void SetScoreText(int _cp)
    {
        if (_cp == 0)
        {
            scoreText.text = "-";
        }
        else
        {
            scoreText.text = currentPoint + " p";
        }
    }

    private void HandleMaxDataType(TaskData _data)
    {
        MaximumTaskData _mtd = _data as MaximumTaskData;
        mxtd = _mtd;

        currentValueInputField.gameObject.SetActive(true);
        targetValueText.gameObject.SetActive(true);

        currentValueInputField.text = _mtd.current.ToString();
        targetValueText.text = "/" + _mtd.targetValue + RuntimeTranslator.TranslateTaskMetricType(_mtd.metric);

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_mtd);
        SetScoreText(currentPoint);

        print("mx_td");
    }

    private void HandleMinDataType(TaskData _data)
    {
        MinimumTaskData _mtd = _data as MinimumTaskData;
        mntd = _mtd;

        currentValueInputField.gameObject.SetActive(true);
        targetValueText.gameObject.SetActive(true);

        currentValueInputField.text = _mtd.current.ToString();
        targetValueText.text = "/" + _mtd.targetValue + RuntimeTranslator.TranslateTaskMetricType(_mtd.metric);

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_mtd);
        SetScoreText(currentPoint);

        print("mn_td");
    }

    private void HandleBoolDataType(TaskData _data)
    {
        BooleanTaskData _btd = _data as BooleanTaskData;
        btd = _btd;

        currentValueInputField.gameObject.SetActive(false);
        targetValueText.gameObject.SetActive(false);
        booleanTaskTypeToggle.gameObject.SetActive(true);

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_btd);
        SetScoreText(currentPoint);

        print("btd");
    }

    private void HandleOptimumDataType(TaskData _data)
    {
        OptimumTaskData _otd = _data as OptimumTaskData;
        otd = _otd;

        currentValueInputField.gameObject.SetActive(true);
        targetValueText.gameObject.SetActive(true);

        currentValueInputField.text = _otd.current.ToString();
        targetValueText.text = "/" + _otd.targetValue + RuntimeTranslator.TranslateTaskMetricType(_otd.metric);

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_otd);
        SetScoreText(currentPoint);

        print("otd");
    }

    private void HandleIntervalDataType(TaskData _data)
    {
        IntervalTaskData _itd = _data as IntervalTaskData;
        itd = _itd;

        currentValueInputField.gameObject.SetActive(true);
        currentValueInputField.text = _itd.current.ToString();

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_itd);
        SetScoreText(currentPoint);

        print("itd");
    }


    public void RemoteCall_TaskEdited()
    {
        
        if(taskData == null) { return; } // For some reason this runs earlier than FeedData, investigate further

        if(currentValueInputField.text != "")
        {

        }

       

        switch (taskData.type)
        {
            case AppManager.TaskType.Maximum:
                if (currentValueInputField.text != "")
                {
                    mxtd.current = int.Parse(currentValueInputField.text);
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(mxtd);
                }
               
                break;
            case AppManager.TaskType.Minimum:
                if (currentValueInputField.text != "")
                {
                    mntd.current = int.Parse(currentValueInputField.text);
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(mntd);
                }
                
                break;
            case AppManager.TaskType.Boolean:
                btd.isDone = booleanTaskTypeToggle.isOn;
                currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(btd);
                break;
            case AppManager.TaskType.Optimum:
                if (currentValueInputField.text != "")
                {
                    otd.current = int.Parse(currentValueInputField.text);
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(otd);
                }
                
                break;
            case AppManager.TaskType.Interval:
                if (currentValueInputField.text != "")
                {
                    itd.current = int.Parse(currentValueInputField.text);
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(itd);
                }
                
                break;

            default:
                Debug.LogError("No datatype has been discovered");
                break;

        }

        SetScoreText(currentPoint);
    }


    public TaskData GetTaskData()
    {
        return taskData;
    }
}
