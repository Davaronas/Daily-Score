using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class StatisticCalculator : MonoBehaviour
{
     public GoalManager goalManager = null;
     public GoalData[] goalDatas = null;
    void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        goalDatas = goalManager.GetGoals();
        DateTime Today = DateTime.Today;
        DateTime LastWeek = Today.AddDays(-7);
        DateTime Test;
        Console.WriteLine(Today.ToString("{0}")); //Debug
        for (int i = 0; i < goalDatas.Length; i++)
        {
            goalDatas[i].GetLastModificationTime();
            int change = goalDatas[i].lastChange.amount;
        }
        int max = 0;//A MAX ami mindenkori lesz.

        while () //Napi adatokig kell mennie 
        { 
            int avarage; //átlag
            int kkavp = 0; //0
            int kkavperm = kkavp / 7; //0
            for (int i = 0; i < 7; i++)
            {
                int kkmax = 0; //Ez folyton változó napi adat.
                if (0 > max)
                {
                    max = kkmax; //0
                }
                int kkav = 0; //0
            }

        }
    }

    
}
