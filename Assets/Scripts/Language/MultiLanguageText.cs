using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
[DisallowMultipleComponent]
public class MultiLanguageText : MonoBehaviour
{
    

    [SerializeField]
    private string[] languages = new string[(int)AppManager.Languages.ENUM_END];

    private TMP_Text text = null;

    private string textToDisplay = "";


    private void Awake()
    {
        text = GetComponent<TMP_Text>();

        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                text.text = languages[0];
                break;
            case AppManager.Languages.Magyar:
                text.text = languages[1];
                break;
            case AppManager.Languages.Deutsch:
                text.text = languages[2];
                break;
            default:
                text.text = "";
                break;
        }

        


        AppManager.OnLanguageChanged += LanguageChanged;
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
