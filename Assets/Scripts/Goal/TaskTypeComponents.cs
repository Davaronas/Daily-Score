using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MaximumComponents
{
    public TMP_InputField targetValue_InputField;
    public TMP_Dropdown metric_Dropdown;
    public TMP_InputField pointsPerOneMetric_InputField;
    public Toggle overachieveBonus_Toggle;
    public TMP_InputField overachieveBonusPercent_InputField;
    public Toggle streak_Toggle;
    public TMP_InputField streakStartsAfterDays_InputField;

    public TMP_InputField[] GetInputFields()
    {
        TMP_InputField[] inputFields = new TMP_InputField[3];
        inputFields[0] = pointsPerOneMetric_InputField;
        inputFields[1] = overachieveBonusPercent_InputField;
        inputFields[2] = streakStartsAfterDays_InputField;
        return inputFields;
    }

    public Toggle[] GetToggles()
    {
        Toggle[] toggles = new Toggle[2];
        toggles[0] = overachieveBonus_Toggle;
        toggles[1] = streak_Toggle;

        return toggles;
    }
}

[System.Serializable]
public class MinimumComponents
{
    public TMP_InputField targetValue_InputField;
    public TMP_Dropdown metric_Dropdown;
    public TMP_InputField pointsForStayingUnderLimit_InputField;
    public TMP_InputField pointsLostPerOneMetric_InputField;
    public Toggle stayingUnderLimit_Toggle;
    public TMP_InputField stayingUnderLimitBonusPercent_InputField;
    public Toggle streak_Toggle;
    public TMP_InputField streakStartsAfterDays_InputField;

    public TMP_InputField[] GetInputFields()
    {
        TMP_InputField[] inputFields = new TMP_InputField[4];
        inputFields[0] = pointsForStayingUnderLimit_InputField;
        inputFields[1] = pointsLostPerOneMetric_InputField;
        inputFields[2] = stayingUnderLimitBonusPercent_InputField;
        inputFields[3] = streakStartsAfterDays_InputField;
        return inputFields;
    }

    public Toggle[] GetToggles()
    {
        Toggle[] toggles = new Toggle[2];
        toggles[0] = stayingUnderLimit_Toggle;
        toggles[1] = streak_Toggle;

        return toggles;
    }
}

[System.Serializable]
public class BooleanComponents
{
    public TMP_InputField pointsGained_InputField;
    public Toggle streak_Toggle;
    public TMP_InputField streakStartsAfterDays_InputField;
    public TMP_InputField[] GetInputFields()
    {
        TMP_InputField[] inputFields = new TMP_InputField[2];
        inputFields[0] = pointsGained_InputField;
        inputFields[1] = streakStartsAfterDays_InputField;
        return inputFields;
    }

    public Toggle[] GetToggles()
    {
        Toggle[] toggles = new Toggle[1];
        toggles[0] = streak_Toggle;
      

        return toggles;
    }
}

[System.Serializable]
public class OptimumComponents
{
    public TMP_InputField targetValue_InputField;
    public TMP_Dropdown metric_Dropdown;
    public TMP_InputField pointsForOptimumValue_InputField;
    public TMP_InputField pointsLostPerOneMetricDifference_InputField;
    public Toggle streak_Toggle;
    public TMP_InputField streakStartsAfterDays_InputField;

    public TMP_InputField[] GetInputFields()
    {
        TMP_InputField[] inputFields = new TMP_InputField[3];
        inputFields[0] = pointsForOptimumValue_InputField;
        inputFields[1] = pointsLostPerOneMetricDifference_InputField;
        inputFields[2] = streakStartsAfterDays_InputField;
        return inputFields;
    }

    public Toggle[] GetToggles()
    {
        Toggle[] toggles = new Toggle[1];
        toggles[0] = streak_Toggle;

        return toggles;
    }
}

[System.Serializable]
public class IntervalComponents
{
    public TMP_Dropdown metric_Dropdown;
    public List<IntervalPrefabUtility> intervals;
    public List<IntervalSummaryPrefabUtility> intervalSummaries;
}


public class TaskTypeComponents : MonoBehaviour
{
    public MaximumComponents maxComponents;
    public MinimumComponents minComponents;
    public BooleanComponents boolComponents;
    public OptimumComponents optimumComponents;
    public IntervalComponents intervalComponents;

    private List<TMP_InputField> inputFields = new List<TMP_InputField>();
    private List<Toggle> toggles = new List<Toggle>();

    private IntervalHolder intervalHolder;

    private void Awake()
    {
        AddArrayToList(inputFields, maxComponents.GetInputFields());
        AddArrayToList(inputFields, minComponents.GetInputFields());
        AddArrayToList(inputFields, boolComponents.GetInputFields());
        AddArrayToList(inputFields, optimumComponents.GetInputFields());

        AddArrayToList(toggles, maxComponents.GetToggles());
        AddArrayToList(toggles, minComponents.GetToggles());
        AddArrayToList(toggles, boolComponents.GetToggles());
        AddArrayToList(toggles, optimumComponents.GetToggles());

        intervalHolder = FindObjectOfType<IntervalHolder>();
    }

    private void OnDisable()
    {
        ClearComponents();
    }

    private void ClearComponents()
    {
        maxComponents.targetValue_InputField.text = "";
        maxComponents.metric_Dropdown.value = 0;

        for(int i = 0; i < inputFields.Count;i++)
        {
            inputFields[i].text = "";
        }

        for(int j = 0; j < toggles.Count;j++)
        {
            toggles[j].isOn = false;
        }

        for (int i = 0; i < intervalComponents.intervalSummaries.Count;i++)
        {
            Destroy(intervalComponents.intervalSummaries[i]);
        }

        for (int i = 0; i < intervalComponents.intervals.Count;i++)
        {
            Destroy(intervalComponents.intervals[i]);
        }

        intervalComponents.intervals.Clear();
        intervalComponents.intervalSummaries.Clear();
        intervalHolder.Clear();

    }

    private void AddArrayToList(List<TMP_InputField> _list, TMP_InputField[] _array)
    {
        for (int i = 0; i < _array.Length;i++)
        {
            _list.Add(_array[i]);
        }
    }

    private void AddArrayToList(List<Toggle> _list, Toggle[] _array)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            _list.Add(_array[i]);
        }
    }

    public TaskData GetData(AppManager.TaskType _type)
    {
        TaskData _taskData = null;
        switch(_type)
        {
            case AppManager.TaskType.Maximum:
                _taskData = GetMaxDataFromComponents();
                break;
            case AppManager.TaskType.Minimum:
                _taskData = GetMinDataFromComponents();
                break;
            case AppManager.TaskType.Boolean:
                _taskData = GetBooleanDataFromComponents();
                break;
            case AppManager.TaskType.Optimum:
                _taskData = GetOptimumDataFromComponents();
                break;
            case AppManager.TaskType.Interval:
                _taskData = GetIntervalDataFromComponents();
                break;
            default:
                _taskData = new TaskData("DOES NOT EXIST");
                Debug.LogError($"AppManager.TaskType does not contain : {_taskData}");
                break;
        }

        return _taskData;
    }

    private MaximumTaskData GetMaxDataFromComponents()
    {

        int _bonusPercent;
        if (maxComponents.overachieveBonus_Toggle.isOn)
        {
            if (isValid(maxComponents.overachieveBonusPercent_InputField))
            {
                _bonusPercent = int.Parse(maxComponents.overachieveBonusPercent_InputField.text);
            }
            else
            {
                return null;
            }
        }
        else
        {
            _bonusPercent = 0;
        }

        int _streakStartsAfterDays;
        if(maxComponents.streak_Toggle.isOn)
        {
            if (isValid(maxComponents.streakStartsAfterDays_InputField))
            {
                _streakStartsAfterDays = int.Parse(maxComponents.streakStartsAfterDays_InputField.text);
            }
            else
            {
                return null;
            }
        }
        else
        {
            _streakStartsAfterDays = 0;
        }

        if(!isValid( maxComponents.targetValue_InputField)  || !isValid(maxComponents.pointsPerOneMetric_InputField)) { return null; }


        MaximumTaskData _maximumTaskData = new MaximumTaskData("EMPTY",
            (AppManager.TaskMetricType)maxComponents.metric_Dropdown.value,
            int.Parse(maxComponents.targetValue_InputField.text),
            int.Parse(maxComponents.pointsPerOneMetric_InputField.text),
            _bonusPercent,
            _streakStartsAfterDays);

        return _maximumTaskData;
    }



    private MinimumTaskData GetMinDataFromComponents()
    {
        int _bonusPercent;
        if (minComponents.stayingUnderLimit_Toggle.isOn)
        {
            if (isValid(minComponents.stayingUnderLimitBonusPercent_InputField))
            {
                _bonusPercent = int.Parse(minComponents.stayingUnderLimitBonusPercent_InputField.text);
            }
            else
            {
                return null;
            }
        }
        else
        {
            _bonusPercent = 0;
        }

        int _streakStartsAfterDays;
        if (minComponents.streak_Toggle.isOn)
        {
            if (isValid(minComponents.streakStartsAfterDays_InputField))
            {
                _streakStartsAfterDays = int.Parse(minComponents.streakStartsAfterDays_InputField.text);
            }
            else
            {
                return null;
            }
        }
        else
        {
            _streakStartsAfterDays = 0;
        }

        if(!isValid(minComponents.targetValue_InputField) || !isValid(minComponents.pointsForStayingUnderLimit_InputField) || !isValid(minComponents.pointsLostPerOneMetric_InputField)){ return null; }

        MinimumTaskData _minimumTaskData = new MinimumTaskData("EMPTY",
            (AppManager.TaskMetricType)minComponents.metric_Dropdown.value,
            int.Parse(minComponents.targetValue_InputField.text),
            int.Parse(minComponents.pointsForStayingUnderLimit_InputField.text),
            int.Parse(minComponents.pointsLostPerOneMetric_InputField.text),
            _bonusPercent,
            _streakStartsAfterDays);

        return _minimumTaskData;
    }

    private BooleanTaskData GetBooleanDataFromComponents()
    {
        int _streakStartsAfterDays;
        if(boolComponents.streak_Toggle.isOn)
        {
            if (isValid(boolComponents.streakStartsAfterDays_InputField))
            {
                _streakStartsAfterDays = int.Parse(boolComponents.streakStartsAfterDays_InputField.text);
            }
            else
            {
                return null;
            }
        }
        else
        {
            _streakStartsAfterDays = 0;
        }
        if (!isValid(boolComponents.pointsGained_InputField)) { return null; }

        BooleanTaskData _boolTaskData = new BooleanTaskData("EMPTY", int.Parse(boolComponents.pointsGained_InputField.text), _streakStartsAfterDays);

        return _boolTaskData;
    }

    private OptimumTaskData GetOptimumDataFromComponents()
    {
        int _streakStartsAfterDays;
        if (optimumComponents.streak_Toggle.isOn)
        {
            if (isValid(optimumComponents.streakStartsAfterDays_InputField))
            {
                _streakStartsAfterDays = int.Parse(optimumComponents.streakStartsAfterDays_InputField.text);
            }
            else
            {
                return null;
            }
        }
        else
        {
            _streakStartsAfterDays = 0;
        }

        if(!isValid(optimumComponents.targetValue_InputField) || !isValid(optimumComponents.pointsForOptimumValue_InputField) || !isValid(optimumComponents.pointsLostPerOneMetricDifference_InputField))
        { return null; }

        OptimumTaskData _optimumTaskData = new OptimumTaskData("EMPTY",
            int.Parse(optimumComponents.targetValue_InputField.text),
            int.Parse(optimumComponents.pointsForOptimumValue_InputField.text),
            int.Parse(optimumComponents.pointsLostPerOneMetricDifference_InputField.text),
            (AppManager.TaskMetricType)optimumComponents.metric_Dropdown.value,
            _streakStartsAfterDays);

        return _optimumTaskData;
    }

    private IntervalTaskData GetIntervalDataFromComponents()
    {
        List<Interval> _intervals = new List<Interval>();
        for(int i = 0; i < intervalComponents.intervals.Count; i++)
        {
            if (intervalComponents.intervals[i].HasIntervalSummaryInstance()) // HasIntervalSummary also checks if the inputfield contents are valid
            {
                
                _intervals.Add((Interval)intervalComponents.intervals[i]);
            }
            else
            {
                return null;
            }
        }

        if(!IntervalRangeCheck())
        {
            return null;
        }

        IntervalTaskData _intervalTaskData = new IntervalTaskData("EMPTY",
            _intervals.ToArray(),
            (AppManager.TaskMetricType)intervalComponents.metric_Dropdown.value);

        return _intervalTaskData;
    }

    public AppManager.TaskMetricType GetIntervalMetricType()
    {
        return (AppManager.TaskMetricType)intervalComponents.metric_Dropdown.value;
    }

    public int GetIntervalAmount()
    {
        return intervalComponents.intervals.Count;
    }


    public void AddInterval(IntervalPrefabUtility _ipu)
    {
        intervalComponents.intervals.Add(_ipu);
    }

    public void AddIntervalSummary(IntervalSummaryPrefabUtility _ispu)
    {
        intervalComponents.intervalSummaries.Add(_ispu);
    }

    public void RemoveInterval(IntervalPrefabUtility _ipu)
    {
        int _sn = _ipu.GetSerialNumber();
        if(intervalComponents.intervals.Contains(_ipu))
        {
            intervalComponents.intervals.Remove(_ipu);
        }
        else
        {
            Debug.LogError($"intervals do not contain this interval: {_ipu}");
        }

        for(int i = 0; i < intervalComponents.intervals.Count;i++)
        {
            print(intervalComponents.intervals[i].GetSerialNumber() + " " + _sn);
            if (intervalComponents.intervals[i].GetSerialNumber() > _sn)
            intervalComponents.intervals[i].ReduceIntervalSerialNumberByOne();
        }
    }

    public void RemoveIntervalSummary(IntervalSummaryPrefabUtility _ispu)
    {
        if (intervalComponents.intervalSummaries.Contains(_ispu))
        {
            intervalComponents.intervalSummaries.Remove(_ispu);
        }
        else
        {
            Debug.LogError($"intervalSummaries do not contain this interval summary: {_ispu}");
        }
    }

    public void RemoteCall_IntervalMetricDropdownChanged()
    {
        for(int i = 0; i < intervalComponents.intervalSummaries.Count;i++)
        {
            intervalComponents.intervalSummaries[i].UpdateMetric((AppManager.TaskMetricType)intervalComponents.metric_Dropdown.value);
        }
    }




    private bool isValid(TMP_InputField _text)
    {
        if( int.TryParse(_text.text,out int _number))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private bool IntervalRangeCheck()
    {
        for(int i = 0; i < intervalComponents.intervals.Count - 1; i++)
        {
            int[] _ranges = new int[4];
            intervalComponents.intervals[i].GetRange(out _ranges[0], out _ranges[1]);
            intervalComponents.intervals[i + 1].GetRange(out _ranges[2], out _ranges[3]);



           int _check = Mathf.Max(0, Mathf.Min(_ranges[1], _ranges[3]) - Mathf.Max(_ranges[0], _ranges[2]) + 1);

            print(_check);

            if(_check > 0)
            {
                return false;
            }
        }

        return true;
    }
    
}
