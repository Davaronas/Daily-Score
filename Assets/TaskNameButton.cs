using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskNameButton : BehaviourButton
{ 
    [HideInInspector] public string heldName = "";
    private TMP_Text nameText = null;
    private Image selectedBackground = null;

    private StatisticCalculator2 statCalc = null;

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
        statCalc.SetSelectedTaskName(heldName);
        AppManager.BarChartCategorySelected(heldName);
    }

    private void CategorySelectedCallback(string _name)
    {
        if(heldName == _name)
        {
            selectedBackground.enabled = true;
        }
        else
        {
            selectedBackground.enabled = false;
        }
    }
}