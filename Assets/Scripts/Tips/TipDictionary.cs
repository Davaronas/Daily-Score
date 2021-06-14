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
    List<string> datas = new List<string>();
    List<string> name = new List<string>();
    List<string> maindata = new List<string>();
    IDictionary<int, string> Tips = new Dictionary<int, string>();
    public bool first_run = true;
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

    public RolledTips GetRandomTip()
    {
        RolledTips rolledtip;
        int a = 0;
        int _key = Random.Range(0, Tips.Count);
        //print(mainlist.data[_key]);            
        rolledtip.data = maindata[_key];
        rolledtip.name = name[_key];
        rolledtip.lastdate = System.DateTime.Now.ToString();
        datas.Add(_key + " " + rolledtip.name + " " + rolledtip.data + " " + rolledtip.lastdate);
        //print(datas[a]);
        a++;
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

    protected override void OnTouch()
    {
        if (first_run == true)
        {
            TipLoad();
            first_run = false;
        }
        GetRandomTip();
        SavedTips();
    }
}
