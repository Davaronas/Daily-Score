using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



public enum ColorType { Simple, Gradient, FourCornerGradient, ENUM_END };

[System.Serializable]
public struct GoalColor
{
    public GoalColor(Color32 _c32)
    {
        r = _c32.r;
        g = _c32.g;
        b = _c32.b;
        a = _c32.a;
    }

    public GoalColor(int _r, int _g, int _b, int _a)
    {
        r = _r;
        g = _g;
        b = _b;
        a = _a;
    }

    public GoalColor(GoalColor _gc)
    {
        r = _gc.r;
        g = _gc.g;
        b = _gc.b;
        a = _gc.a;
    }

    public int r;
    public int g;
    public int b;
    public int a;

    public static implicit operator Color32(GoalColor _gc) => new Color32((byte)_gc.r, (byte)_gc.g, (byte)_gc.b, (byte)_gc.a);
    public static implicit operator GoalColor(Color32 _c32) => new GoalColor(_c32.r,_c32.g,_c32.b,_c32.a);
    public static implicit operator Color(GoalColor _gc) => new Color(_gc.r, _gc.g, _gc.b,_gc.a);

    public static GoalColor operator +(GoalColor _gc1 , GoalColor _gc2)
    {
        return new GoalColor(Mathf.RoundToInt(((float)_gc1.r + _gc2.r) / 2),
            Mathf.RoundToInt(((float)_gc1.g + _gc2.g) / 2),
            Mathf.RoundToInt(((float)_gc1.b + _gc2.b) / 2),
            Mathf.RoundToInt(((float)_gc1.a + _gc2.a) / 2));
    }

    public override string ToString()
    {
        return "RGBA(" + r + ", " + g + ", " + b + ", " + a + ")";
    }

}

[System.Serializable]
public struct GoalData
{

    
    
    public GoalData(string _name, Color32 _color,int _spriteId, int _max = 0,  int _current = 0)
    {
       
        colorType = ColorType.Simple;
        color = new GoalColor[1];

        color[0] = _color;



        name = _name;
        spriteId = _spriteId;
        max = _max;
        current = _current;
        tasks = new List<TaskData>();
    }

    public GoalData(string _name, GoalColor[] _colors, int _spriteId, int _max = 0, int _current = 0)
    {


        if(_colors.Length == 2)
        {
            colorType = ColorType.Gradient;
            color = new GoalColor[2];

            color[0] = _colors[0];
            color[1] = _colors[1];
            
          
       }
       else if(_colors.Length == 4)
       {

           colorType = ColorType.FourCornerGradient;
           color = new GoalColor[4];

            color[0] = _colors[0];
            color[1] = _colors[1];
            color[2] = _colors[2];
            color[3] = _colors[3];
        }
       else
       {
           colorType = ColorType.Simple;
           color = new GoalColor[1];

           color[0] = _colors[0];

           Debug.LogWarning("The color array argument does not contain neither 2 nor 4 colors, only the first color will be used");
       }



       name = _name;
       spriteId = _spriteId;
       max = _max;
       current = _current;
       tasks = new List<TaskData>();
   }



   public string name;
   public GoalColor[] color;
   public ColorType colorType;
   public int spriteId;
   public int max;
   public int current;
   public  List<TaskData> tasks;
}


[System.Serializable]
public struct TaskData
{
   public TaskData(string _name, int _type, int _max, int _current = 0)
   {
       name = _name;
       type = _type;
       max = _max;
       current = _current;
   }

   public string name;
   public int type;
   public int max;
   public int current;
}


public class AppManager : MonoBehaviour
{
   [SerializeField] private GameObject introductionPanel;
   [SerializeField] private GameObject languagePanel;
   [SerializeField] private GameObject mainMenuPanel;
   [SerializeField] private GameObject createGoalPanel;
   [SerializeField] private GameObject goalPanel;
   [SerializeField] private GameObject createTaskPanel;
   [SerializeField] private GameObject settingsPanel;
   [Space]
   [Space]
   [SerializeField] private Sprite[] symbols_e;
   private static Sprite[] symbols;




   private GoalManager goalManager = null;
   private TaskManager taskManager = null;

   public enum Languages { English, Magyar, Deutsch, ENUM_END };
   public static Languages currentLanguage { get; private set; } = 0;
   public enum TaskType { Maximum, Minimum, Boolean, Optimum, Interval, ENUM_END };



   public enum TaskMetricType {Pieces, Minutes, Kilometres, Mile, Grams, Pound, Other, ENUM_END };

   // all static events should be here

   public static event Action<Languages> OnLanguageChanged;

   public static event Action<int> OnSubmenuButtonPressed;
   public static event Action<int> OnSubmenuChangedViaScrolling;
   public static event Action OnNewGoalAdded; // might not be needed
   public static event Action OnNewTaskAdded;

   public static event Action<Goal> OnGoalOpened;

   public static event Action OnAppLayerChangedToMainMenu;







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
        OnNewTaskAdded += NewTaskAddedCallback;
        OnGoalOpened += OnGoalOpenedCallback;

        goalManager = FindObjectOfType<GoalManager>();
        taskManager = FindObjectOfType<TaskManager>();

       

        symbols = symbols_e;
    }

    private void OnDestroy()
    {
        OnNewGoalAdded -= NewGoalAddedCallback;
        OnNewTaskAdded -= NewTaskAddedCallback;
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

        if(createTaskPanel.activeSelf)
        {
            return 2121;
        }

        if(settingsPanel.activeSelf)
        {
            return 3;
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
            settingsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            

            AppLayerChangedToMainMenu();
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
           taskManager.DisplayTasks(goalManager.GetCurrentlySelectedGoal().GetTasks());

            mainMenuPanel.SetActive(false);
            createTaskPanel.SetActive(false);
            goalPanel.SetActive(true);

            return;
        }

        if(_layer == 2121)
        {
            

            goalPanel.SetActive(false);
            createTaskPanel.SetActive(true);

            return;
        }

        if(_layer == 3)
        {
            mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);

            return;
        }

        Debug.LogError($"There is no such layer index: {_layer}");
    }

    

    private void Start()
    {
        // awakebe vannak a feliratkoz�sok, ez�rt csak a startban kapcsoljunk ki mindent


        
    
        string path = Path.Combine(Application.persistentDataPath, "dailyscoredata");
        if (File.Exists(path))
        {
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.IsReadOnly = false;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GoalData[] _savedGoals = formatter.Deserialize(stream) as GoalData[];
            stream.Close();
            fileInfo.IsReadOnly = true;

            goalManager.LoadGoals(_savedGoals);
        }
        else
        {

        }
    
    


        introductionPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        createGoalPanel.SetActive(false);
        goalPanel.SetActive(false);
        createTaskPanel.SetActive(false);
        settingsPanel.SetActive(false);


        // set language if already saved one

        // decide starting page

    }

    private void Update()
    {

        if (!Application.isEditor)
        {
            if (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.Return)) { return; }
        }
        else // debug in editor
        {
            if (!Input.GetButtonDown("Jump")) { return; }
        }

        HandleBack();

    }

    private void HandleBack()
    {
        switch (GetAppLayer())
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
            case 2121:
                SetAppLayer(212);
                break;
            case 3:
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



       
        currentLanguage = _language;
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


    public static void NewTaskAdded()
    {
        OnNewTaskAdded?.Invoke();
    }

    private void NewTaskAddedCallback()
    {
        SetAppLayer(212);
    }


    public static void GoalOpened(Goal _goal)
    {
        OnGoalOpened?.Invoke(_goal);
    }

    private void OnGoalOpenedCallback(Goal _goal)
    {
        SetAppLayer(212);
    }


    public static void AppLayerChangedToMainMenu()
    {
        OnAppLayerChangedToMainMenu?.Invoke();
    }

    


   





    public void RemoteCall_GoToCreateGoalMenu()
    {
        SetAppLayer(211);
    }

    public void RemoteCall_GoToGoalMenu()
    {
        SetAppLayer(212);
    }

    public void RemoteCall_GoToCreateTaskMenu()
    { 
        SetAppLayer(2121);
    }

    public void RemoteCall_GoToMainMenu()
    {
        SetAppLayer(2);
    }

    public void RemoteCall_GoToSettingsMenu()
    {
        SetAppLayer(3);

       
    }

   



    public void DEBUG_RemoteCall_SaveGoals()
    {
        print("Save");
        string path = Path.Combine(Application.persistentDataPath, "dailyscoredata");
        if (File.Exists(path))
        {
            FileInfo fileInfoIfAlreadyExists = new FileInfo(path);
            fileInfoIfAlreadyExists.IsReadOnly = false;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        GoalData[] _goalDatas = goalManager.GetGoals();
        formatter.Serialize(stream, _goalDatas);
        stream.Close();
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.IsReadOnly = true;
    }

    public void DEBUG_RemoteCall_DeleteGoals()
    {
        print("delete");
        string path = Path.Combine(Application.persistentDataPath, "dailyscoredata");
        if (File.Exists(path))
        {
            FileInfo fileInfoIfAlreadyExists = new FileInfo(path);
            fileInfoIfAlreadyExists.IsReadOnly = false;
            File.Delete(path);
        }
    }



    private void OnApplicationQuit()
    {
       

        
    }

    
}
