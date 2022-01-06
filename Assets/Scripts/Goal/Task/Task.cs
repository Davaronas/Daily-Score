using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Task : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText = null;
    [SerializeField] private TMP_Text scoreText = null;

    [SerializeField] private TMP_InputField currentValueInputField = null;
    [SerializeField] private TMP_Text targetValueText = null;
    [SerializeField] private Toggle booleanTaskTypeToggle = null;
    [SerializeField] private Image inactivePanel = null;
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

    [Space]
    [SerializeField] private MaximumTaskData mxtd;
    [SerializeField] private MinimumTaskData mntd;
    [SerializeField] private BooleanTaskData btd;
    [SerializeField] private OptimumTaskData otd;
    [SerializeField] private IntervalTaskData itd;

    private int currentPoint;

    private Coroutine nextTransition;

    private GoalManager goalManager;

    private bool allowValueChange = false;

  

    


    public void FeedData(TaskData _data, GoalManager _gm) // we need the goalamanger here because the goalmanager gameobject is disabled at this point
    {
        goalManager = _gm;


        currentValueInputField.gameObject.SetActive(false);
        targetValueText.gameObject.SetActive(false);
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

        if(!_data.isActiveToday)
        {
            inactivePanel.gameObject.SetActive(true);
            currentValueInputField.interactable = false;
            booleanTaskTypeToggle.interactable = false;
        }
        else
        {
            inactivePanel.gameObject.SetActive(false);
            currentValueInputField.interactable = true;
            booleanTaskTypeToggle.interactable = true;
        }

        allowValueChange = true;

    }

    private void SetScoreText(int _cp)
    {

        if (!taskData.isEditedToday)
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

    }

    private void ModificationHappened(int _cp, string _task)
    {
        GoalData _gd;
        if (goalManager.SearchGoalByName(taskData.owner, out _gd))
        {
            _gd.AddModification(_cp, _task);
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

        if (_mtd.metric != AppManager.TaskMetricType.Other)
        {
            targetValueText.text = "/" + _mtd.targetValue + " " + RuntimeTranslator.TranslateTaskMetricType(_mtd.metric);
        }
        else
        {
            targetValueText.text = "/" + _mtd.targetValue + " " + _mtd.customMetricName;
        }



        if (taskData.lastChangedValue == "")
        {
            taskData.lastChangedValue = DateTime.MinValue.ToString();
        }


        if (Convert.ToDateTime(taskData.lastChangedValue) >= DateTime.Today)
        {
          currentValueInputField.text = _mtd.current.ToString();
            currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_mtd);
        }



        SetScoreText(currentPoint);

    }

    private void HandleMinDataType(TaskData _data)
    {
        MinimumTaskData _mtd = _data as MinimumTaskData;
        mntd = _mtd;

        currentValueInputField.gameObject.SetActive(true);
        targetValueText.gameObject.SetActive(true);


        if (_mtd.metric != AppManager.TaskMetricType.Other)
        {
            targetValueText.text = "/" + _mtd.targetValue + " " + RuntimeTranslator.TranslateTaskMetricType(_mtd.metric);
        }
        else
        {
            targetValueText.text = "/" + _mtd.targetValue + " " + _mtd.customMetricName;
        }

        if (taskData.lastChangedValue == "")
        {
            taskData.lastChangedValue = DateTime.MinValue.ToString();
        }

        if (Convert.ToDateTime(taskData.lastChangedValue) >= DateTime.Today)
        {
            currentValueInputField.text = _mtd.current.ToString();
            currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_mtd);
        }


        SetScoreText(currentPoint);



    }

    private void HandleBoolDataType(TaskData _data)
    {
        BooleanTaskData _btd = _data as BooleanTaskData;
        btd = _btd;

        currentValueInputField.gameObject.SetActive(false);
        targetValueText.gameObject.SetActive(false);
        booleanTaskTypeToggle.gameObject.SetActive(true);
        if(btd.isDone == true)
        {
            booleanTaskTypeToggle.isOn = true;
        }
        else
        {
            booleanTaskTypeToggle.isOn = false;
        }

        if(taskData.lastChangedValue == "")
        {
            taskData.lastChangedValue = DateTime.MinValue.ToString();
        }

        if (Convert.ToDateTime(taskData.lastChangedValue) >= DateTime.Today)
        {
            currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_btd);
        }
        SetScoreText(currentPoint);


    }

    private void HandleOptimumDataType(TaskData _data)
    {
        OptimumTaskData _otd = _data as OptimumTaskData;
        otd = _otd;

        currentValueInputField.gameObject.SetActive(true);
        targetValueText.gameObject.SetActive(true);


        if (_otd.metric != AppManager.TaskMetricType.Other)
        {
            targetValueText.text = "/" + _otd.targetValue + " " + RuntimeTranslator.TranslateTaskMetricType(_otd.metric);
        }
        else
        {
            targetValueText.text = "/" + _otd.targetValue + " " + _otd.customMetricName;
        }


        if (taskData.lastChangedValue == "")
        {
            taskData.lastChangedValue = DateTime.MinValue.ToString();
        }

        if (Convert.ToDateTime(taskData.lastChangedValue) >= DateTime.Today)
        {
        currentValueInputField.text = _otd.current.ToString();
            currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_otd);
        }


        SetScoreText(currentPoint);

    }

    private void HandleIntervalDataType(TaskData _data)
    {
        IntervalTaskData _itd = _data as IntervalTaskData;
        itd = _itd;

        currentValueInputField.gameObject.SetActive(true);


        if (taskData.lastChangedValue == "")
        {
            taskData.lastChangedValue = DateTime.MinValue.ToString();
        }


        if (Convert.ToDateTime(taskData.lastChangedValue) >= DateTime.Today)
        {
            currentValueInputField.text = _itd.current.ToString();
            currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(_itd);
        }

        SetScoreText(currentPoint);

    }


    public void RemoteCall_CheckEnteredCharacter()
    {
        if (taskData == null || !allowValueChange) { return; }

        if (currentValueInputField.text.Length >0)
        {

            int _index = currentValueInputField.text.Length - 1;
            char _last = currentValueInputField.text[_index];

            char[] _cArray = currentValueInputField.text.ToCharArray();
            int _commas = 0;
            for (int i = 0; i < _cArray.Length; i++)
            {
                if (i == 0)
                {
                    if (_cArray[0] == '.' || _cArray[i] == ',')
                    {
                        currentValueInputField.text = "";
                        currentValueInputField.OnDeselect(new BaseEventData(EventSystem.current));
                        AppManager.ErrorHappened(ErrorMessages.EnterRealisticNumbers());
                        break;
                    }
                }

                if(_cArray[i] == '.' || _cArray[i] == ',')
                {
                    _commas++;
                    if(_commas > 1)
                    {
                        currentValueInputField.text = "";
                        currentValueInputField.OnDeselect(new BaseEventData(EventSystem.current));
                        AppManager.ErrorHappened(ErrorMessages.EnterRealisticNumbers());
                        break;
                    }
                }
            }
          //  int _commaCount = currentValueInputField.text.T
            
            print(_last);
            if(!char.IsDigit(_last) && _last != '.' && _last != ',')
            {
                  currentValueInputField.text = currentValueInputField.text.Remove(_index, 1);
            }
        }
    }

    public void RemoteCall_TaskEdited()
    {
        
        if(taskData == null || !allowValueChange) { return; } // For some reason this runs earlier than FeedData, investigate further

        bool _hasDifference = false;
        string _stringToParse = currentValueInputField.text;
        _stringToParse = _stringToParse.Replace(".", ",");
        float _parse = 0;

        switch (taskData.type)
        {
            case AppManager.TaskType.Maximum:
                if (currentValueInputField.text != "")
                {
                    _parse = float.Parse(_stringToParse);
                    if (mxtd.current != _parse) { _hasDifference = true; }
                    else if(!mxtd.isEditedToday) { _hasDifference = true; }
                    mxtd.current = _parse;
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(mxtd);
                }
               
                break;
            case AppManager.TaskType.Minimum:
                if (currentValueInputField.text != "")
                {
                    _parse = float.Parse(_stringToParse);
                    if (mntd.current != _parse) { _hasDifference = true; }
                    else if (!mntd.isEditedToday) { _hasDifference = true; }
                    mntd.current = _parse;
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(mntd);
                }
                
                break;
            case AppManager.TaskType.Boolean:
                _hasDifference = true;
                btd.isDone = booleanTaskTypeToggle.isOn;
                taskData.isEditedToday = true;
                currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(btd);
                break;
            case AppManager.TaskType.Optimum:
                if (currentValueInputField.text != "")
                {
                    _parse = float.Parse(_stringToParse);
                    if (otd.current != _parse) { _hasDifference = true; }
                    else if (!otd.isEditedToday) { _hasDifference = true; }
                    otd.current = _parse;
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(otd);
                }
                
                break;
            case AppManager.TaskType.Interval:
                if (currentValueInputField.text != "")
                {
                    _parse = float.Parse(_stringToParse);
                    if (itd.current != _parse) { _hasDifference = true; }
                    else if (!itd.isEditedToday) { _hasDifference = true; }
                    itd.current = _parse;
                    currentPoint = TaskPointCalculator.GetPointsFromCurrentValue(itd);
                }
                
                break;

            default:
                Debug.LogError("No datatype has been discovered");
                break;

        }

        if (_hasDifference)
        {
            taskData.isEditedToday = true;
            SetScoreText(currentPoint);
            taskData.lastChangedValue = DateTime.Now.Date.ToString();
            ModificationHappened(currentPoint, taskData.name);

            SoundManager.PlaySound1();
        }
    }


    public TaskData GetTaskData()
    {
        return taskData;
    }
}
