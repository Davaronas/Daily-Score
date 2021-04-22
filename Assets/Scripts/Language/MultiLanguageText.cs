using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]

public class MultiLanguageText : MonoBehaviour
{
    

     [SerializeField]
    private string[] languages = new string[(int)AppManager.Languages.ENUM_END];

    private TMP_Text text = null;



 
   
  
    void Awake()
    {
        text = GetComponent<TMP_Text>();

        AppManager.OnLanguageChanged += LanguageChanged;
    }

    private void OnDestroy()
    {
        AppManager.OnLanguageChanged -= LanguageChanged;
    }


    void Update()
    {
        
    }

    private void LanguageChanged(AppManager.Languages _language)
    {
        text.text = languages[(int)_language];
    }


}
