using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AppManager : MonoBehaviour
{
    [SerializeField] private GameObject introductionPanel;
    [SerializeField] private GameObject languagePanel;
    [SerializeField] private GameObject mainMenuPanel;

    public enum Languages { English, Magyar, Deutsch, ENUM_END };


    public static Action<Languages> OnLanguageChanged;

    private void Start()
    {
        // set language if already saved one

        // decide starting page
    }

    public static void SetLanguage(Languages _language)
    {
        // save language if not saved
        // set all texts to the selected language
        if(_language == Languages.ENUM_END)
        {
            Debug.LogError($"Invalid language: {_language}");
            return;
        }


        OnLanguageChanged?.Invoke(_language);


        Debug.Log($"Language set to {_language}");
    }

}
