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
        AppManager.OnTaskValueChanged += UpdateIfDailyIsSelected;
        AppManager.OnLanguageChanged += LoadCharts;

       


        Invoke(nameof(LoadCharts), 0.5f);
        
    }

    private void OnDestroy()
    {
        AppManager.OnTaskValueChanged -= UpdateIfDailyIsSelected;
        AppManager.OnLanguageChanged -= LoadCharts;

    }

    private void LoadCharts(AppManager.Languages _l)
    {
        barChart1_dropdown.ClearOptions();
        List<string> _optionsBarChart1 = new List<string>();
        _optionsBarChart1.Add(RuntimeTranslator.TranslateWeeklyWord());
        _optionsBarChart1.Add(RuntimeTranslator.TranslateMonthlyWord());
        barChart1_dropdown.AddOptions(_optionsBarChart1);

        pieChart1_dropdown.ClearOptions();
        List<string> _optionsPieChart1 = new List<string>();
        _optionsPieChart1.Add(RuntimeTranslator.TranslateDailyWord());
        _optionsPieChart1.Add(RuntimeTranslator.TranslateWeeklyWord());
        _optionsPieChart1.Add(RuntimeTranslator.TranslateMonthlyWord());
        _optionsPieChart1.Add(RuntimeTranslator.TranslateAllTimeWord());
        pieChart1_dropdown.AddOptions(_optionsPieChart1);

        pieChart1_dropdown.value = 1;
        barChart1_dropdown.value = 1;
        barChart1_dropdown.value = 0;
    }

    private void UpdateIfDailyIsSelected(TaskData _data)
    {
        Invoke(nameof(UpdateDailyPieChart), Time.deltaTime);
    }

    private void UpdateDailyPieChart()
    {
        if (pieChart1_dropdown.value == 0)
        {
            statCalculator.GoalPieDaily();
        }
    }

    public void RemoteCall_BarChartValueChanged()
    {
       
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
