using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown barChart1_dropdown;
    [SerializeField] private TMP_Dropdown pieChart1_dropdown;

    private StatisticCalculator2 statCalculator = null;

    private void Awake()
    {
        statCalculator = FindObjectOfType<StatisticCalculator2>();


        barChart1_dropdown.ClearOptions();
        List<string> _optionsBarChart1 = new List<string>();
        _optionsBarChart1.Add(RuntimeTranslator.TranslateWeeklyWord());
        _optionsBarChart1.Add(RuntimeTranslator.TranslateMonthlyWord());
        barChart1_dropdown.AddOptions(_optionsBarChart1);
        barChart1_dropdown.value = 0;

        pieChart1_dropdown.ClearOptions();
        List<string> _optionsPieChart1 = new List<string>();
        _optionsPieChart1.Add(RuntimeTranslator.TranslateDailyWord());
        _optionsPieChart1.Add(RuntimeTranslator.TranslateWeeklyWord());
        _optionsPieChart1.Add(RuntimeTranslator.TranslateMonthlyWord());
        _optionsPieChart1.Add(RuntimeTranslator.TranslateAllTimeWord());
        pieChart1_dropdown.AddOptions(_optionsPieChart1);
        pieChart1_dropdown.value = 0;



    }

    public void RemoteCall_BarChartValueChanged()
    {
        print("Value changed bar " + barChart1_dropdown.value);
        switch(barChart1_dropdown.value)
        {
            case 0:
                statCalculator.Weeklygraph();
                break;
            case 1:
                statCalculator.MonthlyGraph();
                break;
        }
    }

    public void RemoteCall_PieChartValueChanged()
    {
        print("Value changed pie "+ pieChart1_dropdown.value);
        switch (pieChart1_dropdown.value)
        {
            case 0:
                statCalculator.GoalPieDaily();
                break;
            case 1:
                statCalculator.GoalPieWeek();
                break;
            case 2:
                statCalculator.GoalPieMonth();
                break;
            case 3:
                statCalculator.GoalPieMind();
                break;
        }
    }

}
