using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class megbasszaakurvaanyjat : MonoBehaviour
{
    [System.Serializable]
    public struct RolledTips
    {
        public string name;
        public string data;
        public string lastdate;
        public DateTime datum;
    }
    RolledTips rolledtip;
    public List<string> datas = new List<string>();
    public List<string> name = new List<string>();
    public List<string> maindata = new List<string>();
    public List<int> Saved_ID = new List<int>();
    IDictionary<int, string> Tips = new Dictionary<int, string>();
    public int c = 0;
    private TipManager tipManager;
    TipManager tipmanager = null;
    private void Awake()
    {
        tipManager = FindObjectOfType<TipManager>();
    }

    public void TipLoad()
    {
        List<string> _tips = new List<string>();
        StreamReader sr = new StreamReader(@"Assets\Scripts\Tips\tips.txt");
        try
        {
            string sor;
            do
            {
                sor = sr.ReadLine();
                if (sor == null) break;
                _tips.Add(sor);

            } while (sor != null);
        }
        catch (IOException)
        {
            Debug.LogError($"Error loading in the tips!");
            throw;
        }
        string[] cut = new string[2];
        for (int i = 0; i < _tips.Count; i++)
        {
            Tips.Add(i, _tips[i]);
        }
        for (int i = 0; i < _tips.Count; i++)
        {
            print(_tips[i]);
            cut = _tips[i].Split('-');
            name.Add(cut[0]);
            maindata.Add(cut[1]);
        }
        //for (int i = 0; i < Tips.Count; i++)
        //{
        //	print(Tips[i]);
        //}
        sr.Close();
    }

    public void SavingTips()
    {
        StreamWriter sw = new StreamWriter(@"Assets\Scripts\Tips\SavedTips.txt", append: true); //,append: true)
        try
        {
            print(datas[c]);
            sw.WriteLine(datas[c]);
            c++;
            sw.Close();
        }
        catch (IOException)
        {
            Debug.LogError($"Error saving the tips!");
            throw;
        }
    }

    public void LoadSaved()
    {
        List<string> saved_tips = new List<string>();
        StreamReader sr = new StreamReader(@"Assets\Scripts\Tips\SavedTips.txt");
        try
        {
            string sor;
            do
            {
                sor = sr.ReadLine();
                if (sor == null) break;
                saved_tips.Add(sor);

            } while (sor != null);
        }
        catch (IOException)
        {
            Debug.LogError($"Error loading in the tips!");
            throw;
        }
        string[] cut = new string[2];
        for (int i = 0; i < saved_tips.Count; i++)
        {
            cut = saved_tips[i].Split(' ');
            print(cut[0]);
            Saved_ID.Add(int.Parse(cut[0]));
        }
        sr.Close();
    }

    public int _key = 0;
    public int test = 0;
    public void GetRandomTip()
    {
         _key = UnityEngine.Random.Range(0, Tips.Count);
        if (test == _key)
        {
            do
            {
                _key = UnityEngine.Random.Range(0, Tips.Count);
            } while (test != _key);
        }     
        test = _key;
        rolledtip.data = maindata[_key];
        rolledtip.name = name[_key];
        rolledtip.lastdate = System.DateTime.Now.ToString();
        rolledtip.datum = DateTime.Now;
        datas.Add(_key + " " + rolledtip.name + " " + rolledtip.data + " " + rolledtip.lastdate);
        tipmanager.LoadDailyTip(_key, rolledtip.name, rolledtip.data);
    }

    
    void Start()
    {
        tipmanager = FindObjectOfType<TipManager>();
        TipLoad();
        GetRandomTip();
        SavingTips();
        LoadUserSave();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetRandomTip();
            SavingTips();
        }
    }

    public void LoadUserSave()
    {
        for (int i = 0; i < Saved_ID.Count; i++)
        {
            int a = PlayerPrefs.GetInt(Saved_ID[i] + ".tipkey", -1);
            if (a > -1 || a < Saved_ID.Count)
            {
                tipmanager.AddSavedTip(Saved_ID[a], name[i]);
            }
        }
    }

    public void RemoteCall_SaveButtonPressed()
    {
        // Ide rakj mindent amit akkor akarsz mikor r�kattintanak a tipp ment�s gombra
        //PlayerPrefs.SetInt(Saved_ID[_key] + ".tipkey", _key); ebben szar az indexeles
        // ITt a null�t meg a "Test"-et cser�ld ki a mai tipp adataira, ezt csak az�rt csin�ltam hogy tudjam tesztelni hogy m�k�dnek e egy�b dolgok
        tipmanager.AddSavedTip(_key, rolledtip.name);
    }

    public void DeleteTipButtonPressed(int _id)
    {
        // Vedd ki a mentettek k�z�l az _id-val rendelkez� tippet
        PlayerPrefs.DeleteKey(_id + ".tipkey");
    }

}
