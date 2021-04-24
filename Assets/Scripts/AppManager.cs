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

    // all static events should be here

    public static event Action<Languages> OnLanguageChanged;

    public static event Action<int> OnSubmenuButtonPressed;
    public static event Action<int> OnSubmenuChangedViaScrolling;


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

        if(OnLanguageChanged != null)
        OnLanguageChanged?.Invoke(_language);
    }

    public static void SubmenuButtonPressed(int _buttonId)
    {
        OnSubmenuButtonPressed?.Invoke(_buttonId);
    }

    public static void SubmenuChangedViaScrolling(int _buttonId)
    {
        OnSubmenuChangedViaScrolling?.Invoke(_buttonId);
    }
}
