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
    public bool gold;
    private void Awake()
    {
        tipManager = FindObjectOfType<TipManager>();
        AppManager.OnNewDayStartedDuringRuntime += OnNewDayStartedDuringRuntime;
        gold = AppManager.isGold;
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
        //print(cut[1]);
        //print(last_tip.Length);
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
        print(Application.persistentDataPath);
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            ResourceLoad();
            GetRandomTip();
            SavingTips();
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
        }
        else
        {
            print("ciganyvagyok");
            ResourceLoad();
            LoadUserSave();
            LoadSaved();
            if (System.DateTime.Today != AppManager.lastLogin)
            {
                GetRandomTip();
                SavingTips();
            }
            else
            {
                tipmanager.LoadDailyTip(Saved_ID[0], last_tip[0], last_tip[1]);
            }
        }       
    }

    public void ResourceLoad()
    {
        int i = 0;
        print("mükszik");
        
        TextAsset textAsset = (TextAsset)Resources.Load("tips",typeof(TextAsset));
        //reader = new StringReader(textAsset.text);
        string path = "Assets/Resources/tips.txt";
        foreach (var line in File.ReadLines(path))
        {
            print("mukszik");
            string[] parts = line.Split('-');
            print(parts[0] + parts[1]);
            name.Add(parts[0]);
            maindata.Add(parts[1]);
            i++;
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
            while (c != Loaded.Count)
            {
                print(Loaded[c]);
                print(Loaded.Count);
                sw.WriteLine(Loaded[c]);
                c++;
            } 
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
        // Ide rakj mindent amit akkor akarsz mikor rákattintanak a tipp mentés gombra
        rolledtip.saved = true;
        print(rolledtip.ID);
        Loaded.Add(rolledtip.ID);
        
        //SaveData();
        // ITt a nullát meg a "Test"-et cseréld ki a mai tipp adataira, ezt csak azért csináltam hogy tudjam tesztelni hogy mûködnek e egyéb dolgok
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
        print(_id);
        print(Loaded[0]);
        Loaded.Remove(Loaded[_id]);
        tipmanager.EnableSaveButton(_id);
        // Vedd ki a mentettek közül az _id-val rendelkezõ tippet
    }
    public void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnDestroy()
    {
        AppManager.OnNewDayStartedDuringRuntime -= OnNewDayStartedDuringRuntime;
    }
}
