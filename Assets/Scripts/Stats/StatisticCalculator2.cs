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
    public int monthlyavarage;
    List<DailyScoreStruct> DailyScoreStructsList = new List<DailyScoreStruct>();
    [SerializeField] TMP_Text dailyScoreText;
    public GoalData[] GoalDATAS;


    private void Awake()
    {
        AppManager.OnTaskValueChanged += OnTaskValueChanged;
        goalManager = FindObjectOfType<GoalManager>();
    }

    private void OnTaskValueChanged(TaskData _td)
    {
        // Amikor a felhaszn�l� v�ltoztat �rt�ket ez h�v�dik. A _td pedig hogy melyik feladat v�ltozott.
        // _td.owner pedig megadja melyik GoalData v�ltozott n�v szerint (ez egy string v�ltoz�)
        // hogy ez melyik goaldata �gy tudod megkapni hogy goalManager.SearchGoalByName(_td.owner);
        // vagy
        // GoalData _gd; goalManager.SearchGoalByName(_td.owner, out _gd); ezen van bool check is hogy siker�lt e megtal�lni/l�tezik e

        // Csak fut�s k�zben �rz�keli a v�ltoztat�sokat, a program indit�skor ez nem fut le
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

    }
    void MonthlyAvarageCalc()
    {

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
        print(Today.Day);
        int max = 0;//A MAX ami mindenkori lesz.
        lastlogdur = Today.Day - AppManager.lastLogin.Day;
        ScoreCalcs();
       
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
    }
}
