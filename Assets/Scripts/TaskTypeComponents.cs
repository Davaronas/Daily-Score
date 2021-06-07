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
}

[System.Serializable]
public class BooleanComponents
{
    public TMP_InputField pointsGained_InputField;
    public Toggle streak_Toggle;
    public TMP_InputField streakStartsAfterDays_InputField;
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
}


public class TaskTypeComponents : MonoBehaviour
{
    public MaximumComponents maxComponents;
    public MinimumComponents minComponents;
    public BooleanComponents boolComponents;
    public OptimumComponents optimumComponents;
}
