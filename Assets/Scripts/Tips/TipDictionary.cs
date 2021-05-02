using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipDictionary : MonoBehaviour
{
    public int darab;
    public string tips;

    IDictionary<string, int> Tips = new Dictionary<string, int>();

    public void tipLoad(int darab, string tips)
    {
        for (int i = 0; i < darab.Length; i++)
        {
            Tips.Add(tips[i],i);
        }
    }

    public void findTip(int darab)
    {
        int find = Random.range(0, darab);
        return (Tips[find]);
    }

}
