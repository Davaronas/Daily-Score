using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown barChart1_dropdown;
    [SerializeField] private TMP_Dropdown pieChart1_dropdown;

    private StatisticCalculator2 statCalculator = null;

    private RewindTimeHandler rth = null;

    private void Awake()
    {
        statCalculator = FindObjectOfType<StatisticCalculator2>();
        AppManager.OnTaskValueChanged += UpdateIfDailyIsSelected;
        AppManager.OnLanguageChanged += LanguageCallback;
        AppManager.OnBarChartCategorySelected += BarChartCategorySelected;
        

        rth = FindObjectOfType<RewindTimeHandler>();


       
        
    }

    private void Start()
    {
        InvokeLoadCharts();
    }

    private void InvokeLoadCharts()
    {
        Invoke(nameof(LoadCharts), 1.5f);
    }

   private void LanguageCallback(AppManager.Languages _l)
    {
        InvokeLoadCharts();
    }

    private void OnDestroy()
    {
        AppManager.OnTaskValueChanged -= UpdateIfDailyIsSelected;
        AppManager.OnLanguageChanged -= LanguageCallback;
        AppManager.OnBarChartCategorySelected -= BarChartCategorySelected;

    }

   

    private void LoadCharts()
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
       // barChart1_dropdown.value = 0;
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
        print("Value changed " + barChart1_dropdown.value);
       
        switch(barChart1_dropdown.value)
        {
            case 0:
                statCalculator.BarChartCalculation(0,0);
                break;
            case 1:
                statCalculator.BarChartCalculation(0,1);
                break;
        }

        rth.ResetButtons();
    }


    public void BarChartCategorySelected(string _n)
    {
        statCalculator.BarChartCalculation(rth.GetCurrentRewind(), barChart1_dropdown.value);
    }

    public void RewindChanged(int _rewind)
    {
        switch (barChart1_dropdown.value)
        {
            case 0:
                statCalculator.BarChartCalculation(_rewind, 0);
                break;
            case 1:
                statCalculator.BarChartCalculation(_rewind, 1);
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

    public int BarChartValue()
    {
        return barChart1_dropdown.value;
    }

}
