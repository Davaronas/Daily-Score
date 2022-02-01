using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown barChart1_dropdown;
    [SerializeField] private TMP_Dropdown pieChart1_dropdown;
    [SerializeField] private TMP_Dropdown barChartTop3_dropdown;


    private StatisticCalculator2 statCalculator = null;

    [SerializeField] private RewindTimeHandler rth_barChart = null;
    [SerializeField] private RewindTimeHandler rth_pieChart = null;
    [SerializeField] private RewindTimeHandler rth_barChartTop3 = null;

    private ActivityRate activityRateChart = null;
    private TargetValuesReached targetValuesReachedChart = null;


    private void Awake()
    {
        statCalculator = FindObjectOfType<StatisticCalculator2>();
        AppManager.OnTaskValueChanged += UpdateChartsInvoke;
        AppManager.OnLanguageChanged += LanguageCallback;
        AppManager.OnBarChartCategorySelected += BarChartCategorySelected;
        AppManager.OnTaskValueChanged += UpdateChartsInvoke;
        AppManager.OnGoalDeleted += UpdateChartsInvoke;
        AppManager.OnNewGoalAdded += UpdateChartsInvoke;


        activityRateChart = FindObjectOfType<ActivityRate>();
        targetValuesReachedChart = FindObjectOfType<TargetValuesReached>();




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
        AppManager.OnTaskValueChanged -= UpdateChartsInvoke;
        AppManager.OnLanguageChanged -= LanguageCallback;
        AppManager.OnBarChartCategorySelected -= BarChartCategorySelected;
        AppManager.OnTaskValueChanged -= UpdateChartsInvoke;
        AppManager.OnGoalDeleted -= UpdateChartsInvoke;
        AppManager.OnNewGoalAdded -= UpdateChartsInvoke;

    }



    private void LoadCharts()
    {

        barChart1_dropdown.ClearOptions();
        List<string> _options_2Type = new List<string>();
        _options_2Type.Add(RuntimeTranslator.TranslateWeeklyWord());
        _options_2Type.Add(RuntimeTranslator.TranslateMonthlyWord());
        barChart1_dropdown.AddOptions(_options_2Type);

        pieChart1_dropdown.ClearOptions();
        List<string> _options_4Type = new List<string>();
        _options_4Type.Add(RuntimeTranslator.TranslateDailyWord());
        _options_4Type.Add(RuntimeTranslator.TranslateWeeklyWord());
        _options_4Type.Add(RuntimeTranslator.TranslateMonthlyWord());
        _options_4Type.Add(RuntimeTranslator.TranslateAllTimeWord());
        pieChart1_dropdown.AddOptions(_options_4Type);



        barChartTop3_dropdown.ClearOptions();
        List<string> _options_3Type = new List<string>();
        _options_3Type.Add(RuntimeTranslator.TranslateDailyWord());
        _options_3Type.Add(RuntimeTranslator.TranslateWeeklyWord());
        _options_3Type.Add(RuntimeTranslator.TranslateMonthlyWord());
        barChartTop3_dropdown.AddOptions(_options_3Type);



        pieChart1_dropdown.value = 1;

        barChart1_dropdown.value = 1;
        barChart1_dropdown.value = 0;

        barChartTop3_dropdown.value = 1;
    }

    private void UpdateChartsInvoke()
    {
        Invoke(nameof(UpdateCharts), Time.deltaTime*2);
    }

    private void UpdateChartsInvoke(TaskData _data)
    {
        Invoke(nameof(UpdateCharts), Time.deltaTime*2);
    }

    private void UpdateCharts()
    {
        statCalculator.BarChartCalculation(rth_barChart.rewind, barChart1_dropdown.value);
        statCalculator.PieChartCalculation(rth_pieChart.rewind, pieChart1_dropdown.value);
        statCalculator.Top3BarChartCalculation(barChartTop3_dropdown.value, rth_barChartTop3.rewind);
        activityRateChart.CalculateActivityRate();
        targetValuesReachedChart.CalculateTargetValuesReached();
        statCalculator.RollingAverageBarChartCalculation();
    }

    public void RemoteCall_BarChartValueChanged()
    {


        statCalculator.BarChartCalculation(0, barChart1_dropdown.value);

        rth_barChart.ResetButtons();
    }


    public void RemoteCall_PieChartValueChanged()
    {

        if (pieChart1_dropdown.value != 3)
        {

            statCalculator.PieChartCalculation(0, pieChart1_dropdown.value);

            rth_pieChart.ResetButtons();
        }
        else
        {
            statCalculator.PieChartCalculation(0, pieChart1_dropdown.value);
            rth_pieChart.DisableButtons();
        }

    }

    public void RemoteCall_BarChartTop3ValueChanged()
    {
        if (barChartTop3_dropdown.value != 3)
        {
            statCalculator.Top3BarChartCalculation(barChartTop3_dropdown.value, 0);
            rth_barChartTop3.ResetButtons();
        }
        else
        {
            statCalculator.Top3BarChartCalculation(barChartTop3_dropdown.value, 0);
            rth_barChartTop3.DisableButtons();
        }
    }



    public void BarChartCategorySelected(string _n, CategorySelectableBarCharts _c)
    {
        if (_c == CategorySelectableBarCharts.BarChart1)
        {
            statCalculator.BarChartCalculation(rth_barChart.GetCurrentRewind(), barChart1_dropdown.value);
        }
        else if(_c == CategorySelectableBarCharts.RollingAverage)
        {
            statCalculator.RollingAverageBarChartCalculation();
        }
    }

    public void RewindChangedBarChart(int _rewind)
    {
        statCalculator.BarChartCalculation(_rewind, barChart1_dropdown.value);
    }

    public void RewindChangedPieChart(int _rewind)
    {
        statCalculator.PieChartCalculation(_rewind, pieChart1_dropdown.value);
    }

    public void RewindChangedBarChartTop3(int _rewind)
    {
        statCalculator.Top3BarChartCalculation(barChartTop3_dropdown.value, _rewind);
    }

   

    public int BarChartValue()
    {
        return barChart1_dropdown.value;
    }

    public int BarChartTop3Value()
    {
        return barChartTop3_dropdown.value;
    }

    public int PieChartValue()
    {
        return pieChart1_dropdown.value;
    }

}
