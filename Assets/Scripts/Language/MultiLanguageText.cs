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

    private string textToDisplay = "";

   public MultiLanguageText()
    {
        AppManager.OnLanguageChanged += LanguageChanged;
    }

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = textToDisplay;
    }

    private void OnDestroy()
    {
        AppManager.OnLanguageChanged -= LanguageChanged;
    }

    private void LanguageChanged(AppManager.Languages _language)
    {
        textToDisplay = languages[(int)_language];


        // ha az Awake hamarabb futna le mint LanguageChanged
        if(text != null)
        {
            text.text = textToDisplay;
        }
    }


}
