using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TipDictionary : BehaviourButton
{   
    [System.Serializable]
    public struct RolledTips
    {
        public string name;
        public string data;
        public string lastdate;
    }
    public List<string> datas = new List<string>();
    public List<string> name = new List<string>();
    public List<string> maindata = new List<string>();
    IDictionary<int, string> Tips = new Dictionary<int, string>();
    public bool first_run = true;
    public bool first_run_roll = true;
    public int c = 0;
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
    }
    List<int> Saved_ID = new List<int>();
    public void LoadSaved()
    {
        List<string> saved_tips = new List<string>();
        StreamReader sr = new StreamReader(Path.Combine(Application.persistentDataPath, "SavedTips.txt"));
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
            Saved_ID.Add(int.Parse(cut[0]));
        }
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

    public RolledTips FirstTipRoll()
    {
        RolledTips rolledtip;
        int go = 0;
        rolledtip.data = maindata[go];
        rolledtip.name = name[go];
        rolledtip.lastdate = System.DateTime.Now.ToString();
        datas.Add(go + " " + rolledtip.name + " " + rolledtip.data + " " + rolledtip.lastdate);
        go++;
        return rolledtip;
    }
    public int _key = 0;
    public RolledTips GetRandomTip()
    {
        RolledTips rolledtip;
        _key = Random.Range(Saved_ID.Count-2, Saved_ID.Count); //- ahány végsõ elem a tûréshatár, tippek nagyságátol fuggõen állítsuk.         
        rolledtip.data = maindata[_key];
        rolledtip.name = name[_key];
        rolledtip.lastdate = System.DateTime.Now.ToString();
        datas.Add(_key + " " + rolledtip.name + " " + rolledtip.data + " " + rolledtip.lastdate);      
        return rolledtip;
    }

    public void SavedTips()
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

    public void UserSave()
    {
        PlayerPrefs.SetInt(Saved_ID[_key] + ".tipkey", _key);
    }

    public List<string> LoadUserSave()
    {
        List<string> SavedData = new List<string>();
        for (int i = 0; i < Saved_ID.Count; i++)
        {
            int a = PlayerPrefs.GetInt(Saved_ID[i] + ".tipkey", -1);
            if (a > -1 || a < Saved_ID.Count) SavedData.Add(maindata[a]);
        }
        return SavedData;
    }

    protected override void OnTouch()
    {
        if (first_run == true)
        {
            TipLoad();
            LoadSaved();
            first_run = false;
        }
        if (first_run_roll == true)
        {
            do
            {
                FirstTipRoll();
                first_run_roll = false;
            } while (Saved_ID.Count != Tips.Count);
        }
        else GetRandomTip();
        SavedTips();
    }
}
