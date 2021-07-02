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
    public int weeklyavarage;
    public int monthlyavarage;
    List<DailyScoreStruct> DailyScoreStructsList = new List<DailyScoreStruct>();
    [SerializeField] TMP_Text dailyScoreText;
    public GoalData[] GoalDATAS;


    private void Awake()
    {
        AppManager.OnTaskValueChanged += OnTaskValueChanged;
        AppManager.OnNewDayStartedDuringRuntime += OnNewDayStartedDuringRuntime;
        goalManager = FindObjectOfType<GoalManager>();
    }

    private void OnTaskValueChanged(TaskData _td)
    {
        // Amikor a felhasználó változtat értéket ez hívódik. A _td pedig hogy melyik feladat változott.
        // _td.owner pedig megadja melyik GoalData változott név szerint (ez egy string változó)
        // hogy ez melyik goaldata úgy tudod megkapni hogy goalManager.SearchGoalByName(_td.owner);
        // vagy
        // GoalData _gd; goalManager.SearchGoalByName(_td.owner, out _gd); ezen van bool check is hogy sikerült e megtalálni/létezik e

        // Csak futás közben érzékeli a változtatásokat, a program inditáskor ez nem fut le
        print("Task");
        ScoreCalcs();
    }

    private void ScoreCalcs()
    {
        StatLoad();
        DailyScoreCalc();
        WeeklyScoreCal();
        MonthlyAvarageCalc();
    }

    void StatLoad()
    {
        print(goalManager);
        GoalDATAS = goalManager.GetGoals();
        print(GoalDATAS == null);
    }

    void DailyScoreCalc()
    {
        int i = 0;
        dailysc = 0;
        do
        {
            if (GoalDATAS[i].GetLastModificationTime().Day == Today.Day)
            {
                //dailysc += GoalDATAS[i].lastChange.amount;
                dailysc += GoalDATAS[i].current;
            }
            i++;
        }
        while (i < GoalDATAS.Length);
        dailyScoreText.text = dailysc.ToString();
    }

    void WeeklyScoreCal()
    {
        int [] weeklydata = new int[7];
        weeklyavarage = 0;
        int weeklyfleet=0;
        int i = 0;
        do
        {
            if(GoalDATAS[i].GetLastModificationTime() >= Today.AddDays(-7))
            {
                if ((GoalDATAS[i].GetLastModificationTime().DayOfWeek == DayOfWeek.Monday))
                {
                    weeklydata[1] += GoalDATAS[i].current;
                }
                if ((GoalDATAS[i].GetLastModificationTime().DayOfWeek == DayOfWeek.Tuesday))
                {
                    weeklydata[2] += GoalDATAS[i].current;
                }
                if ((GoalDATAS[i].GetLastModificationTime().DayOfWeek == DayOfWeek.Wednesday))
                {
                    weeklydata[3] += GoalDATAS[i].current;
                }
                if ((GoalDATAS[i].GetLastModificationTime().DayOfWeek == DayOfWeek.Thursday))
                {
                    weeklydata[4] += GoalDATAS[i].current;
                }
                if ((GoalDATAS[i].GetLastModificationTime().DayOfWeek == DayOfWeek.Friday))
                {
                    weeklydata[5] += GoalDATAS[i].current;
                }
                if ((GoalDATAS[i].GetLastModificationTime().DayOfWeek == DayOfWeek.Saturday))
                {
                    weeklydata[6] += GoalDATAS[i].current;
                }
                if ((GoalDATAS[i].GetLastModificationTime().DayOfWeek == DayOfWeek.Sunday))
                {
                    weeklydata[7] += GoalDATAS[i].current;
                }
            }
            i++;
        } while (GoalDATAS.Length == i);
        for (int j = 1; j <= 7; j++)
        {
            weeklyfleet += weeklydata[i];
        }
        weeklyavarage = weeklyfleet / 7;
        dailyScoreText.text = weeklyavarage.ToString();
    }
    void MonthlyAvarageCalc()
    {
        monthlyavarage = 0;
        int monthlyfleet = 0;
        int i = 0;
        do
        {
            if (GoalDATAS[i].GetLastModificationTime() >= Today.AddDays(-30))
            {
                monthlyfleet += GoalDATAS[i].current;
            }
            i++;
        } while (GoalDATAS.Length == i);
        monthlyavarage = monthlyfleet / 30;
        dailyScoreText.text = monthlyavarage.ToString();
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
        print(Today.Day);
        int max = 0;//A MAX ami mindenkori lesz.
        lastlogdur = Today.Day - AppManager.lastLogin.Day;
        ScoreCalcs();
       
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
    }
}
