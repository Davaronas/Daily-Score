using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class StatisticCalculator : MonoBehaviour
{
    [HideInInspector] public GoalManager goalManager = null;
     [HideInInspector]public GoalData[] goalDatas = null; //Ezt ki kell kapcsolgatni ha tesztelek és kell.
    void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        goalDatas = goalManager.GetGoals();
        DateTime Today = DateTime.Today;
        DateTime LastWeek = Today.AddDays(-7);
        DateTime Test;
        DateTime Lastmodificationfleet =DateTime.Now.AddYears(-10); //Ezzel keresem a maxot
        int LastmodificationDur=0; //Ez lesz a legutóbbi módosítás dátuma amiről számolok
        int lastmodmaxI = 0;
        Console.WriteLine(Today.ToString("{0}")); //Debug
        StartCoroutine(TimeCheck());
        print(Today.Day);
        int max = 0;//A MAX ami mindenkori lesz.


        if(Today.Date != Lastmodificationfleet.Date) //Napi adatokig kell mennie 
        {
            for (int i = 0; i < goalDatas.Length; i++)
            {
                if (goalDatas[i].GetLastModificationTime() > Lastmodificationfleet)
                {
                    Lastmodificationfleet = goalDatas[i].GetLastModificationTime();
                    lastmodmaxI = i;
                }
            }
        }

        if(Today.Day - Lastmodificationfleet.Day !=0)
        {
            int difference = Today.Day - Lastmodificationfleet.Day;
        }

        //Napi pont számoló
        int dailysc = 0;
        while (true) //Naponta kezdenie kell elölről dailysc resettel
        {
            int i = 0;
            if (goalDatas[i].GetLastModificationTime().Day == Today.Day)
            {
                dailysc += goalDatas[i].lastChange.amount;
            }
            i++;
        }

        //Heti átlag pont számoló
        int avarage; //átlag
        int kkavp = 0; //0
        int kkavperm = kkavp / 7; //0
        for (int i = 0; i < 7; i++)
        {
            int kkmax = dailysc; //Ez folyton változó napi adat.
            if (0 > max)
            {
                max = kkmax; 
            }
            int kkav = 0; //0
        }
        
        
        //  kkavp = kkav;
        

    }
    private void Update()
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
    }
    //int change = goalDatas[i].lastChange.amount;

}
