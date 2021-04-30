using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AppManager : MonoBehaviour
{
    [SerializeField] private GameObject introductionPanel;
    [SerializeField] private GameObject languagePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject createGoalPanel;
    [Space]
    [Space]
    [SerializeField] private Sprite[] symbols_e;
     private static Sprite[] symbols;

    public enum Languages { English, Magyar, Deutsch, ENUM_END };

    // all static events should be here

    public static event Action<Languages> OnLanguageChanged;

    public static event Action<int> OnSubmenuButtonPressed;
    public static event Action<int> OnSubmenuChangedViaScrolling;
    public static event Action OnNewGoalAdded; // might not be needed

    /* Layer app index
     * 0 - Language screen
     * 1 - Intro screen
     * 2 - Main menu
     * 21 - Create goal screen
     * 3 - Settings
     * 
     */


    private void Awake()
    {
        OnNewGoalAdded += NewGoalAddedCallback;

        symbols = symbols_e;
    }

    private int GetAppLayer()
    {
        if(languagePanel.activeSelf)
        {
            return 0;
        }

        if(introductionPanel.activeSelf)
        {
            return 1;
        }

        if(mainMenuPanel.activeSelf)
        {
            return 2;
        }

        if(createGoalPanel.activeSelf)
        {
            return 21;
        }

        return -1;
    }

    private void SetAppLayer(int _layer)
    {
        if(_layer == 0)
        {
            introductionPanel.SetActive(false);
            languagePanel.SetActive(true);
        }

        if(_layer == 1)
        {
            // do nothing, this is the intro screen you shouldn't come back here
        }

        if(_layer == 2)
        {
            introductionPanel.SetActive(false);
            createGoalPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }

        if(_layer == 21)
        {
            mainMenuPanel.SetActive(false);
            createGoalPanel.SetActive(true);
        }

        Debug.LogError($"There is no such layer index: {_layer}");
    }

    

    private void Start()
    {
        // awakebe vannak a feliratkozások, ezért csak a startban kapcsoljunk ki mindent

        introductionPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        createGoalPanel.SetActive(false);


        // set language if already saved one

        // decide starting page
    }

    private void Update()
    {
        if(!Input.GetKeyDown(KeyCode.Escape)) { return; }
        
        switch(GetAppLayer())
        {
            case 0:
                Application.Quit();
                break;
            case 1:
                SetAppLayer(0);
                break;
            case 2:
                Application.Quit();
                break;
            case 21:
                SetAppLayer(2);
                break;
        }

      

        
        
    }

    public static void SetLanguage(Languages _language)
    {
        // save language if not saved
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

    public static void NewGoalAdded()
    {
        OnNewGoalAdded?.Invoke();
    }








    public static Sprite GetSpriteFromId(int _id)
    {
        if(_id > 0 && _id < symbols.Length - 1)
        {
            return symbols[_id];
        }

        Debug.LogError("Symbol index out of bounds");
        return null;
    }

    

    private void NewGoalAddedCallback()
    {
        createGoalPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
