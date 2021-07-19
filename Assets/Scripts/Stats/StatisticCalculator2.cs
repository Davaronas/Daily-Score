using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

/// <summary>
/// StatCalc version 2.7.2
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
    [SerializeField] PieChart piechart1;
    [SerializeField] PieChart piechart2;
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] BarChartHolder barchart1;
    [SerializeField] BarChartHolder barchart2;
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        AppManager.OnTaskValueChanged += OnTaskValueChanged;
        AppManager.OnNewDayStartedDuringRuntime += OnNewDayStartedDuringRuntime;
       
        AppManager.OnGoalOpened += OnGoalOpened;
    }
    private void OnGoalOpened(Goal _td)
    {
        Invoke(nameof(GoalScoreCalcs), 0.1f);
        Invoke(nameof(TaskGoalCalc), 0.2f);
    }
    private void OnTaskValueChanged(TaskData _td)
    {
        // Amikor a felhasználó változtat értéket ez hívódik. A _td pedig hogy melyik feladat változott.
        // _td.owner pedig megadja melyik GoalData változott név szerint (ez egy string változó)
        // hogy ez melyik goaldata úgy tudod megkapni hogy goalManager.SearchGoalByName(_td.owner);
        // vagy
        // GoalData _gd; goalManager.SearchGoalByName(_td.owner, out _gd); ezen van bool check is hogy sikerült e megtalálni/létezik e

        // Csak futás közben érzékeli a változtatásokat, a program inditáskor ez nem fut le

        Invoke(nameof(GoalScoreCalcs), 0.1f);
        Invoke(nameof(TaskGoalCalc), 0.2f);


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
        float weeklyfleet = dailysc;
        int daycounter = 0;
        int i = 0;
        for (int l = 0; l < 7; l++)
        {
            weeklydata[l] = 0;
        }
        for (i = 0; i < GoalDATAS.Length; i++)
        {
            for (int k = 0; k < GoalDATAS[i].dailyScores.Count; k++)
            {

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

        if (daycounter == 0) { return; }

        weeklyavarage = weeklyfleet / daycounter;

        weeklyScoreText.text = Math.Round(weeklyavarage, 2).ToString();
        weeklyScoreTextStatMenu.text = Math.Round(weeklyavarage, 2).ToString();
    }
    void MonthlyAvarageCalc()
    {
        monthlyavarage = 0;
        int monthlyfleet = 0;
        int daycountermonth = 0;
        int i = 0;
        for (int j = 0; j < GoalDATAS.Length; j++)
        {
            if (GoalDATAS[j].dailyScores == null)
            {
                continue;
            }
            for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
            {
                if (Convert.ToDateTime(GoalDATAS[j].dailyScores[k].time) >= Today.AddDays(-30))
                {
                    daycountermonth++;
                    monthlyfleet += GoalDATAS[j].dailyScores[k].amount;
                }
            }
        }

        if (daycountermonth == 0) { return; }

        monthlyavarage = monthlyfleet / daycountermonth;
        monthlyScoreText.text = Math.Round(weeklyavarage, 2).ToString();
    }

    void TaskDailyCalc()
    {
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
        GoalData currentselectedGoal = goalManager.GetCurrentlySelectedGoal().GetGoalData();
        float weeklytaskpointav = 0;
        int kcounter = 0;
        float weeklytaskfleet = dailytaskpoint;
        int[] weeklydata = new int[7];
        for (int k = 0; k < currentselectedGoal.dailyScores.Count; k++)
        {

            if (Convert.ToDateTime(currentselectedGoal.dailyScores[k].time).Date >= Today.AddDays(-7))
            {
                kcounter++;

                switch (Convert.ToDateTime(currentselectedGoal.dailyScores[k].time).DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        weeklydata[0] += currentselectedGoal.dailyScores[k].amount;
                        print("Week0");
                        break;
                    case DayOfWeek.Tuesday:
                        weeklydata[1] += currentselectedGoal.dailyScores[k].amount;
                        print("Week1");
                        break;
                    case DayOfWeek.Wednesday:
                        weeklydata[2] += currentselectedGoal.dailyScores[k].amount;
                        print("Week2");
                        break;
                    case DayOfWeek.Thursday:
                        weeklydata[3] += currentselectedGoal.dailyScores[k].amount;
                        print("Week3");
                        break;
                    case DayOfWeek.Friday:
                        weeklydata[4] += currentselectedGoal.dailyScores[k].amount;
                        print("Week4");
                        break;
                    case DayOfWeek.Saturday:
                        weeklydata[5] += currentselectedGoal.dailyScores[k].amount;
                        print("Week5");
                        break;
                    case DayOfWeek.Sunday:
                        weeklydata[6] += currentselectedGoal.dailyScores[k].amount;
                        print("Week6");
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

    }
    public void TaskMaskCalc()
    {
        GoalData currentselectedGoal = goalManager.GetCurrentlySelectedGoal().GetGoalData();
        int taskmaxfleet = currentselectedGoal.current;
        for (int j = 0; j < currentselectedGoal.dailyScores.Count; j++)
        {
            if (taskmaxfleet < currentselectedGoal.dailyScores[j].amount)
            {
                taskmaxfleet = currentselectedGoal.dailyScores[j].amount;
            }
        }

        taskmaxScoreText.text = taskmaxfleet.ToString();
    }

    public void Weeklygraph()
    {
        List<BarChartInfo> barChartInfos = new List<BarChartInfo>();
        StatLoad();
        int i;
        int[] weeklydata = new int[7];
        int counter = 0;
        for (i = 0; i < GoalDATAS.Length; i++)
        {
               // int _sum = 0;
            for (int k = 0; k < GoalDATAS[i].dailyScores.Count; k++)
            {
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

        foreach(int _w in weeklydata)
        {
            print(_w);
        }

        barchart1.Clear();
        if (counter >= 1)
        {
            #region LoadDays 


            switch(DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    break;
                case DayOfWeek.Tuesday:
                    barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    break;
                case DayOfWeek.Wednesday:
                    barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    break;
                case DayOfWeek.Thursday:
                    barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    break;
                case DayOfWeek.Friday:
                    barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    break;
                case DayOfWeek.Saturday:
                    barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    break;
                case DayOfWeek.Sunday:
                    barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[0], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(0, 3)));
                    barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(0, 3)));
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

            barchart1.LoadData(barChartInfos.ToArray(), true);
        }

    }

    public void MonthlyGraph()
    {

        DateTime[] _startAndFinishTime = new DateTime[2];
        List<BarChartInfo> barChartInfosmonth = new List<BarChartInfo>();
        int[] monthlyData = new int[30];
        for (int i = 1; i <= 30; i++)
        {
            for (int j = 0; j < GoalDATAS.Length; j++)
            {
                for (int k = 0; k < GoalDATAS[j].dailyScores.Count; k++)
                {
                  

                    if (GoalDATAS[j].dailyScores[k].GetDateTime() == Today.AddDays(-i))
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
            if(l == monthlyData.Length - 1)
            {
                barChartInfosmonth.Add(new BarChartInfo(monthlyData[l], _startAndFinishTime[1].Month + "/" + _startAndFinishTime[1].Day));
            }
            else if(l == 0)
            {
                barChartInfosmonth.Add(new BarChartInfo(monthlyData[l], _startAndFinishTime[0].Month + "/" + _startAndFinishTime[0].Day));
            }
            else
            barChartInfosmonth.Add(new BarChartInfo(monthlyData[l], ""));
        }
        barchart1.LoadData(barChartInfosmonth.ToArray(), false);
        

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


    }

    public void GoalPieDaily()
    {
        StatLoad();
        List<PieChartInfo> PiteNap = new List<PieChartInfo>();
        for (int i = 0; i < GoalDATAS.Length; i++)
        {
            PiteNap.Add(new PieChartInfo(GoalDATAS[i].current, GoalDATAS[i].name, GoalDATAS[i].color[0]));
        }
        piechart1.Clear();
        if (PiteNap.Count > 0)
        {
            piechart1.LoadData(PiteNap.ToArray());
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
                PiteHete.Add(new PieChartInfo(sziauram, GoalDATAS[i].name,GoalDATAS[i].color[0]));
            
        }
        piechart1.Clear();
        if (PiteHete.Count > 0)
        {
            piechart1.LoadData(PiteHete.ToArray());
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
            PiteHava.Add(new PieChartInfo(sziauram, GoalDATAS[i].name,  GoalDATAS[i].color[0]));

        }
        piechart1.Clear();
        if (PiteHava.Count > 0)
        {
            piechart1.LoadData(PiteHava.ToArray());
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
            PiteMind.Add(new PieChartInfo(sziauram, GoalDATAS[i].name, GoalDATAS[i].color[0]));

        }
        piechart1.Clear();
        if (PiteMind.Count > 0)
        {
            piechart1.LoadData(PiteMind.ToArray());
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
    }
}
