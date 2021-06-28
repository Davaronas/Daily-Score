using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

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
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/tips.txt");
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
        List<string> saved_tips = new List<string>();
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/SavedTips.txt");
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
        datas.Add(rolledtip.ID + " " + rolledtip.name + " " + rolledtip.data + " " + rolledtip.lastdate + " " + rolledtip.saved);
        tipmanager.LoadDailyTip(rolledtip.ID, rolledtip.name, rolledtip.data);
    }
    
    void Start()
    {
        tipmanager = FindObjectOfType<TipManager>();        
        TipLoad();
        LoadUserSave();
        LoadSaved();
        GetRandomTip();
        SavingTips();         
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
        // Ide rakj mindent amit akkor akarsz mikor rákattintanak a tipp mentés gombra
        rolledtip.saved = true;
        Loaded.Add(rolledtip.ID);
        //SaveData();
        // ITt a nullát meg a "Test"-et cseréld ki a mai tipp adataira, ezt csak azért csináltam hogy tudjam tesztelni hogy mûködnek e egyéb dolgok
        tipmanager.AddSavedTip(_key, rolledtip.name);
    }
    public void DeleteTipButtonPressed(int _id)
    {
        Loaded.Remove(Loaded[_id]);
        // Vedd ki a mentettek közül az _id-val rendelkezõ tippet
        //PlayerPrefs.DeleteKey(_id + ".id");
    }
    public void OnApplicationQuit()
    {
        //print("lefut");
        SaveData();
    }
}
