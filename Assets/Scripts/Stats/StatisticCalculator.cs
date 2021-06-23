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
public class StatisticCalculator : MonoBehaviour
{
    [HideInInspector] public GoalManager goalManager = null;
     [HideInInspector]public GoalData[] goalDatas = null; //Ezt ki kell kapcsolgatni ha tesztelek és kell.
    public DateTime Today;
    public int dailysc = 0;
    public int lastlogdur;
    public int monthlyavarage;
    List<DailyScoreStruct> DailyScoreStructsList = new List<DailyScoreStruct>();
    void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        goalDatas = goalManager.GetGoals();
        Today = DateTime.Today;
        DateTime LastWeek = Today.AddDays(-7);
        DateTime Test;
        DateTime Lastmodificationfleet =DateTime.Now.AddYears(-10); //Ezzel keresem a maxot
        int LastmodificationDur=0; //Ez lesz a legutóbbi módosítás dátuma amiről számolok
        int lastmodmaxI = 0;
        Console.WriteLine(Today.ToString("{0}")); //Debug
        StartCoroutine(TimeCheck());
        print(Today.Day);
        int max = 0;//A MAX ami mindenkori lesz.
        lastlogdur = Today.Day - AppManager.lastLogin.Day;


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
        bool dayhaschanged = false;
        while (dayhaschanged == true) //Naponta kezdenie kell elölről dailysc resettel
        {
            int i = 0;
            if (goalDatas[i].GetLastModificationTime().Day == Today.Day)
            {
                dailysc += goalDatas[i].lastChange.amount;
            }
            i++;
        }
        DailyScoreStructsList.Add(new DailyScoreStruct(Today.Date,dailysc));
       
        //Max számoló
        int avarage; //átlag
        int kkavp = 0; //0
        int kkavperm = kkavp / 7; //0
        for (int i = 0; i < 7; i++)
        {
            int kkmax = dailysc; //Ez folyton változó napi adat.
            if (dailysc > max)
            {
                max = kkmax; 
            }
            int kkav = 0; //0
        }


        string path = Path.Combine(Application.persistentDataPath, "dailyscoresave");
        if (File.Exists(path))
        {
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.IsReadOnly = false;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            DailyScoreStructsList = formatter.Deserialize(stream) as List<DailyScoreStruct>;
            stream.Close();
            fileInfo.IsReadOnly = true;

        }



    }
    //Beolvasó
    public void StatLoad()
    {
        int fleetmonthly=0;
        int fleetweekly=0;
        List<string> dailystats = new List<string>();
        StreamReader dsts = new StreamReader(@"Assets\Scripts\Stats\dailyscore.txt"); //Testhez való elérési útvonal
        try
        {
            string sor;
            do
            {
                sor = dsts.ReadLine();
                if (sor == null) break;
                dailystats.Add(sor);

            } while (sor != null);
        }
        catch (IOException)
        {
            Debug.LogError($"Error loading in the stats!");
            throw;
        }
        string[] cut = new string[2];

       // var lastitem = goalDatas[].GetLastModificationTime();
        //Havi pont számoló
        int monthlenght = 0;
        monthlenght = Today.Month;
        //if ((Today.Month-goalDatas[].GetLastModificationTime() )) 
        for (int i = 0; i <monthlenght ; i++) //Havi debaszogatni kell még
        {
            
            fleetmonthly += DailyScoreStructsList[i].dailyScore;
            if(i<7)
            {
                fleetweekly+=DailyScoreStructsList[i].dailyScore;
            }
            
        }
    }
    public void DailyScore()
    {
        string dscdate_string = Today.Date.ToString();
        string dailyscoreint =dailysc.ToString();
        List<string> dsc = new List<string>();//Lehet nem kell 80%
        //StreamWriter savedsc = new StreamWriter(@"Asstet\Scripts\Stats\dailyscore.txt");
        if (lastlogdur < 1)
        {
            DailyScoreStruct dailyscoreSave;
            dailyscoreSave.DailyDate = Today.Date;
            dailyscoreSave.dailyScore = dailysc;
            DailyScoreStructsList.Add(dailyscoreSave);
           // foreach (string line in dsc)
              //  savedsc.WriteLine($"{Today:d MMMM,yyyy}{dailyscoreint}");
        }
        else
        {
            for (int i = 1; i < lastlogdur; i++)
            {
                DateTime dayi = Today.AddDays(-i);
                
            }
        }
    }

    public void Datasave()
    {
        string path = Path.Combine(Application.persistentDataPath, "dailyscoresave");
        if (File.Exists(path))
        {
            FileInfo fileInfoIfAlreadyExists = new FileInfo(path);
            fileInfoIfAlreadyExists.IsReadOnly = false;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, DailyScoreStructsList);
        stream.Close();
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.IsReadOnly = true;
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
