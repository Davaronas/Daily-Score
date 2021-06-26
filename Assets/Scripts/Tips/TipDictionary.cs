using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class TipDictionary : MonoBehaviour
{   
    [System.Serializable]
    public struct RolledTips
    {
        public string name;
        public string data;
        public string lastdate;
        public DateTime datum;
    }
    public List<string> datas = new List<string>();
    public List<string> name = new List<string>();
    public List<string> maindata = new List<string>();
    IDictionary<int, string> Tips = new Dictionary<int, string>();
    public bool first_run = true;
    public bool first_run_roll = true;
    public int c = 0;
    RolledTips rolledtip;
    
    private TipManager tipManager;
    public bool empty = true;
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
            Tips.Add(i,_tips[i]);
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
    List<int> Saved_ID = new List<int>();
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

    public string FindTip(int _key)
    {
        
        if (Tips.ContainsKey(_key))
        {
            
            return Tips[_key];
        }
        else
        {
            Debug.LogError($"Key not found in {Tips} dictionary: {_key}");
            return null;
        }    
    }

    public int _key = 0;
    public void GetRandomTip()
    {       
        _key = UnityEngine.Random.Range(0,3); //- ahány végsõ elem a tûréshatár, tippek nagyságátol fuggõen állítsuk.         
        rolledtip.data = maindata[_key];
        rolledtip.name = name[_key];
        rolledtip.lastdate = System.DateTime.Now.ToString();
        rolledtip.datum = DateTime.Now;
        datas.Add(_key + " " + rolledtip.name + " " + rolledtip.data + " " + rolledtip.lastdate);
        tipmanager.LoadDailyTip(_key, rolledtip.name, rolledtip.data);
    }

    public void SavingTips()
    {
        StreamWriter sw = new StreamWriter(@"Assets\Scripts\Tips\SavedTips.txt", append: true); //,append: true)
        try
        {
            //print(datas[c]);
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

    public void UserSave()
    {
        PlayerPrefs.SetInt(Saved_ID[_key] + ".tipkey", _key);
    }

    public void UserDelete(int _id)
    {
        PlayerPrefs.DeleteKey(_id + ".tipkey");
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

    TipManager tipmanager = null;
    // AddSavedTip(int _id, string _header)
    // RemoveSavedTip(int _id)
    // LoadDailyTip(int _id, string _header, string _content)
    public int counter = 0;
    void Start()
    {
        tipmanager = FindObjectOfType<TipManager>();       
        //TipLoad();
        //LoadSaved();
        //LoadUserSave();       
        GetRandomTip();
        //print("buzi vagyok");
        SavingTips();
               
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {               
            
        }
    }

    public void RemoteCall_SaveButtonPressed()
    {       
        // Ide rakj mindent amit akkor akarsz mikor rákattintanak a tipp mentés gombra
        UserSave();
        // ITt a nullát meg a "Test"-et cseréld ki a mai tipp adataira, ezt csak azért csináltam hogy tudjam tesztelni hogy mûködnek e egyéb dolgok
        tipManager.AddSavedTip(_key, rolledtip.name);
    }

    public void DeleteTipButtonPressed(int _id)
    {
        // Vedd ki a mentettek közül az _id-val rendelkezõ tippet
        UserDelete(_id);
    }
}
