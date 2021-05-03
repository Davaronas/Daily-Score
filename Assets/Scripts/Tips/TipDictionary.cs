using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipDictionary : MonoBehaviour
{
    IDictionary<int, string> Tips = new Dictionary<int, string>();

    public void TipLoad(string[] _tips)
    {
        for (int i = 0; i < _tips.Length; i++)
        {
            Tips.Add(i,_tips[i]);
        }
    }

   

    public string FindTip(int _key)
    {
        if (Tips.ContainsKey(_key))
        {
            return Tips[_key];
        }

        Debug.LogError($"Key not found in {Tips} dictionary: {_key}");
        return null;
    }



    public string GetRandomTip()
    {
        if(Tips.Count < 1) { Debug.LogWarning($"Dictionary is empty: {Tips}"); return null; }

        int _key = Random.Range(0, Tips.Count);
        return Tips[_key];
    }

}
