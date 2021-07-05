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
            int.TryParse(cut[0], out int _p);
            Saved_ID.Add(_p);  
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
    public bool second = false;
    public List<string> itwas = new List<string>();
    public void GetRandomTip()
    {
        if (second == true)
        {
            itwas.Add(rolledtip.ID.ToString());
            itwas.Add(rolledtip.name);
            itwas.Add(rolledtip.data);
        }
        //rolledtip = new RolledTips();
         _key = UnityEngine.Random.Range(0, maindata.Count);
        if (rolledtip.ID == _key)
        {
            do
            {
                _key = UnityEngine.Random.Range(0, maindata.Count);
            } while (rolledtip.ID == _key);
        }
        rolledtip.ID = _key;
        rolledtip.data = maindata[_key];
        rolledtip.name = name[_key];
        rolledtip.lastdate = System.DateTime.Now.ToString();
        rolledtip.datum = DateTime.Now;
        rolledtip.saved = false;
        datas.Add(rolledtip.ID + "-" + rolledtip.name + "-" + rolledtip.data + "-" + rolledtip.lastdate + "-" + rolledtip.saved);
    }

    void Start()
    {
        tipmanager = FindObjectOfType<TipManager>();
       // print(Application.persistentDataPath);
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            ResourceLoad();
            GetRandomTip();
            tipmanager.LoadDailyTip(rolledtip.ID, rolledtip.name, rolledtip.data);
            SavingTips();
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
        }
        else
        {
            // print("ciganyvagyok");
            ResourceLoad();
            LoadSaved();
            LoadUserSave();
            if (System.DateTime.Today != AppManager.lastLogin.Date)
            {
                GetRandomTip();
                tipmanager.LoadDailyTip(rolledtip.ID, rolledtip.name, rolledtip.data);
                SavingTips();
            }
            else
            {
                if (second == true)
                {
                    tipmanager.LoadDailyTip(Saved_ID[Saved_ID.Count-1], name[Saved_ID.Count-1], maindata[Saved_ID.Count-1]);
                }
                tipmanager.LoadDailyTip(Saved_ID[Saved_ID.Count], name[Saved_ID.Count], maindata[Saved_ID.Count]);
            }
        }       
    }

    public void ResourceLoad()
    {
        //  int i = 0;

        // print(textAsset);
        //reader = new StringReader(textAsset.text);
        //string path = "Assets/Resources/tips.txt";
        // foreach (var line in File.ReadLines(path))
        /*
        foreach(var line in File.ReadLines(textAsset))
         {
             //print("mukszik");
             string[] parts = line.Split('-');
           //  print(parts[0] + parts[1]);
             name.Add(parts[0]);
             maindata.Add(parts[1]);
             i++;
         }           
        */

        TextAsset textAsset = (TextAsset)Resources.Load("tips");
        string[] _lines = textAsset.ToString().Split('\n');
        print(_lines.Length);

        foreach (string _line in _lines)
        {
            string[] _parts = _line.Split('-');
            name.Add(_parts[0]);
            maindata.Add(_parts[1]);
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
            //print(Loaded[i]);
            tipmanager.AddSavedTip(Loaded[i], name[Loaded[i]]);
        }      
    }

    public int AskForSecondTip(out string _header, out string _content)
    {
        second = true;
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENINGSECOND", 1) == 1)
        {
            GetRandomTip();
            SavingTips();
            _header = rolledtip.name; // a c�me a tipnek
            _content = rolledtip.data; // a tartalma a tipnek
            PlayerPrefs.SetInt("FIRSTTIMEOPENINGSECOND", 0);
            return rolledtip.ID; // tip id ide
        }
        else
        {
            if (System.DateTime.Today != AppManager.lastLogin.Date)
            {
                GetRandomTip();
                SavingTips();
                _header = rolledtip.name; // a c�me a tipnek
                _content = rolledtip.data; // a tartalma a tipnek
                return rolledtip.ID; // tip id ide
            }
            else
            {
                //tipmanager.UnlockSecondTip(); tip feloldas
                GetRandomTip();
                SavingTips();
                _header = name[Saved_ID.Count]; // a c�me a tipnek
                _content = maindata[Saved_ID.Count]; // a tartalma a tipnek
                return Saved_ID[Saved_ID.Count]; // tip id ide
            }
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
                //print(Loaded[c]);
                //print(Loaded.Count);
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
        // Ide rakj mindent amit akkor akarsz mikor r�kattintanak a tipp ment�s gombra
        rolledtip.saved = true;
        if (second == true)
        {
            tipmanager.AddSavedTip(int.Parse(itwas[0]), itwas[1]);
            Loaded.Add(int.Parse(itwas[0]));
        }
        else
        {
            Loaded.Add(rolledtip.ID);
            tipmanager.AddSavedTip(_key, rolledtip.name);    
        }
        SaveData();
        
    }

    public void RemoteCall_SecondTip_SaveButtonPressed()
    {
        print(rolledtip.ID);
        rolledtip.saved = true;
        Loaded.Add(rolledtip.ID);
        tipmanager.AddSavedTip(_key, rolledtip.name);
        SaveData();
    }

   

    private void OnNewDayStartedDuringRuntime()
    {

    }

    public void DeleteTipButtonPressed(int _id)
    {
        
        //print(Loaded[_id]);
        for (int i = 0; i < Loaded.Count; i++)
        {
            if (Loaded[i] == _id)
            {
                Loaded.Remove(Loaded[i]);
                print($"Found and deleted: {_id}");
            }
        }
       // tipmanager.SetSaveButtonState(_id);
        // Vedd ki a mentettek k�z�l az _id-val rendelkez� tippet
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
