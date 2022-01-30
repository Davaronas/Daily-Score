using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// StatCalc version 2.8;
/// </summary>
struct DailyScoreStruct
{
    public DateTime DailyDate;
    public int dailyScore;
    public DailyScoreStruct(DateTime dailyDate, int dailyscore)
    {
        DailyDate = dailyDate;
        dailyScore = dailyscore;
    }
    //Test
}

struct Top3BarChartData
{
    public int amount;
    public int pictoId;
    public Color32 goalColor;

    public Top3BarChartData(int _a, int _id, Color32 _c)
    {
        amount = _a;
        pictoId = _id;
        goalColor = _c;
    }

    public override bool Equals(object obj)
    {
        return obj is Top3BarChartData data &&
               amount == data.amount &&
               pictoId == data.pictoId &&
               EqualityComparer<Color32>.Default.Equals(goalColor, data.goalColor);
    }


}

public enum StatCalculationFilter {All, Goal, Task}

public class StatisticCalculator2 : MonoBehaviour
{
    public GoalManager goalManager = null;
    public DateTime Today;
    public int dailysc = 0;
    public int lastlogdur;
    public float weeklyavarage;
    public float monthlyavarage;
    public float weeklytaskpointav;
    public int dailytaskpoint;
    public int maxofmax;
    public int maxoftheday;
    public int maxoftheweek;
    public int maxofthemonth;
    public float alltimevarage;
    public int buttoncounter = 0;


    List<DailyScoreStruct> DailyScoreStructsList = new List<DailyScoreStruct>();
    [SerializeField] TMP_Text dailyScoreText;
    [SerializeField] TMP_Text dailyScoreTextStatMenu;
    [SerializeField] TMP_Text taskdailyScoreText;
    [SerializeField] TMP_Text weeklyScoreText;
    [SerializeField] TMP_Text weeklyScoreTextStatMenu;
    [SerializeField] TMP_Text taskweeklyScoreText;
    [SerializeField] TMP_Text monthlyScoreText;
    [SerializeField] TMP_Text maxScoreText;
    [SerializeField] TMP_Text taskmaxScoreText;
    [SerializeField] TMP_Text HighScoreThisWeekStatMenu;
    [SerializeField] TMP_Text HighCoreThisMonthScoreStatMenu;
    [SerializeField] TMP_Text AllTimeBestScoreTextStatMenu;
    [SerializeField] TMP_Text AllTimeAvarageScoreTextStatMenu;

    public GoalData[] GoalDATAS;

    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private PieChart piechart1;

    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private BarChartHolder barchart1;
    [SerializeField] private BarChartHolder barChartTop3;
    [SerializeField] private BarChartHolder barChartRollingAverage;

    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 


    private string selectedGoalName_BarChart1 = "";
    private string selectedTaskName_BarChart1 = "";

    private string selectedGoalName_RollingAverage = "";
    private string selectedTaskName_RollingAverage = "";

    private void Awake()
    {
        AppManager.OnTaskValueChanged += OnTaskValueChanged;
        AppManager.OnNewDayStartedDuringRuntime += OnNewDayStartedDuringRuntime;

        AppManager.OnGoalOpened += OnGoalOpened;
        AppManager.OnGoalDeleted += OnGoalDeleted;


        Invoke(nameof(GoalScoreCalcs), Time.deltaTime);

    }


    public void SetSelectedGoalName_BarChart1(string _g)
    {
        selectedGoalName_BarChart1 = _g;
        selectedTaskName_BarChart1 = "";
    }


    public void SetSelectedTaskName_BarChart1(string _t)
    {
        selectedTaskName_BarChart1 = _t;
        selectedGoalName_BarChart1 = "";
    }

    public void EverythingSelected_BarChart1()
    {
        selectedGoalName_BarChart1 = "";
        selectedTaskName_BarChart1 = "";
    }



    public void SetSelectedGoalName_RollingAverage(string _g)
    {
        selectedGoalName_RollingAverage = _g;
        selectedTaskName_RollingAverage = "";
    }


    public void SetSelectedTaskName_RollingAverage(string _t)
    {
        selectedTaskName_RollingAverage = _t;
        selectedGoalName_RollingAverage = "";
    }

    public void EverythingSelected_RollingAverage()
    {
        selectedGoalName_RollingAverage = "";
        selectedTaskName_RollingAverage = "";
    }



    private void OnGoalDeleted()
    {
        Invoke(nameof(GoalScoreCalcs), Time.deltaTime * 3);
        Invoke(nameof(TaskGoalCalc), Time.deltaTime * 3);
    }

    private void OnGoalOpened(Goal _td)
    {
        Invoke(nameof(GoalScoreCalcs), Time.deltaTime * 3);
        Invoke(nameof(TaskGoalCalc), Time.deltaTime * 3);
    }
    private void OnTaskValueChanged(TaskData _td)
    {
        // Amikor a felhasználó változtat értéket ez hívódik. A _td pedig hogy melyik feladat változott.
        // _td.owner pedig megadja melyik GoalData változott név szerint (ez egy string változó)
        // hogy ez melyik goaldata úgy tudod megkapni hogy goalManager.SearchGoalByName(_td.owner);
        // vagy
        // GoalData _gd; goalManager.SearchGoalByName(_td.owner, out _gd); ezen van bool check is hogy sikerült e megtalálni/létezik e

        // Csak futás közben érzékeli a változtatásokat, a program inditáskor ez nem fut le

        Invoke(nameof(GoalScoreCalcs), Time.deltaTime * 2);
        Invoke(nameof(TaskGoalCalc), Time.deltaTime * 2);


    }

    private void TaskGoalCalc()
    {
        TaskDailyCalc();
        TaskWeeklyCalc();
        TaskMaskCalc();
    }
    private void GoalScoreCalcs()
    {
        StatLoad();
        DailyScoreCalc();
        WeeklyScoreCal();
        MonthlyAvarageCalc();
        MAXCalc();
    }

    void StatLoad()
    {

        GoalDATAS = goalManager.GetExistingGoals();
        //   print(GoalDATAS == null);
    }

    void DailyScoreCalc()
    {
        int i = 0;
        dailysc = 0;
        for (i = 0; i < GoalDATAS.Length; i++)
        {
            if (GoalDATAS[i].GetLastModificationTime().Day == Today.Day)
            {
                //dailysc += GoalDATAS[i].lastChange.amount;
                dailysc = dailysc + GoalDATAS[i].current;
            }
        }
        dailyScoreText.text = dailysc.ToString();
        dailyScoreTextStatMenu.text = dailysc.ToString();
    }

    void WeeklyScoreCal()
    {
        int[] weeklydata = new int[7];
        weeklyavarage = 0;
        //  print(dailysc);
        float weeklyfleet = dailysc;
        int daycounter = 1;
        int i = 0;
        for (int l = 0; l < 7; l++)
        {
            weeklydata[l] = 0;
        }
        for (i = 0; i < GoalDATAS.Length; i++)
        {
            for (int k = 0; k < GoalDATAS[i].dailyScores.Count; k++)
            {
                if (GoalDATAS[i].dailyScores[k].isRestDay) { continue; }

                if (Convert.ToDateTime(GoalDATAS[i].dailyScores[k].time).Date >= Today.AddDays(-7))
                {
                    daycounter++;
                    switch (Convert.ToDateTime(GoalDATAS[i].dailyScores[k].time).DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            weeklydata[0] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week0");
                            break;
                        case DayOfWeek.Tuesday:
                            weeklydata[1] += GoalDATAS[i].dailyScores[k].amount;
                            // print("Week1");
                            break;
                        case DayOfWeek.Wednesday:
                            weeklydata[2] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week2");
                            break;
                        case DayOfWeek.Thursday:
                            weeklydata[3] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week3");
                            break;
                        case DayOfWeek.Friday:
                            weeklydata[4] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week4");
                            break;
                        case DayOfWeek.Saturday:
                            weeklydata[5] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week5");
                            break;
                        case DayOfWeek.Sunday:
                            weeklydata[6] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week6");
                            break;
                    }
                }
            }
        }
        for (int j = 0; j < 7; j++)
        {
            weeklyfleet += weeklydata[j];
            // print(weeklydata[j]);
            // print(weeklyfleet);
        }

        // if (daycounter == 0) { return; }

        weeklyavarage = (float)weeklyfleet / daycounter;
        weeklyScoreText.text = Math.Round(weeklyavarage, 2).ToString();
        weeklyScoreTextStatMenu.text = Math.Round(weeklyavarage, 2).ToString();
    }
    void MonthlyAvarageCalc()
    {
        monthlyavarage = 0;
        int monthlyfleet = dailysc;
        int daycountermonth = 1;
        int i = 0;
        for (int j = 0; j < GoalDATAS.Length; j++)
        {
            if (GoalDATAS[j].dailyScores == null)
            {
                continue;
            }
            for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
            {
                if (GoalDATAS[j].dailyScores[k].isRestDay) { continue; }

                if (Convert.ToDateTime(GoalDATAS[j].dailyScores[k].time) >= Today.AddDays(-30))
                {

                    daycountermonth++;
                    monthlyfleet += GoalDATAS[j].dailyScores[k].amount;
                }
            }
        }

        // if (daycountermonth == 0) { return; }

        monthlyavarage = (float)monthlyfleet / daycountermonth;
        monthlyScoreText.text = Math.Round(weeklyavarage, 2).ToString();
    }

    void TaskDailyCalc()
    {
        if (goalManager.GetCurrentlySelectedGoal() == null)
        {
            return;
        }

        GoalData currentselectedGoal = goalManager.GetCurrentlySelectedGoal().GetGoalData();
        dailytaskpoint = 0;
        int i = 0;
        for (int j = 0; j < currentselectedGoal.tasks.Count; j++)
        {
            if (currentselectedGoal.tasks[j].isEditedToday)
            {
                dailytaskpoint += TaskPointCalculator.GetPointsFromCurrentValue(currentselectedGoal.tasks[j]);
            }

        }

        taskdailyScoreText.text = dailytaskpoint.ToString();

    }

    void TaskWeeklyCalc()
    {
        if (goalManager.GetCurrentlySelectedGoal() == null)
        {
            return;
        }

        GoalData currentselectedGoal = goalManager.GetCurrentlySelectedGoal().GetGoalData();
        float weeklytaskpointav = 0;
        int kcounter = 0;
        float weeklytaskfleet = dailytaskpoint;
        int[] weeklydata = new int[7];
        for (int k = 0; k < currentselectedGoal.dailyScores.Count; k++)
        {
            if (currentselectedGoal.dailyScores[k].isRestDay) { continue; }

            if (Convert.ToDateTime(currentselectedGoal.dailyScores[k].time).Date >= Today.AddDays(-7))
            {
                kcounter++;

                switch (Convert.ToDateTime(currentselectedGoal.dailyScores[k].time).DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        weeklydata[0] += currentselectedGoal.dailyScores[k].amount;
                     //   print("Week0");
                        break;
                    case DayOfWeek.Tuesday:
                        weeklydata[1] += currentselectedGoal.dailyScores[k].amount;
                     //   print("Week1");
                        break;
                    case DayOfWeek.Wednesday:
                        weeklydata[2] += currentselectedGoal.dailyScores[k].amount;
                    //    print("Week2");
                        break;
                    case DayOfWeek.Thursday:
                        weeklydata[3] += currentselectedGoal.dailyScores[k].amount;
                     //   print("Week3");
                        break;
                    case DayOfWeek.Friday:
                        weeklydata[4] += currentselectedGoal.dailyScores[k].amount;
                      //  print("Week4");
                        break;
                    case DayOfWeek.Saturday:
                        weeklydata[5] += currentselectedGoal.dailyScores[k].amount;
                      //  print("Week5");
                        break;
                    case DayOfWeek.Sunday:
                        weeklydata[6] += currentselectedGoal.dailyScores[k].amount;
                      //  print("Week6");
                        break;
                }
            }
        }
        for (int j = 0; j < 7; j++)
        {
            weeklytaskfleet += weeklydata[j];
        }

        /*
        for (int j = 0; j < currentselectedGoal.tasks.Count; j++)
        {
            if (currentselectedGoal.tasks[j].isEditedToday)
            {
                print("calculate " + currentselectedGoal.tasks[j].name + " " + weeklytaskfleet);
                weeklytaskfleet += TaskPointCalculator.GetPointsFromCurrentValue(currentselectedGoal.tasks[j]);
            }
        }
        */

        if (kcounter >= 0)
        {
            weeklytaskpointav = weeklytaskfleet / (kcounter + 1);
        }

        taskweeklyScoreText.text = Math.Round(weeklytaskpointav, 2).ToString();
    }
    public void MAXCalc()
    {
        Dictionary<DateTime, int> _unkownDates = new Dictionary<DateTime, int>();
        int[] _month = new int[30];
        int[] _week = new int[7];

        int _allTimeMax = dailysc;
        int _monthMax = dailysc;
        int _weekMax = dailysc;

        float _allTimeAverage = 0;
        float _monthAverage = 0;
        float _weekAverage = 0;

        int _allTimeFound = 1;
        int _monthTimeFound = 1;
        int _weekTimeFound = 1;

        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
            {
                if (GoalDATAS[i].dailyScores[j].isRestDay) { continue; }

                _allTimeAverage += GoalDATAS[i].dailyScores[j].amount;
                _allTimeFound++;

                if (!_unkownDates.ContainsKey(GoalDATAS[i].dailyScores[j].GetDateTime()))
                {
                    _unkownDates.Add(GoalDATAS[i].dailyScores[j].GetDateTime(), GoalDATAS[i].dailyScores[j].amount);
                }
                else
                {
                    _unkownDates[GoalDATAS[i].dailyScores[j].GetDateTime()] += GoalDATAS[i].dailyScores[j].amount;
                }

                for (int k = 0; k < 30; k++)
                {
                    if (GoalDATAS[i].dailyScores[j].GetDateTime() == Today.AddDays(-k))
                    {
                        _month[k] += GoalDATAS[i].dailyScores[j].amount;
                        _monthAverage += GoalDATAS[i].dailyScores[j].amount;
                        _monthTimeFound++;
                        break;
                    }
                }

                for (int l = 0; l < 7; l++)
                {
                    if (GoalDATAS[i].dailyScores[j].GetDateTime() == Today.AddDays(-l))
                    {
                        _week[l] += GoalDATAS[i].dailyScores[j].amount;
                        _weekAverage += GoalDATAS[i].dailyScores[j].amount;
                        _weekTimeFound++;
                        break;
                    }
                }

            }
        }

        foreach (KeyValuePair<DateTime, int> _d in _unkownDates)
        {
            if (_d.Value > _allTimeMax)
            {
                _allTimeMax = _d.Value;
            }
        }

        for (int i = 0; i < _month.Length; i++)
        {
            if (_month[i] > _monthMax)
            {
                _monthMax = _month[i];
            }
        }


        for (int i = 0; i < _week.Length; i++)
        {
            if (_week[i] > _weekMax)
            {
                _weekMax = _week[i];
            }
        }

        _monthAverage += dailysc;
        _allTimeAverage += dailysc;
        _weekAverage += dailysc;

        if (_allTimeFound > 0)
        {
            _allTimeAverage /= _allTimeFound;
        }

        if (_monthTimeFound > 0)
        {
            _monthAverage /= _monthTimeFound;
        }

        if (_weekTimeFound > 0)
        {
            _weekAverage /= _weekTimeFound;
        }



        maxScoreText.text = _allTimeMax.ToString();
        HighScoreThisWeekStatMenu.text = _weekMax.ToString();
        HighCoreThisMonthScoreStatMenu.text = _monthMax.ToString();
        AllTimeBestScoreTextStatMenu.text = _allTimeMax.ToString();
        AllTimeAvarageScoreTextStatMenu.text = Math.Round(_allTimeAverage, 2).ToString();

        // dictionary <int, DateTime> all
        // month[30]
        // week[7]

        // all

        // 30 days

        // 7 days


        // collect datetimes and add integers to it if bigger than 30 days, compare with month max

        // if i (-30 day) == dailyscore[k]
        // i += ds amount

        //if j (-7 day) == ds[k]
        // j += ds amount

        /*
        int maxmonthfleet = 0;
        int maxweekfleet = 0;
        int maxmaxfleet = 0;
        int alltimeavaragefleet = 0;
        int kcounter = 0;

        for (int j = 0; j < GoalDATAS.Length; j++)
        {
            if (GoalDATAS[j].dailyScores == null)
            {
                continue;
            }
            for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
            {
                kcounter++;
                alltimeavaragefleet += GoalDATAS[j].dailyScores[k].amount;
                if (Convert.ToDateTime(GoalDATAS[j].dailyScores[k].time) >= Today.AddDays(-30))
                {

                    if (Convert.ToDateTime(GoalDATAS[j].dailyScores[k].time) >= Today.AddDays(-7))
                    {

                        if (maxweekfleet < GoalDATAS[j].dailyScores[k].amount)
                        {
                            maxweekfleet = GoalDATAS[j].dailyScores[k].amount;
                        }
                    }
                    if (maxmonthfleet < GoalDATAS[j].dailyScores[k].amount)
                    {
                        maxmonthfleet = GoalDATAS[j].dailyScores[k].amount;
                    }
                }
                if (maxmaxfleet < GoalDATAS[j].dailyScores[k].amount)
                {
                    maxmaxfleet = GoalDATAS[j].dailyScores[k].amount;
                }
            }
        }
        maxofmax = maxmaxfleet;
        maxofthemonth = maxmonthfleet;
        maxoftheweek = maxweekfleet;

        if (kcounter != 0)
        {
            alltimevarage = alltimeavaragefleet / kcounter;
        }

        maxScoreText.text = maxofmax.ToString();
        HighScoreThisWeekStatMenu.text = maxoftheweek.ToString();
        HighCoreThisMonthScoreStatMenu.text = maxofthemonth.ToString();
        AllTimeBestScoreTextStatMenu.text = maxofmax.ToString();
        AllTimeAvarageScoreTextStatMenu.text = Math.Round(alltimevarage, 2).ToString();
        */

    }
    public void TaskMaskCalc()
    {
        if (goalManager.GetCurrentlySelectedGoal() == null)
        {
            return;
        }

        GoalData currentselectedGoal = goalManager.GetCurrentlySelectedGoal().GetGoalData();
        int taskmaxfleet = currentselectedGoal.current;
        for (int j = 0; j < currentselectedGoal.modifications.Count; j++)
        {
            if (taskmaxfleet < currentselectedGoal.modifications[j].amount)
            {
                taskmaxfleet = currentselectedGoal.modifications[j].amount;
            }
        }

        taskmaxScoreText.text = taskmaxfleet.ToString();
    }

    public void Weeklygraph()
    {
        List<BarChartInfoText> barChartInfos = new List<BarChartInfoText>();
        StatLoad();
        int i;
        int[] weeklydata = new int[7];
        int counter = 0;
        for (i = 0; i < GoalDATAS.Length; i++)
        {
            // int _sum = 0;
            for (int k = 0; k < GoalDATAS[i].dailyScores.Count; k++)
            {
                if (GoalDATAS[i].dailyScores[k].isRestDay) { continue; }

                if (Convert.ToDateTime(GoalDATAS[i].dailyScores[k].time).Date >= Today.AddDays(-7) && Convert.ToDateTime(GoalDATAS[i].dailyScores[k].time).Date < Today)
                {
                    //_sum += GoalDATAS[i].dailyScores[k].amount;

                    counter++;

                    switch (Convert.ToDateTime(GoalDATAS[i].dailyScores[k].time).DayOfWeek)
                    {

                        case DayOfWeek.Monday:
                            weeklydata[0] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week0");
                            //   barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 2)));
                            break;
                        case DayOfWeek.Tuesday:
                            weeklydata[1] += GoalDATAS[i].dailyScores[k].amount;
                            // print("Week1");
                            //  barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 2)));
                            break;
                        case DayOfWeek.Wednesday:
                            weeklydata[2] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week2");
                            // barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 2)));
                            break;
                        case DayOfWeek.Thursday:
                            weeklydata[3] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week3");
                            // barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 2)));
                            break;
                        case DayOfWeek.Friday:
                            weeklydata[4] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week4");
                            // barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 2)));
                            break;
                        case DayOfWeek.Saturday:
                            weeklydata[5] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week5");
                            // barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 2)));
                            break;
                        case DayOfWeek.Sunday:
                            weeklydata[6] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week6");
                            // barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 2)));
                            break;
                    }

                }
            }
            // weeklydata[i] = _sum;
        }

        foreach (int _w in weeklydata)
        {
            // print(_w);
        }

        barchart1.Clear();
        if (counter >= 1)
        {
            #region LoadDays 


            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    barChartInfos.Add(new BarChartInfoText(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    break;
                case DayOfWeek.Tuesday:
                    barChartInfos.Add(new BarChartInfoText(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    break;
                case DayOfWeek.Wednesday:
                    barChartInfos.Add(new BarChartInfoText(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    break;
                case DayOfWeek.Thursday:
                    barChartInfos.Add(new BarChartInfoText(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    break;
                case DayOfWeek.Friday:
                    barChartInfos.Add(new BarChartInfoText(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    break;
                case DayOfWeek.Saturday:
                    barChartInfos.Add(new BarChartInfoText(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    break;
                case DayOfWeek.Sunday:
                    barChartInfos.Add(new BarChartInfoText(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfoText(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    break;
            }

            #endregion


            /*
            barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
            barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
            barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
            barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
            barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
            barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
            barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
            */

            //  barchart1.LoadData(barChartInfos.ToArray(), true);
        }

    }

    public void MonthlyGraph(int buttoncounter)
    {

        DateTime[] _startAndFinishTime = new DateTime[2];
        List<BarChartInfoText> barChartInfosmonth = new List<BarChartInfoText>();



        StatLoad();
        int rewindtimes;
        if (buttoncounter == 0)
        {
            rewindtimes = 1;
        }
        else
        {
            rewindtimes = Math.Abs(buttoncounter) + 1;
        }




        int[] monthlyData = new int[30];
        for (int i = 1; i <= 30; i++)
        {
            for (int j = 0; j < GoalDATAS.Length; j++)
            {
                for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
                {


                    if (Convert.ToDateTime(GoalDATAS[j].dailyScores[k].time).Date >= Today.AddDays(rewindtimes + 1 * -30) && (Convert.ToDateTime(GoalDATAS[j].dailyScores[k].time).Date >= Today.AddDays(rewindtimes * -30)))
                    {
                        if (i == 1)
                        {
                            _startAndFinishTime[0] = GoalDATAS[j].dailyScores[k].GetDateTime();
                        }

                        if (i == 29)
                        {
                            _startAndFinishTime[1] = GoalDATAS[j].dailyScores[k].GetDateTime();
                        }

                        monthlyData[i - 1] += GoalDATAS[j].dailyScores[k].amount;
                    }
                }
            }
        }




        barchart1.Clear();
        for (int l = monthlyData.Length - 1; l >= 0; l--)
        {
            if (l == monthlyData.Length - 1)
            {
                barChartInfosmonth.Add(new BarChartInfoText(monthlyData[l], _startAndFinishTime[1].Month + "/" + _startAndFinishTime[1].Day));
            }
            else if (l == 0)
            {
                barChartInfosmonth.Add(new BarChartInfoText(monthlyData[l], _startAndFinishTime[0].Month + "/" + _startAndFinishTime[0].Day));
            }
            else
                barChartInfosmonth.Add(new BarChartInfoText(monthlyData[l], ""));
        }
        //  barchart1.LoadData(barChartInfosmonth.ToArray(), false);



    }

    public void GoalPieDaily()
    {
        StatLoad();
        List<PieChartInfo> PiteNap = new List<PieChartInfo>();
        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            PiteNap.Add(new PieChartInfo(GoalDATAS[i].current, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));
        }
        piechart1.Clear();
        if (PiteNap.Count > 0)
        {
            //  piechart1.LoadData(PiteNap.ToArray());
        }
    }

    public void GoalPieWeek()
    {
        StatLoad();
        List<PieChartInfo> PiteHete = new List<PieChartInfo>();
        for (int i = 0; i < GoalDATAS.Length; i++)
        {


            float sziauram = 0;
            for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
            {
                if (Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date >= Today.AddDays(-7))
                {
                    sziauram += GoalDATAS[i].dailyScores[j].amount;
                }
            }
            PiteHete.Add(new PieChartInfo(sziauram, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));

        }
        piechart1.Clear();
        if (PiteHete.Count > 0)
        {
            //  piechart1.LoadData(PiteHete.ToArray());
        }
    }

    public void GoalPieMonth()
    {
        StatLoad();
        List<PieChartInfo> PiteHava = new List<PieChartInfo>();
        for (int i = 0; i < GoalDATAS.Length; i++)
        {


            float sziauram = 0;
            for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
            {
                if (Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date >= Today.AddDays(-30))
                {
                    sziauram += GoalDATAS[i].dailyScores[j].amount;
                }
            }
            PiteHava.Add(new PieChartInfo(sziauram, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));

        }
        piechart1.Clear();
        if (PiteHava.Count > 0)
        {
            // piechart1.LoadData(PiteHava.ToArray());
        }
    }

    public void GoalPieMind()
    {
        StatLoad();
        List<PieChartInfo> PiteMind = new List<PieChartInfo>();
        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            float sziauram = 0;
            for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
            {
                sziauram += GoalDATAS[i].dailyScores[j].amount;
            }
            PiteMind.Add(new PieChartInfo(sziauram, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));

        }
        piechart1.Clear();
        if (PiteMind.Count > 0)
        {
            // piechart1.LoadData(PiteMind.ToArray());
        }
    }

    void RewindDaily(int buttoncounter)
    {
        StatLoad();
        int rewindtimes = buttoncounter;
        int buttonchecker = 0;

        List<PieChartInfo> RewindDay = new List<PieChartInfo>();
        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
            {
                if (Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date == Today.AddDays(rewindtimes)) //Itt lehet a kutya
                {
                    RewindDay.Add(new PieChartInfo(GoalDATAS[i].dailyScores[j].amount, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));
                    print(Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date);
                }
            }


        }
        piechart1.Clear();
        if (RewindDay.Count > 0)
        {
            //  piechart1.LoadData(RewindDay.ToArray());
        }

    }
    public void RewindWeek(int buttoncounter)
    {
        StatLoad();
        List<PieChartInfo> RewindWeek = new List<PieChartInfo>();
        int rewindtimes;
        int buttonchecker = 0;
        if (buttoncounter == 0)
        {
            rewindtimes = 1;
        }
        else
        {
            rewindtimes = Math.Abs(buttoncounter) + 1;
        }
        for (int i = 0; i < GoalDATAS.Length; i++)
        {


            float sziauram = 0;
            for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
            {
                if (Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date >= Today.AddDays(rewindtimes + 1 * -7) && (Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date >= Today.AddDays(rewindtimes * -7))) //Itt lehet a kutya
                {
                    sziauram += GoalDATAS[i].dailyScores[j].amount;
                    // print(Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date);
                }
            }
            RewindWeek.Add(new PieChartInfo(sziauram, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));

        }
        piechart1.Clear();
        if (RewindWeek.Count > 0)
        {
            // piechart1.LoadData(RewindWeek.ToArray());
        }
    }

    public void RewindMonth(int buttoncounter)
    {
        StatLoad();
        List<PieChartInfo> RewindMonth = new List<PieChartInfo>();
        int rewindtimes;
        if (buttoncounter == 0)
        {
            rewindtimes = 1;
        }
        else
        {
            rewindtimes = Math.Abs(buttoncounter) + 1;
        }
        for (int i = 0; i < GoalDATAS.Length; i++)
        {


            float sziauram = 0;
            for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
            {
                if (Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date >= Today.AddDays(rewindtimes + 1 * -30) && (Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date >= Today.AddDays(rewindtimes * -30))) //Itt lehet a kutya
                {
                    sziauram += GoalDATAS[i].dailyScores[j].amount;
                    print(Convert.ToDateTime(GoalDATAS[i].dailyScores[j].time).Date);
                }
            }
            RewindMonth.Add(new PieChartInfo(sziauram, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));

        }
        piechart1.Clear();
        if (RewindMonth.Count > 0)
        {
            // piechart1.LoadData(RewindMonth.ToArray());
        }
    }
    void Start()
    {

        Today = DateTime.Today;
        DateTime LastWeek = Today.AddDays(-7);
        DateTime Test;
        DateTime Lastmodificationfleet = DateTime.Now.AddYears(-10); //Ezzel keresem a maxot
        int LastmodificationDur = 0; //Ez lesz a legutóbbi módosítás dátuma amirõl számolok
        int lastmodmaxI = 0;
        Console.WriteLine(Today.ToString("{0}")); //Debug
        StartCoroutine(TimeCheck());
        //       print(Today.Day);
        int max = 0;//A MAX ami mindenkori lesz.
        lastlogdur = Today.Day - AppManager.lastLogin.Day;
        StatLoad();
        Invoke(nameof(GoalScoreCalcs), 0.2f);
        RewindWeek(0);
    }

    public bool CanRewindBarChartOverall(int _type, int _rewind)
    {

        StatLoad();

        int _time = 0;
        switch (_type)
        {
            case 0:
                _time = 7;
                break;
            case 1:
                _time = 30;
                break;
            default:

                return false;
        }

        DateTime _smallest = Today;



        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            if (GoalDATAS[i].dailyScores.Count > 0)
            {

                if (GoalDATAS[i].dailyScores[0].GetDateTime().Date < _smallest)
                {
                    _smallest = GoalDATAS[i].dailyScores[0].GetDateTime().Date;
                }

            }
        }


        if (_smallest < Today.AddDays(_rewind * _time))
        {
            return true;
        }
        else
            return false;


    }


    public bool CanRewindBarChartTop3(int _type, int _rewind)
    {

        StatLoad();

        int _time = 0;
        switch (_type)
        {
            case 0:
                _time = 1;
                break;
            case 1:
                _time = 7;
                break;
            case 2:
                _time = 30;
                break;
            case 3:
                return false;

        }

        DateTime _smallest = Today;



        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            if (GoalDATAS[i].dailyScores.Count > 0)
            {

                if (GoalDATAS[i].dailyScores[0].GetDateTime().Date < _smallest)
                {
                    _smallest = GoalDATAS[i].dailyScores[0].GetDateTime().Date;
                }

            }
        }


        if (_smallest < Today.AddDays(_rewind * _time))
        {
            return true;
        }
        else
            return false;


    }

    public bool CanRewindPieChart(int _type, int _rewind)
    {

        int _time = 0;


        DateTime _smallest = Today;

        switch (_type)
        {
            case 0:
                _time = 1;
                break;
            case 1:
                _time = 7;
                break;
            case 2:
                _time = 30;
                break;
            case 3:
                return false;

        }

        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            if (GoalDATAS[i].dailyScores.Count > 0)
            {

                if (GoalDATAS[i].dailyScores[0].GetDateTime().Date < _smallest)
                {
                    _smallest = GoalDATAS[i].dailyScores[0].GetDateTime().Date;
                }

            }
        }


        if (_smallest < Today.AddDays(_rewind * _time))
        {
            return true;
        }
        else
            return false;
    }

    public DateTime GetEarliestDailyScore()
    {
        DateTime _smallest = Today;

        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            if (GoalDATAS[i].dailyScores.Count > 0)
            {

                if (GoalDATAS[i].dailyScores[0].GetDateTime().Date < _smallest)
                {
                    _smallest = GoalDATAS[i].dailyScores[0].GetDateTime().Date;
                }

            }
        }
        return _smallest;
    }



    private void OnNewDayStartedDuringRuntime()
    {

    }


    IEnumerator TimeCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

        }
        //yield return new WaitForSeconds(1); //Lehet kell
    }
    private void OnDestroy()
    {
        StopCoroutine(TimeCheck());
        AppManager.OnTaskValueChanged -= OnTaskValueChanged;
        AppManager.OnNewDayStartedDuringRuntime -= OnNewDayStartedDuringRuntime;
        AppManager.OnGoalOpened -= OnGoalOpened;
        AppManager.OnGoalDeleted -= OnGoalDeleted;
    }



    // ------------------------------------


    public void BarChartCalculation(int _rewind, int _w_Or_m)
    {
        StatCalculationFilter _f = StatCalculationFilter.All;
        if (selectedGoalName_BarChart1 == "" && selectedTaskName_BarChart1 == "")
        {
            _f = StatCalculationFilter.All;
        }
        else if (selectedGoalName_BarChart1 != "")
        {
            _f = StatCalculationFilter.Goal;
        }
        else if (selectedTaskName_BarChart1 != "")
        {
            _f = StatCalculationFilter.Task;
        }



        List<BarChartInfoText> barChartInfos = new List<BarChartInfoText>();
        int _time = _w_Or_m == 0 ? 7 : 30;

        int[] datas = new int[_time];
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i] = 0;
        }

        StatLoad();
        barchart1.Clear();

        int counter = 0;

        for (int i = 1; i <= _time; i++)
        {
            for (int j = 0; j < GoalDATAS.Length; j++)
            {

                if (_f != StatCalculationFilter.Task)
                {
                    // int _sum = 0;
                    for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
                    {
                        if (GoalDATAS[j].dailyScores[k].GetDateTime().Date >= Today.AddDays((_rewind - 1) * _time) && GoalDATAS[j].dailyScores[k].GetDateTime().Date < Today.AddDays(_rewind * _time))
                        {
                            if (_f == StatCalculationFilter.All)
                            {

                                if (GoalDATAS[j].dailyScores[k].GetDateTime().Date == Today.AddDays((_rewind * _time) - (i)))
                                {
                                    if(GoalDATAS[j].dailyScores[k].isRestDay)
                                    {
                                        datas[i - 1] = int.MaxValue;
                                    }
                                    else
                                    {
                                        datas[i - 1] += GoalDATAS[j].dailyScores[k].amount;
                                    }
                                   
                                    counter++;


                                }
                            }
                            else if (_f == StatCalculationFilter.Goal)
                            {
                                if (GoalDATAS[j].name == selectedGoalName_BarChart1)
                                {
                                    if (GoalDATAS[j].dailyScores[k].GetDateTime().Date == Today.AddDays((_rewind * _time) - (i)))
                                    {
                                        datas[i - 1] += GoalDATAS[j].dailyScores[k].amount;
                                        counter++;


                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int l = 0; l < GoalDATAS[j].modifications.Count; l++)
                    {
                        if (GoalDATAS[j].modifications[l].taskName == selectedTaskName_BarChart1)
                        {
                            if (GoalDATAS[j].modifications[l].GetDateTime().Date >= Today.AddDays((_rewind - 1) * _time) && GoalDATAS[j].modifications[l].GetDateTime().Date < Today.AddDays(_rewind * _time))
                            {
                                if (GoalDATAS[j].modifications[l].GetDateTime() == Today.AddDays((_rewind * _time) - (i)))
                                {

                                    datas[i - 1] += GoalDATAS[j].modifications[l].amount;
                                    counter++;


                                }
                            }

                        }
                    }

                }
            }
        }

        int _todayPoint = 0;
        if(_rewind == 0)
        {
            for (int i = 0; i <  GoalDATAS.Length; i++)
            {
                for (int j = 0; j < GoalDATAS[i].tasks.Count; j++)
                {
                    if(GoalDATAS[i].tasks[j].isEditedToday)
                    {
                        if (_f == StatCalculationFilter.Task)
                        {
                            if (GoalDATAS[i].tasks[j].name == selectedTaskName_BarChart1)
                            {
                                _todayPoint += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[i].tasks[j]);
                            }
                        }
                        else if(_f == StatCalculationFilter.Goal)
                        {
                            if (GoalDATAS[i].name == selectedGoalName_BarChart1)
                            {
                                _todayPoint += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[i].tasks[j]);
                            }
                        }
                        else
                        {
                                _todayPoint += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[i].tasks[j]);
                            
                        }
                    }
                }
            }
        }



        if (_w_Or_m == 0)
        {

            if (_rewind == 0)
            {
                for (int i = 6; i > -1; i--)
                {
                    barChartInfos.Add(new BarChartInfoText(datas[i], RuntimeTranslator.TranslateDayOfWeek((DateTime.Today.AddDays(-i - 1).DayOfWeek)).Substring(0, 3)));
                }


                barChartInfos.Add(new BarChartInfoText(_todayPoint, RuntimeTranslator.TranslateDayOfWeek((DateTime.Today.DayOfWeek)).Substring(0, 3)));

                barchart1.LoadData(barChartInfos.ToArray(), true, true);
            }
            else
            {
                for (int i = 6; i > -1; i--)
                {
                    if (i == 6)
                    {
                        DateTime _t = Today.AddDays((_rewind - 1) * _time);
                        string _h = _t.Month.ToString().Length == 1 ? "0" + _t.Month + "." : _t.Month + ".";
                        string _d = _t.Day.ToString().Length == 1 ? "0" + _t.Day + "." : _t.Day + ".";

                        barChartInfos.Add(new BarChartInfoText(datas[i], _h + _d));
                    }
                    else if (i == 0)
                    {
                        DateTime _t = Today.AddDays(_rewind * _time - 1);
                        string _h = _t.Month.ToString().Length == 1 ? "0" + _t.Month + "." : _t.Month + ".";
                        string _d = _t.Day.ToString().Length == 1 ? "0" + _t.Day + "." : _t.Day + ".";

                        barChartInfos.Add(new BarChartInfoText(datas[i], _h + _d));
                    }
                    else
                    {
                        barChartInfos.Add(new BarChartInfoText(datas[i], ""));
                    }
                }

                barchart1.LoadData(barChartInfos.ToArray(), false, true);
            }


        }
        else
        {

            if (_rewind == 0)
            {
                for (int i = 29; i > -1; i--)
                {
                    if (i == 29)
                    {
                        DateTime _oneMonthEarlier = Today.AddDays((_rewind - 1) * _time);
                        string _hm = _oneMonthEarlier.Month.ToString().Length == 1 ? "0" + _oneMonthEarlier.Month + "." : _oneMonthEarlier.Month + ".";
                        string _dm = _oneMonthEarlier.Day.ToString().Length == 1 ? "0" + _oneMonthEarlier.Day + "." : _oneMonthEarlier.Day + ".";

                        barChartInfos.Add(new BarChartInfoText(datas[i], _hm + _dm));
                    }  
                    else
                    {
                        barChartInfos.Add(new BarChartInfoText(datas[i], ""));
                    }
                }
                DateTime _t = Today;
                string _h = _t.Month.ToString().Length == 1 ? "0" + _t.Month + "." : _t.Month + ".";
                string _d = _t.Day.ToString().Length == 1 ? "0" + _t.Day + "." : _t.Day + ".";
                barChartInfos.Add(new BarChartInfoText(_todayPoint, _h + _d));

                barchart1.LoadData(barChartInfos.ToArray(), false, false);
            }
            else
            {
                 for (int i = 29; i > -1; i--)
                {
                    if (i == 29)
                    {
                        DateTime _t = Today.AddDays((_rewind - 1) * _time);
                        string _h = _t.Month.ToString().Length == 1 ? "0" + _t.Month + "." : _t.Month + ".";
                        string _d = _t.Day.ToString().Length == 1 ? "0" + _t.Day + "." : _t.Day + ".";

                        barChartInfos.Add(new BarChartInfoText(datas[i], _h + _d));
                    }
                    else if (i == 0)
                    {
                        DateTime _t = Today.AddDays(_rewind * _time - 1);
                        string _h = _t.Month.ToString().Length == 1 ? "0" + _t.Month + "." : _t.Month + ".";
                        string _d = _t.Day.ToString().Length == 1 ? "0" + _t.Day + "." : _t.Day + ".";

                        barChartInfos.Add(new BarChartInfoText(datas[i], _h + _d));
                    }
                    else
                    {
                        barChartInfos.Add(new BarChartInfoText(datas[i], ""));
                    }
                }

                barchart1.LoadData(barChartInfos.ToArray(), false, false);
            }
        }
    }



    public void PieChartCalculation(int _rewind, int _type)
    {
        StatLoad();
        piechart1.Clear();


        List<PieChartInfo> _goalPoints = new List<PieChartInfo>();

        DateTime _from = DateTime.MinValue;
        DateTime _to = DateTime.Today;

        int _time = 0;
        bool _skip = false;

        switch (_type)
        {
            case 0: // daily

                _time = 1;


                if (_rewind == 0)
                {


                    for (int i = 0; i < GoalDATAS.Length; i++)
                    {
                        float _points = 0;
                        for (int j = 0; j < GoalDATAS[i].tasks.Count; j++)
                        {
                            _points += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[i].tasks[j]);
                        }
                        _goalPoints.Add(new PieChartInfo(_points, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));
                    }
                    _skip = true;
                }
                /*
                else
                {
                    for (int i = 0; i < GoalDATAS.Length; i++)
                    {
                        float _points = 0;
                        for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
                        {
                            if (GoalDATAS[i].dailyScores[j].GetDateTime().Date >= Today.AddDays(-_rewind))
                            {
                                _points += GoalDATAS[i].dailyScores[j].amount;
                            }
                        }
                        _goalPoints.Add(new PieChartInfo(_points, GoalDATAS[i].name, GoalDATAS[i].color[0] + GoalDATAS[i].color[1]));
                    }
                }
                */
                break;
            case 1: // weekly
                _time = 7;
                break;
            case 2: // monthly
                _time = 30;
                break;
            case 3: // all time
                for (int i = 0; i < GoalDATAS.Length; i++)
                {
                    float _points = 0;
                    for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
                    {
                        _points += GoalDATAS[i].dailyScores[j].amount;
                    }

                    if (_rewind == 0)
                    {
                        for (int k = 0; k < GoalDATAS[i].tasks.Count; k++)
                        {
                            if (GoalDATAS[i].tasks[k].isEditedToday)
                            {
                                _points += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[i].tasks[k]);
                            }
                        }
                    }

                    _goalPoints.Add(new PieChartInfo(_points, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));

                }

                _skip = true;
                break;
        }



        if (!_skip)
        {
            if (_type == 0)
            {
                for (int i = 0; i < GoalDATAS.Length; i++)
                {
                    float _points = 0;
                    for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
                    {
                        if (GoalDATAS[i].dailyScores[j].isRestDay) { continue; }

                        if (GoalDATAS[i].dailyScores[j].GetDateTime().Date == Today.AddDays(_rewind))
                        {
                            _points += GoalDATAS[i].dailyScores[j].amount;
                        }
                    }


                    if (_rewind == 0)
                    {
                        for (int k = 0; k < GoalDATAS[i].tasks.Count; k++)
                        {
                            if (GoalDATAS[i].tasks[k].isEditedToday)
                            {
                                _points += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[i].tasks[k]);
                            }
                        }
                    }

                    _goalPoints.Add(new PieChartInfo(_points, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));
                }
            }
            else
            {


                for (int i = 0; i < GoalDATAS.Length; i++)
                {
                    float _points = 0;
                    for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
                    {
                        if (GoalDATAS[i].dailyScores[j].isRestDay) { continue; }

                        if (GoalDATAS[i].dailyScores[j].GetDateTime().Date >= Today.AddDays(((_rewind - 1) * _time))
                            && (GoalDATAS[i].dailyScores[j].GetDateTime().Date < Today.AddDays((_rewind * _time))))
                        {
                            _points += GoalDATAS[i].dailyScores[j].amount;
                        }
                    }


                    if (_rewind == 0)
                    {
                        for (int k = 0; k < GoalDATAS[i].tasks.Count; k++)
                        {
                            if (GoalDATAS[i].tasks[k].isEditedToday)
                            {
                                _points += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[i].tasks[k]);
                            }
                        }
                    }

                    _goalPoints.Add(new PieChartInfo(_points, GoalDATAS[i].name, GoalDATAS[i].color[0] , GoalDATAS[i].color[1]));
                }
            }
        }




        if (_type != 3)
        {
            _from = Today.AddDays(((_rewind - 1) * _time));
            _to = Today.AddDays((_rewind * _time));

        }
        else
        {
            _from = GetEarliestDailyScore();
            _to = Today.Date;
        }


        string _fromMonth = _from.Month.ToString().Length == 1 ? "0" + _from.Month : _from.Month.ToString();
        string _toMonth = _to.Month.ToString().Length == 1 ? "0" + _to.Month : _to.Month.ToString();

        string _fromDay = _from.Day.ToString().Length == 1 ? "0" + _from.Day : _from.Day.ToString();
        string _toDay = _to.Day.ToString().Length == 1 ? "0" + _to.Day : _to.Day.ToString();

        if (_type != 0)
        {
            piechart1.LoadData(_goalPoints.ToArray(), _fromMonth + "." + _fromDay + " - " + _toMonth + "." + _toDay);
        }
        else if (_type == 0)
        {
            piechart1.LoadData(_goalPoints.ToArray(), _toMonth + "." + _toDay);
        }

    }


    public void Top3BarChartCalculation(int _type, int _rewind)
    {
       

        barChartTop3.Clear();
        StatLoad();

        List<Top3BarChartData> _datas = new List<Top3BarChartData>();
        bool _skip = false;

        DateTime _from = DateTime.MinValue;
        DateTime _to = DateTime.Today;

        int _time = 0;
        switch(_type)
        {
            case 0:
                _time = 1;
                if(_rewind == 0)
                {
                    _skip = true;
                    for (int i = 0; i < GoalDATAS.Length; i++)
                    {

                        Top3BarChartData _thisGoal = new Top3BarChartData(0, GoalDATAS[i].spriteId, GoalDATAS[i].color[0] + GoalDATAS[i].color[1]);

                        for (int j = 0; j < GoalDATAS[i].tasks.Count; j++)
                        {
                            if (GoalDATAS[i].tasks[j].isEditedToday)
                            {
                              _thisGoal.amount += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[i].tasks[j]);
                            }
                        }
                        _datas.Add(_thisGoal);
                    }

                    
                }
                break;
            case 1:
                _time = 7;
                break;
            case 2:
                _time = 30;
                break;
        }





        if (!_skip)
        {
            for (int i = 0; i < GoalDATAS.Length; i++)
            {
                Top3BarChartData _thisGoal = new Top3BarChartData(0, GoalDATAS[i].spriteId, GoalDATAS[i].color[0] + GoalDATAS[i].color[1]);
                for (int j = 0; j < GoalDATAS[i].dailyScores.Count; j++)
                {
                    if (GoalDATAS[i].dailyScores[j].isRestDay) { continue; }

                    if (GoalDATAS[i].dailyScores[j].GetDateTime().Date >= Today.AddDays((_rewind - 1) * _time) && GoalDATAS[i].dailyScores[j].GetDateTime().Date < Today.AddDays(_rewind * _time))
                    {
                        _thisGoal.amount += GoalDATAS[i].dailyScores[j].amount;
                    }
                }
                _datas.Add(_thisGoal);
            }
        }


        Top3BarChartData _first;
        Top3BarChartData _second;
        List<Top3BarChartData> _top3 = new List<Top3BarChartData>();

        _top3.Add( FindMax(_datas.ToArray(), out _first));
        _top3.Add(FindMax(_datas.ToArray(), _first, out _second));
        _top3.Add(FindMax(_datas.ToArray(), _first, _second));

        List<BarChartInfoImage> _infos = new List<BarChartInfoImage>();

        _infos.Add(new BarChartInfoImage(_top3[1].amount, _top3[1].pictoId));
        _infos.Add(new BarChartInfoImage(_top3[0].amount, _top3[0].pictoId));
        _infos.Add(new BarChartInfoImage(_top3[2].amount, _top3[2].pictoId));

        Color[] _colors = new Color[3];
        _colors[0] = _top3[1].goalColor;
        _colors[1] = _top3[0].goalColor;
        _colors[2] = _top3[2].goalColor;

        _from = Today.AddDays(((_rewind - 1) * _time));
        _to = Today.AddDays((_rewind * _time));

        string _fromMonth = _from.Month.ToString().Length == 1 ? "0" + _from.Month : _from.Month.ToString();
        string _toMonth = _to.Month.ToString().Length == 1 ? "0" + _to.Month : _to.Month.ToString();

        string _fromDay = _from.Day.ToString().Length == 1 ? "0" + _from.Day : _from.Day.ToString();
        string _toDay = _to.Day.ToString().Length == 1 ? "0" + _to.Day : _to.Day.ToString();

        if (_type != 0)
        {
            barChartTop3.LoadDataWithImages(_infos.ToArray(),_colors, _fromMonth + "." + _fromDay + " - " + _toMonth + "." + _toDay);
        }
        else if (_type == 0)
        {
            barChartTop3.LoadDataWithImages(_infos.ToArray(),_colors, _toMonth + "." + _toDay);
        }


    }


    public void RollingAverageBarChartCalculation()
    {
        StatCalculationFilter _f = StatCalculationFilter.All;
        if (selectedGoalName_RollingAverage == "" && selectedTaskName_RollingAverage == "")
        {
            _f = StatCalculationFilter.All;
        }
        else if (selectedGoalName_RollingAverage != "")
        {
            _f = StatCalculationFilter.Goal;
        }
        else if (selectedTaskName_RollingAverage != "")
        {
            _f = StatCalculationFilter.Task;
        }
        // name filters

        StatLoad();
        barChartRollingAverage.Clear();


        int[][] _divide = new int[7][];
        int[][] _weeklyData = new int[7][];
        for (int i = 0; i < _weeklyData.Length; i++)
        {
            _weeklyData[i] = new int[7];
            _divide[i] = new int[7];
            for (int o = 0; o < _weeklyData[i].Length; o++)
            {
                _weeklyData[i][o] = 0;
                _divide[i][o] = 0;
            }
        }


        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < GoalDATAS.Length; j++)
            {
                if (_f != StatCalculationFilter.Task)
                {
                    if (_f == StatCalculationFilter.All)
                    {
                        if (i == 0)
                        {
                            for (int tasks = 0; tasks < GoalDATAS[j].tasks.Count; tasks++)
                            {
                                if (GoalDATAS[j].tasks[tasks].isEditedToday)
                                {
                                    _weeklyData[0][0] += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[j].tasks[tasks]);
                                }
                            }

                            for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
                            {
                                if (GoalDATAS[j].dailyScores[k].isRestDay) { continue; }

                                for (int week = 1; week < 7; week++)
                                {
                                    if (GoalDATAS[j].dailyScores[k].GetDateTime().Date == Today.AddDays(-week))
                                    {
                                        _weeklyData[0][week] += GoalDATAS[j].dailyScores[k].amount;
                                        _divide[0][week]++; //= 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
                            {
                                if (GoalDATAS[j].dailyScores[k].isRestDay) { continue; }

                                for (int week = 0; week < 7; week++)
                                {
                                    if (GoalDATAS[j].dailyScores[k].GetDateTime().Date == Today.AddDays(-(week + i)))
                                    {
                                        _weeklyData[i][week] += GoalDATAS[j].dailyScores[k].amount;
                                        _divide[i][week]++;
                                    }
                                }
                            }
                        }
                    }
                    else if(_f == StatCalculationFilter.Goal)
                    {
                        if (i == 0)
                        {
                            if (GoalDATAS[j].name == selectedGoalName_RollingAverage)
                            {
                                for (int tasks = 0; tasks < GoalDATAS[j].tasks.Count; tasks++)
                                {
                                    _weeklyData[0][0] += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[j].tasks[tasks]);
                                }

                                for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
                                {
                                    if (GoalDATAS[j].dailyScores[k].isRestDay) { continue; }

                                    for (int week = 1; week < 7; week++)
                                    {
                                        if (GoalDATAS[j].dailyScores[k].GetDateTime().Date == Today.AddDays(-week))
                                        {
                                            _weeklyData[0][week] += GoalDATAS[j].dailyScores[k].amount;
                                            _divide[0][week]++;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (GoalDATAS[j].name == selectedGoalName_RollingAverage)
                            {
                                for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
                                {
                                    if (GoalDATAS[j].dailyScores[k].isRestDay) { continue; }

                                    for (int week = 0; week < 7; week++)
                                    {
                                        if (GoalDATAS[j].dailyScores[k].GetDateTime().Date == Today.AddDays(-(week + i)))
                                        {
                                         
                                            _weeklyData[i][week] += GoalDATAS[j].dailyScores[k].amount;
                                            _divide[i][week]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                   
                }
                else if (_f == StatCalculationFilter.Task)
                {
                    for (int l = 0; l < GoalDATAS[j].modifications.Count; l++)
                    {
                        if (GoalDATAS[j].modifications[l].taskName == selectedTaskName_RollingAverage)
                        {
                            if (i == 0)
                            {
                                for (int tasks = 0; tasks < GoalDATAS[j].tasks.Count; tasks++)
                                {
                                    if (GoalDATAS[j].tasks[tasks].name == selectedTaskName_RollingAverage)
                                    {
                                        if (GoalDATAS[j].tasks[tasks].isEditedToday)
                                        {
                                            _weeklyData[0][0] += TaskPointCalculator.GetPointsFromCurrentValue(GoalDATAS[j].tasks[tasks]);
                                        }
                                    }
                                }

                                for (int week = 1; week < 7; week++)
                                {
                                    if (GoalDATAS[j].modifications[l].GetDateTime().Date == Today.AddDays(-(week)))
                                    {
                                        _weeklyData[0][week] += GoalDATAS[j].modifications[l].amount;
                                        _divide[0][week]++;
                                    }
                                }
                            }
                            else
                            {
                                for (int week = 0; week < 7; week++)
                                {
                                    if (GoalDATAS[j].modifications[l].GetDateTime().Date == Today.AddDays(-(week + i)))
                                    {
                                        _weeklyData[i][week] += GoalDATAS[j].modifications[l].amount;
                                        _divide[i][week]++;
                                    }
                                }
                            }
                        }
                    }
                }

            }
        
        }


        List<BarChartInfoText> _infos = new List<BarChartInfoText>();


        int _sum = 0;
        int _div = 0;
        int _week = 0;
        for (int _weekNumber = _weeklyData.Length - 1; _weekNumber > -1 ; _weekNumber--)
        {
            _sum = 0;
            _div = 0;
            for (_week = 0; _week < _weeklyData[_weekNumber].Length; _week++)
            {
                _sum += _weeklyData[_weekNumber][_week];
                _div += _divide[_weekNumber][_week];
               // print(_sum + " " + _div);
            }

            if(_div == 0)
            {
                _sum = 0;
            }
            else
            {
               // print(_sum + " " + _div + " " + (float)_sum / _div);
                _sum = Mathf.RoundToInt((float)_sum / _div);
            }

            if (_sum > 0)
            {
                _infos.Add(new BarChartInfoText(_sum, RuntimeTranslator.TranslateDayOfWeek(Today.AddDays(-(_week + _weekNumber)).DayOfWeek).Substring(0, 3)));
            }
           
        }


        barChartRollingAverage.LoadData(_infos.ToArray(), true, true);
        

    }

    private Top3BarChartData FindMax(Top3BarChartData[] _datas, out Top3BarChartData _maxNumber)
    {
        Top3BarChartData _max = new Top3BarChartData();
        _max.amount = 0;

        for (int i = 0; i < _datas.Length; i++)
        {
            if(_datas[i].amount > _max.amount)
            {
                _max = _datas[i];
            }
        }

        _maxNumber = _max;
        return _max;
    }

    private Top3BarChartData FindMax(Top3BarChartData[] _datas , Top3BarChartData _notThisNumber, out Top3BarChartData _maxNumber)
    {
        Top3BarChartData _max = new Top3BarChartData();
        _max.amount = 0;

        for (int i = 0; i < _datas.Length; i++)
        {
            if (_datas[i].amount > _max.amount && !_datas[i].Equals(_notThisNumber))
            {
                _max = _datas[i];
            }
        }

        _maxNumber = _max;
        return _max;
    }

    private Top3BarChartData FindMax(Top3BarChartData[] _datas, Top3BarChartData _notThisNumber, Top3BarChartData _notThisNumber2)
    {
        Top3BarChartData _max = new Top3BarChartData();
        _max.amount = 0;

        for (int i = 0; i < _datas.Length; i++)
        {
            if (_datas[i].amount > _max.amount && !_datas[i].Equals(_notThisNumber) && !_datas[i].Equals(_notThisNumber2))
            {
                _max = _datas[i];
            }
        }

        return _max;
    }

}


    // PIECHART

    // pressed daily, weekly, monthly or all time

    // calculate daily
    //      calculate day if exists (check oldest daily score, not previous day)
    //      if rewind is 0 calculate current scores

    // calculate weekly
    //      calculate 7 days using rewind

    // calculate monthly
    //      calculate 30 days using rewind

    // calculate all time
    //      calculate every daily score




    // ----------------------





    // BARCHART

    // pressed weekly or monthly:
    // use 0 as rewind

    //  calculate weekly
    //      calculate all
    //      or
    //      calculate with a certain goal
    //      or
    //      calculate with a certain task


    // or


    //  calculate monthly
    //      calculate all
    //      or
    //      calculate with a certain goal
    //      or
    //      calculate with a certain task


    // ---------------------


    // pressed rewind button:
    // use rewind button counter

    //  calculate weekly
    //      calculate all
    //      or
    //      calculate with a certain goal
    //      or
    //      calculate with a certain task


    // or


    //  calculate monthly
    //      calculate all
    //      or
    //      calculate with a certain goal
    //      or
    //      calculate with a certain task






/*
        int[] monthlyData = new int[30];
        int i = 0;
        int minusdays = -30;
        for (int j = 0; j < GoalDATAS.Length; j++)
        {
            if (GoalDATAS[j].dailyScores == null)
            {
                continue;
            }
            for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
            {

                if (minusdays >=0)
                {
                    break;
                }
                else
                {
                    if (Convert.ToDateTime(GoalDATAS[j].dailyScores[k].time) > Today.AddDays(minusdays))
                    {

                        monthlyData[i] += GoalDATAS[j].dailyScores[k].amount;
                       // print(i);
                        i++;
                        minusdays++;
                    }
                }
            }
        }
        barchart1.Clear();
        if (i >= 1)
        {
            for (int l = 0; l < monthlyData.Length; l++)
            {
                barChartInfosmonth.Add(new BarChartInfo(monthlyData[l], ""));
              //  print(l);
            }
            barchart1.LoadData(barChartInfosmonth.ToArray(), false);
        }
        */


