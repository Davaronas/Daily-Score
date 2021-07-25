using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipHandler : MonoBehaviour
{
   public struct Tip
   {
        public int id;
        public string header;
        public string content;
        public bool isSaved;
   }

    private void Awake()
    {
        AppManager.OnLanguageChanged += LanguageChanged;
    }

    private void Start()
    {
        

        TextAsset textAsset = (TextAsset)Resources.Load("tips_hu");
        if(textAsset == null)
        {
            Debug.LogError("There are no tips");
        }

        string[] _lines = textAsset.ToString().Split('\n');
        //    print(_lines.Length);

        foreach (string _line in _lines)
        {
            string[] _parts = _line.Split('-');
        }
    }

    private void LanguageChanged(AppManager.Languages _l)
    {

    }
}
