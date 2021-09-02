using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum CategorySelectableBarCharts {BarChart1, RollingAverage  }
public class GoalNameButton : BehaviourButton
{
    [SerializeField] private CategorySelectableBarCharts chartType;

    [HideInInspector] public string heldName = "";
    private TMP_Text nameText = null;
    [SerializeField] private Image selectedBackground = null;

    private StatisticCalculator2 statCalc = null;

     private SubmenuBroadcaster categorySelectorBroadcaster = null;

    public void Overall(SubmenuBroadcaster _smb, CategorySelectableBarCharts _category)
    {
        categorySelectorBroadcaster = _smb;

        chartType = _category;
        
        AppManager.OnBarChartCategorySelected += CategorySelectedCallback;

        nameText = GetComponent<TMP_Text>();

        statCalc = FindObjectOfType<StatisticCalculator2>();

        selectedBackground.enabled = true;

        heldName = "";
        nameText.text = RuntimeTranslator.TranslateOverallWord();
    }


    public void SetName(string _name, SubmenuBroadcaster _smb, CategorySelectableBarCharts _category)
    {
        categorySelectorBroadcaster = _smb;

        chartType = _category;

        AppManager.OnBarChartCategorySelected += CategorySelectedCallback;

        nameText = GetComponent<TMP_Text>();

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
        if (!Application.isEditor)
        {
            categorySelectorBroadcaster.FeedClickPosition(Input.GetTouch(0).position);
        }
        else
        {
            categorySelectorBroadcaster.FeedClickPosition(Input.mousePosition);
        }


    }

    protected override void OnRelease()
    {
        if (!categorySelectorBroadcaster.isBeingDragged)
        {
            if (chartType == CategorySelectableBarCharts.BarChart1)
            {
                if (heldName != "")
                {
                    statCalc.SetSelectedGoalName_BarChart1(heldName);
                }
                else
                {
                    statCalc.EverythingSelected_BarChart1();
                }
            }
            else if(chartType == CategorySelectableBarCharts.RollingAverage)
            {
                if (heldName != "")
                {
                    statCalc.SetSelectedGoalName_RollingAverage(heldName);
                }
                else
                {
                    statCalc.EverythingSelected_RollingAverage();
                }
            }

            AppManager.BarChartCategorySelected(heldName, chartType);

            SoundManager.PlaySound2();
        }
    }

    private void CategorySelectedCallback(string _name, CategorySelectableBarCharts _chart)
    {
        if (chartType == _chart)
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
}
