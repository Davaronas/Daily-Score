using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

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
    [HideInInspector] public GoalManager goalManager = null;
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
    [SerializeField] BarChartHolder barchart;
    public GoalData[] GoalDATAS;


    private void Awake()
    {
        AppManager.OnTaskValueChanged += OnTaskValueChanged;
        AppManager.OnNewDayStartedDuringRuntime += OnNewDayStartedDuringRuntime;
        goalManager = FindObjectOfType<GoalManager>();
        AppManager.OnGoalOpened += OnGoalOpened;
    }
    private void OnGoalOpened(Goal _td)
    {
        Invoke(nameof(GoalScoreCalcs), 0.1f);
        Invoke(nameof(TaskGoalCalc), 0.2f);
    }
    private void OnTaskValueChanged(TaskData _td)
    {
        // Amikor a felhaszn�l� v�ltoztat �rt�ket ez h�v�dik. A _td pedig hogy melyik feladat v�ltozott.
        // _td.owner pedig megadja melyik GoalData v�ltozott n�v szerint (ez egy string v�ltoz�)
        // hogy ez melyik goaldata �gy tudod megkapni hogy goalManager.SearchGoalByName(_td.owner);
        // vagy
        // GoalData _gd; goalManager.SearchGoalByName(_td.owner, out _gd); ezen van bool check is hogy siker�lt e megtal�lni/l�tezik e

        // Csak fut�s k�zben �rz�keli a v�ltoztat�sokat, a program indit�skor ez nem fut le
        
        Invoke(nameof( GoalScoreCalcs),0.1f);
        Invoke(nameof(TaskGoalCalc),0.2f);


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
        GoalDATAS = goalManager.GetGoals();
     //   print(GoalDATAS == null);
    }

    void DailyScoreCalc()
    {
        int i = 0;
        dailysc = 0;
        for ( i = 0; i < GoalDATAS.Length; i++)
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
        int [] weeklydata = new int[7];
        weeklyavarage = 0;
        float weeklyfleet=dailysc;
        int daycounter = 0;
        int i = 0;
        for (int l = 0; l < 7; l++)
        {
            weeklydata[l] = 0;
        }
        for(i = 0; i<GoalDATAS.Length; i++)
        {
            for (int k = 0; k < GoalDATAS[i].dailyScores.Count; k++)
            {
                
                if (Convert.ToDateTime( GoalDATAS[i].dailyScores[k].time).Date >= Today.AddDays(-7))
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

        if(daycounter == 0) { return; }

        weeklyavarage = weeklyfleet / daycounter;
       
        weeklyScoreText.text = Math.Round(weeklyavarage,2).ToString();
        weeklyScoreTextStatMenu.text = Math.Round(weeklyavarage,2).ToString();
    }
    void MonthlyAvarageCalc()
    {
        monthlyavarage = 0;
        int monthlyfleet = 0;
        int daycountermonth = 0;
        int i = 0;
            for (int j = 0; j < GoalDATAS.Length; j++)
            {
                if(GoalDATAS[j].dailyScores == null)
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

            if(daycountermonth == 0) { return; }

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

        taskweeklyScoreText.text = Math.Round(weeklytaskpointav,2).ToString();
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
                    
                    if(Convert.ToDateTime(GoalDATAS[j].dailyScores[k].time) >= Today.AddDays(-7))
                    {
                        
                        if(maxweekfleet< GoalDATAS[j].dailyScores[k].amount)
                        {
                            maxweekfleet = GoalDATAS[j].dailyScores[k].amount;
                        }
                    }
                   if(maxmonthfleet < GoalDATAS[j].dailyScores[k].amount)
                    {
                        maxmonthfleet = GoalDATAS[j].dailyScores[k].amount;
                    }
                }
                if(maxmaxfleet < GoalDATAS[j].dailyScores[k].amount)
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
            alltimevarage =alltimeavaragefleet / kcounter;
        }

        maxScoreText.text = maxofmax.ToString();
        HighScoreThisWeekStatMenu.text = maxoftheweek.ToString();
        HighCoreThisMonthScoreStatMenu.text = maxofthemonth.ToString();
        AllTimeBestScoreTextStatMenu.text = maxofmax.ToString();
        AllTimeAvarageScoreTextStatMenu.text = Math.Round(alltimevarage,2).ToString();
        
    }
    public void TaskMaskCalc()
    {
        GoalData currentselectedGoal = goalManager.GetCurrentlySelectedGoal().GetGoalData();
        int taskmaxfleet = currentselectedGoal.current;
        for (int j = 0; j < currentselectedGoal.dailyScores.Count; j++)
        {
            if(taskmaxfleet < currentselectedGoal.dailyScores[j].amount)
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
        for (i = 0; i < GoalDATAS.Length; i++)
        {
            for (int k = 0; k < GoalDATAS[i].dailyScores.Count; k++)
            {

                if (Convert.ToDateTime(GoalDATAS[i].dailyScores[k].time).Date >= Today.AddDays(-7))
                {
                    switch (Convert.ToDateTime(GoalDATAS[i].dailyScores[k].time).DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            weeklydata[0] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week0");
                            barChartInfos.Add(new BarChartInfo(weeklydata[0],RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Monday).Substring(1,3)));
                            break;
                        case DayOfWeek.Tuesday:
                            weeklydata[1] += GoalDATAS[i].dailyScores[k].amount;
                            // print("Week1");
                            barChartInfos.Add(new BarChartInfo(weeklydata[1], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Tuesday).Substring(1, 3)));
                            break;
                        case DayOfWeek.Wednesday:
                            weeklydata[2] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week2");
                            barChartInfos.Add(new BarChartInfo(weeklydata[2], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Wednesday).Substring(1, 3)));
                            break;
                        case DayOfWeek.Thursday:
                            weeklydata[3] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week3");
                            barChartInfos.Add(new BarChartInfo(weeklydata[3], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Thursday).Substring(1, 3)));
                            break;
                        case DayOfWeek.Friday:
                            weeklydata[4] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week4");
                            barChartInfos.Add(new BarChartInfo(weeklydata[4], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Friday).Substring(1, 3)));
                            break;
                        case DayOfWeek.Saturday:
                            weeklydata[5] += GoalDATAS[i].dailyScores[k].amount;
                            //  print("Week5");
                            barChartInfos.Add(new BarChartInfo(weeklydata[5], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Saturday).Substring(1, 3)));
                            break;
                        case DayOfWeek.Sunday:
                            weeklydata[6] += GoalDATAS[i].dailyScores[k].amount;
                            //   print("Week6");
                            barChartInfos.Add(new BarChartInfo(weeklydata[6], RuntimeTranslator.TranslateDayOfWeek(DayOfWeek.Sunday).Substring(1, 3)));
                            break;
                    }
                }
            }
        }
        barchart.Clear();
        barchart.LoadData(barChartInfos.ToArray(),true);
    }

    void Start()
    {
        
        Today = DateTime.Today;
        DateTime LastWeek = Today.AddDays(-7);
        DateTime Test;
        DateTime Lastmodificationfleet = DateTime.Now.AddYears(-10); //Ezzel keresem a maxot
        int LastmodificationDur = 0; //Ez lesz a legut�bbi m�dos�t�s d�tuma amir�l sz�molok
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
