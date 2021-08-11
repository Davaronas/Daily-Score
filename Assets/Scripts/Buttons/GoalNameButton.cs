using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoalNameButton : BehaviourButton
{
    [HideInInspector] public string heldName = "";
    private TMP_Text nameText = null;
    private Image selectedBackground = null;

    private StatisticCalculator2 statCalc = null;

    public void Overall()
    {
        AppManager.OnBarChartCategorySelected += CategorySelectedCallback;

        nameText = GetComponent<TMP_Text>();
        selectedBackground = GetComponentInChildren<Image>();
        statCalc = FindObjectOfType<StatisticCalculator2>();

        selectedBackground.enabled = true;

        heldName = "";
        nameText.text = RuntimeTranslator.TranslateOverallWord();
    }


    public void SetName(string _name)
    {
        AppManager.OnBarChartCategorySelected += CategorySelectedCallback;

        nameText = GetComponent<TMP_Text>();
        selectedBackground = GetComponentInChildren<Image>();
        statCalc = FindObjectOfType<StatisticCalculator2>();

        selectedBackground.enabled = false;

        heldName = _name;
        nameText.text = heldName;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AppManager.OnBarChartCategorySelected -= CategorySelectedCallback;
    }

    protected override void OnTouch()
    {
        if (heldName != "")
        {
            statCalc.SetSelectedGoalName(heldName);
        }
        else
        {
            statCalc.EverythingSelected();
        }

        AppManager.BarChartCategorySelected(heldName);
    }

    private void CategorySelectedCallback(string _name)
    {
        if (heldName == _name)
        {
            selectedBackground.enabled = true;
        }
        else
        {
            selectedBackground.enabled = false;
        }
    }
}
