using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;

public class TipMain : MonoBehaviour
{
    [System.Serializable]
    public struct RolledTips
    {
        public int ID;
        public string name;
        public string data;
        public string lastdate;
        public DateTime datum;
        public bool saved;
    }
    RolledTips rolledtip;
    public List<RolledTips> TipUserSave = new List<RolledTips>();
    public List<string> datas = new List<string>();
    public List<string> name = new List<string>();
    public List<string> maindata = new List<string>();
    public List<int> Saved_ID = new List<int>();
    public string[] last_tip;
    IDictionary<int, string> Tips = new Dictionary<int, string>();
    public int c = 0;
    private TipManager tipManager;
    TipManager tipmanager = null;
    private void Awake()
    {
        tipManager = FindObjectOfType<TipManager>();
        AppManager.OnNewDayStartedDuringRuntime += OnNewDayStartedDuringRuntime;
    }

    public void TipLoad()
    {
        List<string> _tips = new List<string>();
        string path = Application.persistentDataPath + "/tips.txt";
        if(!File.Exists(path)) { Debug.LogError("tips.txt doesn't exist"); return; }

        StreamReader sr = new StreamReader(path);
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
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/SavedTips.txt", append: true); //,append: true)
        try
        {
            do
            {
                sw.WriteLine(datas[c]);
                c++;
            } while (c != datas.Count);
            
            
        }
        catch (IOException)
        {
            Debug.LogError($"Error saving the tips!");
            throw;
        }
        sw.Close();
    }

    public void LoadSaved()
    {
        last_tip = new string[4];
        List<string> saved_tips = new List<string>();
        string path = Application.persistentDataPath + "/SavedTips.txt";
        if (!File.Exists(path)) { Debug.LogError("SavedTips.txt doesn't exist"); return; }

        StreamReader sr = new StreamReader(path);
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
        string[] cut = new string[4];
        for (int i = 0; i < saved_tips.Count; i++)
        {
            cut = saved_tips[i].Split('-');
            //print(cut[1]);
            Saved_ID.Add(int.Parse(cut[0]));  
        }
        cut = saved_tips[saved_tips.Count-1].Split('-');
        print(cut[1]);
        print(last_tip.Length);
        last_tip[0] = cut[1];
        last_tip[1] = cut[2];
        test = Saved_ID[Saved_ID.Count-1];
        sr.Close();
    }

    public int _key = 0;
    public int test = 0;
    public void GetRandomTip()
    {      
        rolledtip = new RolledTips();
         _key = UnityEngine.Random.Range(0, Tips.Count);
        if (test == _key)
        {
            do
            {
                _key = UnityEngine.Random.Range(0, Tips.Count);
            } while (test != _key);
        }
        test = _key;
        rolledtip.ID = _key;
        rolledtip.data = maindata[_key];
        rolledtip.name = name[_key];
        rolledtip.lastdate = System.DateTime.Now.ToString();
        rolledtip.datum = DateTime.Now;
        rolledtip.saved = false;
        datas.Add(rolledtip.ID + "-" + rolledtip.name + "-" + rolledtip.data + "-" + rolledtip.lastdate + "-" + rolledtip.saved);
        tipmanager.LoadDailyTip(rolledtip.ID, rolledtip.name, rolledtip.data);
    }
    
    void Start()
    {
        tipmanager = FindObjectOfType<TipManager>();        
        TipLoad();
        LoadUserSave();
        LoadSaved();
        if (System.DateTime.Today != AppManager.lastLogin)
        {
            GetRandomTip();
            //PlayerPrefs.SetString("LastLogin", System.DateTime.Today.ToString());
            SavingTips();
        }
        else
        {
            tipmanager.LoadDailyTip(Saved_ID[0],last_tip[0],last_tip[1]);
        }
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
        LoadData();
        for (int i = 0; i < Loaded.Count; i++)
        {
            tipmanager.AddSavedTip(Loaded[i], name[i]);
        }      
    }

    public void SaveData()
    {
        c = 0;
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/binary.txt"); //,append: true)
        try
        {
            do
            {
                sw.WriteLine(Loaded[c]);
                c++;
            } while (c != Loaded.Count);
        }
        catch (IOException)
        {
            Debug.LogError($"Error saving the tips!");
            throw;
        }
        sw.Close();
    }

    public List<int> Loaded = new List<int>();

    public void LoadData()
    {
        
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/binary.txt");
        try
        {
            string sor;
            do
            {
                sor = sr.ReadLine();
                if (sor == null) break;
                Loaded.Add(int.Parse(sor));

            } while (sor != null) ;
        }
        catch (IOException)
        {
            Debug.LogError($"Error loading in the tips!");
            throw;
        }
        sr.Close();
    }

    public void RemoteCall_SaveButtonPressed()
    {
        // Ide rakj mindent amit akkor akarsz mikor r�kattintanak a tipp ment�s gombra
        rolledtip.saved = true;
        Loaded.Add(rolledtip.ID);
        
        SaveData();
        // ITt a null�t meg a "Test"-et cser�ld ki a mai tipp adataira, ezt csak az�rt csin�ltam hogy tudjam tesztelni hogy m�k�dnek e egy�b dolgok
        tipmanager.AddSavedTip(_key, rolledtip.name);
        tipmanager.DisableSaveButton();
    }

    public void RemoteCall_WatchAdButtonPressed()
    {

    }

    private void OnNewDayStartedDuringRuntime()
    {

    }

    public void DeleteTipButtonPressed(int _id)
    {
        Loaded.Remove(Loaded[_id]);
        // Vedd ki a mentettek k�z�l az _id-val rendelkez� tippet
        //PlayerPrefs.DeleteKey(_id + ".id");
    }
    public void OnApplicationQuit()
    {
        //print("lefut");
        
        if (Loaded != null)
        SaveData();
    }

    private void OnDestroy()
    {
        AppManager.OnNewDayStartedDuringRuntime -= OnNewDayStartedDuringRuntime;
    }
}
