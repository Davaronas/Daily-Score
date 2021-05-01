using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class AppManager : MonoBehaviour
{
    [SerializeField] private GameObject introductionPanel;
    [SerializeField] private GameObject languagePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject createGoalPanel;
    [SerializeField] private GameObject goalPanel;
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

    public static event Action<Goal> OnGoalOpened;

    /* Layer app index
     * 0 - Language screen
     * 1 - Intro screen
     * 2 - Main menu
     *  211 - Create goal screen
     *  212 - Goal screen
     *      2121 - Create task screen
     * 3 - Settings
     */


    private void Awake()
    {
        OnNewGoalAdded += NewGoalAddedCallback;
        OnGoalOpened += OnGoalOpenedCallback;

        symbols = symbols_e;
    }

    private void OnDestroy()
    {
        OnNewGoalAdded -= NewGoalAddedCallback;
        OnGoalOpened -= OnGoalOpenedCallback;
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
            return 211;
        }

        if(goalPanel.activeSelf)
        {
            return 212;
        }

        Debug.LogError($"App layer couldn't be determined, all panels could be disabled ?");
        return -1;
    }

    private void SetAppLayer(int _layer)
    {
        if(_layer == 0)
        {
            introductionPanel.SetActive(false);
            languagePanel.SetActive(true);

            return;
        }

        if(_layer == 1)
        {
            // do nothing, this is the intro screen you shouldn't come back here

            return;
        }

        if(_layer == 2)
        {
            introductionPanel.SetActive(false);
            createGoalPanel.SetActive(false);
            goalPanel.SetActive(false);
            mainMenuPanel.SetActive(true);

            return;
        }

        if(_layer == 211)
        {
            mainMenuPanel.SetActive(false);
            createGoalPanel.SetActive(true);

            return;
        }

        if(_layer == 212)
        {
            mainMenuPanel.SetActive(false);
            goalPanel.SetActive(true);

            return;
        }

        Debug.LogError($"There is no such layer index: {_layer}");
    }

    

    private void Start()
    {
        // awakebe vannak a feliratkozások, ezért csak a startban kapcsoljunk ki mindent

        introductionPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        createGoalPanel.SetActive(false);
        goalPanel.SetActive(false);


        // set language if already saved one

        // decide starting page
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) { print("A"); }

        if (!Application.isEditor)
        {
            if (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.Return)) { return; }
        }
        else // debug in editor
        { 
            if (!Input.GetButtonDown("Jump")) { return; }
        }

        print(GetAppLayer());
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
            case 211:
                SetAppLayer(2);
                break;
            case 212:
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

  


   





    public static Sprite GetSpriteFromId(int _id)
    {
        if(_id >= 0 && _id < symbols.Length)
        {
            return symbols[_id];
        }

        Debug.LogError("Symbol index out of bounds");
        return null;
    }



    public static void NewGoalAdded()
    {
        OnNewGoalAdded?.Invoke();
    }

    private void NewGoalAddedCallback()
    {
        SetAppLayer(2);
    }


    public static void GoalOpened(Goal _goal)
    {
        OnGoalOpened?.Invoke(_goal);
    }

    private void OnGoalOpenedCallback(Goal _goal)
    {
        SetAppLayer(212);
    }
}
