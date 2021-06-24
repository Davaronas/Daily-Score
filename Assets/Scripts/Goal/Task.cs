using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Task : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText = null;
    [SerializeField] private TMP_Text scoreText = null;

    [SerializeField] private TMP_InputField currentValueInputField = null;
    [SerializeField] private TMP_Text targetValueText = null;
    [SerializeField] private Toggle booleanTaskTypeToggle = null;
    [Space]
    private RectTransform scoreTextRectTransform;
    [Space]
    [Space]
    [Space]
    [SerializeField] private int scoreTextSizeIncreasePercent = 20;
    [SerializeField] private float animationSpeed = 0.2f;

    private Vector2 scoreTextOriginalSize;

    public bool isPrefab = false;

    private TaskData taskData;

    private MaximumTaskData mxtd;
    private MinimumTaskData mntd;
    private BooleanTaskData btd;
    private OptimumTaskData otd;
    private IntervalTaskData itd;

    private int currentPoint;

    private Coroutine nextTransition;

    private GoalManager goalManager;


   

    public void FeedData(TaskData _data, GoalManager _gm) // we need the goalamanger here because the goalmanager gameobject is disabled at this point
    {
        goalManager = _gm;


        currentValueInputField.gameObject.SetActive(false);
        targetValueText.gameObject.SetActive(false);
        booleanTaskTypeToggle.isOn = false;
        booleanTaskTypeToggle.gameObject.SetActive(false);

        scoreTextRectTransform = scoreText.GetComponent<RectTransform>();
        scoreTextOriginalSize = scoreTextRectTransform.sizeDelta;

        taskData = _data;

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

        nameText.text = taskData.name;

    }

    private void SetScoreText(int _cp)
    {
        

        if (_cp == 0)
        {
            scoreText.text = "-";
        }
        else
        {
            if (gameObject.activeInHierarchy)
            {
                LTDescr _des = LT_Animator.SizeTransition(scoreTextRectTransform, scoreTextOriginalSize -
                    new Vector2(scoreTextOriginalSize.x * ((float)scoreTextSizeIncreasePercent / 100), scoreTextOriginalSize.y * ((float)scoreTextSizeIncreasePercent / 100)), animationSpeed);
                nextTransition = StartCoroutine(GoBackToNormalSize(_des.time));
            }
            scoreText.text = currentPoint + " p";
        }

        GoalData _gd;
        if(goalManager.SearchGoalByName(taskData.owner, out _gd))
        {
            _gd.AddModification(_cp);
            AppManager.TaskValueChanged(taskData);
        }
        


           

    }

  

    IEnumerator GoBackToNormalSize(float _time)
    {
        if(nextTransition != null) {yield break; }
        yield return new WaitForSeconds(_time);
        LT_Animator.SizeTransition(scoreTextRectTransform, scoreTextOriginalSize, animationSpeed);
        nextTransition = null;
    }

    private void HandleMaxDataType(TaskData _data)
    {
        MaximumTaskData _mtd = _data as MaximumTaskData;
        mxtd = _mtd;

        currentValueInputField.gameObject.SetActive(true);
        targetValueText.gameObject.SetActive(true);

        currentValueInputField.text = _mtd.current.ToString();
        targetValueText.text = "/" + _mtd.targetValue + " " + RuntimeTranslator.TranslateTaskMetricType(_mtd.metric);

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_mtd);
        SetScoreText(currentPoint);

    }

    private void HandleMinDataType(TaskData _data)
    {
        MinimumTaskData _mtd = _data as MinimumTaskData;
        mntd = _mtd;

        currentValueInputField.gameObject.SetActive(true);
        targetValueText.gameObject.SetActive(true);

        currentValueInputField.text = _mtd.current.ToString();
        targetValueText.text = "/" + _mtd.targetValue + " " + RuntimeTranslator.TranslateTaskMetricType(_mtd.metric);

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_mtd);
        SetScoreText(currentPoint);

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

    }

    private void HandleOptimumDataType(TaskData _data)
    {
        OptimumTaskData _otd = _data as OptimumTaskData;
        otd = _otd;

        currentValueInputField.gameObject.SetActive(true);
        targetValueText.gameObject.SetActive(true);

        currentValueInputField.text = _otd.current.ToString();
        targetValueText.text = "/" + _otd.targetValue + " " + RuntimeTranslator.TranslateTaskMetricType(_otd.metric);

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_otd);
        SetScoreText(currentPoint);

    }

    private void HandleIntervalDataType(TaskData _data)
    {
        IntervalTaskData _itd = _data as IntervalTaskData;
        itd = _itd;

        currentValueInputField.gameObject.SetActive(true);
        currentValueInputField.text = _itd.current.ToString();

        currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_itd);
        SetScoreText(currentPoint);

    }


    public void RemoteCall_TaskEdited()
    {
        
        if(taskData == null) { return; } // For some reason this runs earlier than FeedData, investigate further

        bool _hasDifference = false;
        int _parse = 0;

        switch (taskData.type)
        {
            case AppManager.TaskType.Maximum:
                if (currentValueInputField.text != "")
                {
                    _parse = int.Parse(currentValueInputField.text);
                    if (mxtd.current != _parse) { _hasDifference = true; }
                    mxtd.current = _parse;
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(mxtd);
                }
               
                break;
            case AppManager.TaskType.Minimum:
                if (currentValueInputField.text != "")
                {
                    _parse = int.Parse(currentValueInputField.text);
                    if (mntd.current != _parse) { _hasDifference = true; }
                    mntd.current = _parse;
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(mntd);
                }
                
                break;
            case AppManager.TaskType.Boolean:
                _hasDifference = true;
                btd.isDone = booleanTaskTypeToggle.isOn;
                currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(btd);
                break;
            case AppManager.TaskType.Optimum:
                if (currentValueInputField.text != "")
                {
                    _parse = int.Parse(currentValueInputField.text);
                    if (otd.current != _parse) { _hasDifference = true; }
                    otd.current = _parse;
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(otd);
                }
                
                break;
            case AppManager.TaskType.Interval:
                if (currentValueInputField.text != "")
                {
                    _parse = int.Parse(currentValueInputField.text);
                    if (itd.current != _parse) { _hasDifference = true; }
                    itd.current = _parse;
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(itd);
                }
                
                break;

            default:
                Debug.LogError("No datatype has been discovered");
                break;

        }

        if(_hasDifference)
        SetScoreText(currentPoint);
    }


    public TaskData GetTaskData()
    {
        return taskData;
    }
}
