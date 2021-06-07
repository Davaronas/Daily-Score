using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MaximumComponents
{
    public TMP_InputField targetValueInputField;
    public TMP_Dropdown metricDropdown;
    public Toggle overachieveBonusToggle;
    public TMP_InputField overachieveBonusPercentInputField;
    public Toggle streakToggle;
    public TMP_InputField streakStartsAfterDaysInputField;



}


public class TaskTypeComponents : MonoBehaviour
{
    public MaximumComponents maxComponents;
}
