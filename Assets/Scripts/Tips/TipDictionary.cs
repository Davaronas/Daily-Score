using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TipDictionary : BehaviourButton
{

    IDictionary<int, string> Tips = new Dictionary<int, string>();

    public void TipLoad()
    {
        List<string> _tips = new List<string>();
        StreamReader sr = new StreamReader("tips.txt");
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
        for (int i = 0; i < _tips.Count; i++)
        {
            Tips.Add(i,_tips[i]);
        }
        for (int i = 0; i < Tips.Count; i++)
        {
            print(Tips[i]);
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


    public string GetRandomTip()
    {
        if(Tips.Count < 1) { Debug.LogWarning($"Dictionary is empty: {Tips}"); return null; }

        int _key = Random.Range(0, Tips.Count);
        print(Tips[_key]);

        return Tips[_key];
    }

    protected override void OnTouch()
    {
        TipLoad();
        GetRandomTip();
    }
}
