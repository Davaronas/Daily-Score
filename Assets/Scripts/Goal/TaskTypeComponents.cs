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


public class TaskTypeComponents : MonoBehaviour
{
    public MaximumComponents maxComponents;
    public MinimumComponents minComponents;
    public BooleanComponents boolComponents;
    public OptimumComponents optimumComponents;

    public List<TMP_InputField> inputFields = new List<TMP_InputField>();
    List<Toggle> toggles = new List<Toggle>();

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
                _taskData = new TaskData("INTERVAL NOT SETUP");
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
            _streakStartsAfterDays);

        return _optimumTaskData;
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

    
}
