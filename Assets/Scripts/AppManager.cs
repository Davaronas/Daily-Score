using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

     


#region Goal related data types
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
public class GoalData
{
    public enum ModificationType { Create, ChangeValue}

    
    
    
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
        lastChange = new GoalChange(0,ModificationType.Create,DateTime.Now);
        modifications = new List<GoalChange>();
        modifications.Add(lastChange);


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
       lastChange = new GoalChange(0, ModificationType.Create, DateTime.Now);
       modifications = new List<GoalChange>();
        modifications.Add(lastChange);
        dailyScores = new List<ScorePerDay>();
    }

    public DateTime GetLastModificationTime()
    {
        return Convert.ToDateTime(lastChange.time);
    }

    public void AddModification(GoalChange _m)
    {
        modifications.Add(_m);
        lastChange = _m;
    }

    public void AddModification(int _amount)
    {
        GoalChange _gc = new GoalChange(_amount, ModificationType.ChangeValue, DateTime.Now);
        modifications.Add(_gc);
        lastChange = _gc;
    }

    public void AddDailyScore(int _amount, DateTime _time)
    {
        dailyScores.Add(new ScorePerDay(_amount, _time));
    }


    public string name;
   public GoalColor[] color;
   public ColorType colorType;
   public int spriteId;
   public int max;
   public int current;
   public  List<TaskData> tasks;

   public GoalChange lastChange;
    public List<GoalChange> modifications;
    public List<ScorePerDay> dailyScores;
    

}

[System.Serializable]
public struct GoalChange
{
    public GoalChange(int _amount,GoalData.ModificationType _modification, DateTime _time)
    {
        amount = _amount;
        modification = _modification;
        time = _time.ToString();
    }


    public int amount;
    public GoalData.ModificationType modification;
    public string time;

    public DateTime GetDateTime()
    {
        return Convert.ToDateTime(time);
    }
    
}
[System.Serializable]
public struct ScorePerDay
{
    public ScorePerDay(int _amount,  DateTime _time)
    {
        amount = _amount;
        time = _time.ToString();
    }


    public int amount;
    public string time;

    public DateTime GetDateTime()
    {
        return Convert.ToDateTime(time);
    }

}


[System.Serializable]
public class TaskData
{
    public enum ActiveType { DayOfWeek, EveryThDay }

    public TaskData(string _name)
   {
       name = _name;
       owner = null;
   }

    public string name;
    public AppManager.TaskType type;
    public string owner;


    public bool isActiveToday;
    public ActiveType beingActiveType;
    public List<int> activeOnDays;
    public int activeEveryThDay;

    public string nextActiveDay;

    public string lastChangedValue;
}



[System.Serializable]
public class MaximumTaskData : TaskData
{
    public MaximumTaskData(string _name, AppManager.TaskMetricType _metric,
        int _targetValue, int _pointsGainedPerOne, int _overachievePercentBonus = 0, int _streakStartsAfterDays = 0, int _current = 0) 
        : base(_name)
    {
        targetValue = _targetValue;
        type = AppManager.TaskType.Maximum;
        current = _current;
        metric = _metric;
        pointsGainedPerOne = _pointsGainedPerOne;
        overachievePercentBonus = _overachievePercentBonus;
        streakStartsAfterDays = _streakStartsAfterDays;
    }

    public int targetValue;
    public int current;
    public AppManager.TaskMetricType metric;
    public int pointsGainedPerOne;
    public int overachievePercentBonus;
    public int streakStartsAfterDays;
}

[System.Serializable]
public class MinimumTaskData : TaskData
{
    public MinimumTaskData(string _name, AppManager.TaskMetricType _metric,
       int _targetValue ,int _pointsForStayingUnderTargetValue, int _pointsLostPerOne, int _underTargetValuePercentBonus = 0, int _streakStartsAfterDays = 0, int _current = 0)
        : base(_name)
    {
        targetValue = _targetValue;
        type = AppManager.TaskType.Minimum;
        pointsForStayingUnderTargetValue = _pointsForStayingUnderTargetValue;
        metric = _metric;
        pointsLostPerOne = _pointsLostPerOne;
        underTargetValuePercentBonus = _underTargetValuePercentBonus;
        streakStartsAfterDays = _streakStartsAfterDays;
        current = _current;
    }

    public AppManager.TaskMetricType metric;
    public int targetValue;
    public int current;
    public int pointsForStayingUnderTargetValue;
    public int pointsLostPerOne;
    public int underTargetValuePercentBonus;
    public int streakStartsAfterDays;
}


[System.Serializable]
public class BooleanTaskData : TaskData
{
    public BooleanTaskData(string _name, int _pointsGained, int _streakStartsAfterDays = 0) : base(_name)
    {
        type = AppManager.TaskType.Boolean;
        pointsGained = _pointsGained;
        streakStartsAfterDays = _streakStartsAfterDays;
        isDone = false;
    }

    public bool isDone;
    public int pointsGained;
    public int streakStartsAfterDays;
}


[System.Serializable]
public class OptimumTaskData : TaskData
{
    public OptimumTaskData(string _name, int _targetValue, int _pointsForOptimum, int _pointsLostPerOneDifference, AppManager.TaskMetricType _metric, int _streakStartsAfterDays = 0, int _current = 0) : base(_name)
    {
        type = AppManager.TaskType.Optimum;
        targetValue = _targetValue;
        pointsForOptimum = _pointsForOptimum;
        pointsLostPerOneDifference = _pointsLostPerOneDifference;
        streakStartsAfterDays = _streakStartsAfterDays;
        current = _current;
        metric = _metric;
    }

    public int targetValue;
    public int current;
    public AppManager.TaskMetricType metric;
    public int pointsForOptimum;
    public int pointsLostPerOneDifference;
    public int streakStartsAfterDays;
}

[System.Serializable]
public class IntervalTaskData : TaskData
{
    public IntervalTaskData(string _name, Interval[] _intervals, AppManager.TaskMetricType _metric) : base(_name)
    {
        type = AppManager.TaskType.Interval;
        metric = _metric;
        intervals = _intervals;
    }

    public int current;
    public AppManager.TaskMetricType metric;
    public Interval[] intervals;
}

[System.Serializable]
public struct Interval
{
    public Interval(int _from, int _to, int _points)
    {
        from = _from;
        to = _to;
        points = _points;
    }

    public int from;
    public int to;
    public int points;

    public void GetRange(out int _from, out int _to)
    {
        _from = from;
        _to = to;
    }
}



public static class ErrorMessages
{
    public static string NameNotEntered()
    {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.NameNotEntered;

            case AppManager.Languages.Magyar:
                return Magyar.NameNotEntered;

            case AppManager.Languages.Deutsch:
                return Deutsch.NameNotEntered;


            default:
                return "";

        }
    }

    public static string NewDayStarted()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.NewDayStarted;

            case AppManager.Languages.Magyar:
                return Magyar.NewDayStarted; ;

            case AppManager.Languages.Deutsch:
                return Deutsch.NewDayStarted; ;


            default:
                return "";

        }
    }

    public static string SavedTipContainerIsFull()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.SavedTipContainerIsFull;

            case AppManager.Languages.Magyar:
                return Magyar.SavedTipContainerIsFull;

            case AppManager.Languages.Deutsch:
                return Deutsch.SavedTipContainerIsFull;


            default:
                return "";

        }
    }

     public static string ColorNotSelected_CreateGoalPanel()
    {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.ColorNotSelected_CreateGoalPanel;

            case AppManager.Languages.Magyar:
                return Magyar.ColorNotSelected_CreateGoalPanel;

            case AppManager.Languages.Deutsch:
                return Deutsch.ColorNotSelected_CreateGoalPanel;


            default:
                return "";

        }
    }

    public static string SymbolNotSelected_CreateGoalPanel()
    {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.SymbolNotSelected_CreateGoalPanel;

            case AppManager.Languages.Magyar:
                return Magyar.SymbolNotSelected_CreateGoalPanel;

            case AppManager.Languages.Deutsch:
                return Deutsch.SymbolNotSelected_CreateGoalPanel;


            default:
                return "";

        }
    }

public static string DaysNotSelected_CreateTaskPanel()
    {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.DaysNotSelected_CreateTaskPanel;

            case AppManager.Languages.Magyar:
                return Magyar.DaysNotSelected_CreateTaskPanel;

            case AppManager.Languages.Deutsch:
                return Deutsch.DaysNotSelected_CreateTaskPanel;


            default:
                return "";

        }
    }

    public static string TaskTypeNotSelected_CreateTaskPanel()
    {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.TaskTypeNotSelected_CreateTaskPanel;

            case AppManager.Languages.Magyar:
                return Magyar.TaskTypeNotSelected_CreateTaskPanel;

            case AppManager.Languages.Deutsch:
                return Deutsch.TaskTypeNotSelected_CreateTaskPanel;


            default:
                return "";

        }
    }

    public static string TaskTypeInputFieldEmpty_CreateTaskTPanel()
    {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.TaskTypeInputFieldEmpty_CreateTaskTPanel;

            case AppManager.Languages.Magyar:
                return Magyar.TaskTypeInputFieldEmpty_CreateTaskTPanel;

            case AppManager.Languages.Deutsch:
                return Deutsch.TaskTypeInputFieldEmpty_CreateTaskTPanel;


            default:
                return "";

        }
    }

    public static string IntervalTaskTypeOverlap_CreateTaskPanel()
    {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.IntervalTaskTypeOverlap_CreateTaskPanel;

            case AppManager.Languages.Magyar:
                return Magyar.IntervalTaskTypeOverlap_CreateTaskPanel;

            case AppManager.Languages.Deutsch:
                return Deutsch.IntervalTaskTypeOverlap_CreateTaskPanel;


            default:
                return "";

        }
    }

    public static class English
    {
        public const string NameNotEntered = "Please enter a name!";
        public const string ColorNotSelected_CreateGoalPanel = "Please select a color!";
        public const string SymbolNotSelected_CreateGoalPanel = "Please select a symbol!";
        public const string DaysNotSelected_CreateTaskPanel = "Please specify when do you want this task to be active!";
        public const string TaskTypeNotSelected_CreateTaskPanel = "Please select a way to earn points!";
        public const string TaskTypeInputFieldEmpty_CreateTaskTPanel = "One or more required input fields are empty!";
        public const string IntervalTaskTypeOverlap_CreateTaskPanel = "One or more intervals overlap. Please ensure the ranges of the intervals do not overlap!";
        public const string NewDayStarted = "New day started!";
        public const string SavedTipContainerIsFull = "Saved Tip container is full, consider switching to a Gold account, if you haven't already";
    }

    public static class Magyar
    {
        public const string NameNotEntered = "Please select a name!";
        public const string ColorNotSelected_CreateGoalPanel = "Please select a color!";
        public const string SymbolNotSelected_CreateGoalPanel = "Please select a symbol!";
        public const string DaysNotSelected_CreateTaskPanel = "Please specify when do you want this task to be active!";
        public const string TaskTypeNotSelected_CreateTaskPanel = "Please select a way to earn points!";
        public const string TaskTypeInputFieldEmpty_CreateTaskTPanel = "One or more required input fields are empty!";
        public const string IntervalTaskTypeOverlap_CreateTaskPanel = "One or more intervals overlap. Please ensure the ranges of the intervals do not overlap!";
        public const string NewDayStarted = "New day started!";
        public const string SavedTipContainerIsFull = "Saved Tip container is full, consider switching to a Gold account, if you haven't already";
    }


    public static class Deutsch
    {
        public const string NameNotEntered = "Please enter a name!";
        public const string ColorNotSelected_CreateGoalPanel = "Please select a color!";
        public const string SymbolNotSelected_CreateGoalPanel = "Please select a symbol!";
        public const string DaysNotSelected_CreateTaskPanel = "Please specify when do you want this task to be active!";
        public const string TaskTypeNotSelected_CreateTaskPanel = "Please select a way to earn points!";
        public const string TaskTypeInputFieldEmpty_CreateTaskTPanel = "One or more required input fields are empty!";
        public const string IntervalTaskTypeOverlap_CreateTaskPanel = "One or more intervals overlap. Please ensure the ranges of the intervals do not overlap!";
        public const string NewDayStarted = "New day started!";
        public const string SavedTipContainerIsFull = "Saved Tip container is full, consider switching to a Gold account, if you haven't already";

    }

    // max task type

}


#endregion


public class AppManager : MonoBehaviour
{
   [SerializeField] private GameObject introductionPanel;
   [SerializeField] private GameObject languagePanel;
   [SerializeField] private GameObject mainMenuPanel;
   [SerializeField] private GameObject createGoalPanel;
   [SerializeField] private GameObject goalPanel;
   [SerializeField] private GameObject createTaskPanel;
   [SerializeField] private GameObject settingsPanel;
    [SerializeField] private ErrorWindow errorPanel;
   [Space]
   [Space]
   [SerializeField] private Sprite[] symbols_e;
   private static Sprite[] symbols;

    public string testTime;
    public string testLastLoginTime;
    // yyyy. mm. dd. 0:00:00



    private GoalManager goalManager = null;
   private TaskManager taskManager = null;

   public enum Languages { English, Magyar, Deutsch, ENUM_END };
   public static Languages currentLanguage { get; private set; } = 0;

    
   public enum TaskType { Maximum, Minimum, Boolean, Optimum, Interval, ENUM_END };


    public static bool isNightModeOn = false;
    public static bool isGold = false;



   public enum TaskMetricType {Pieces, Minutes, Kilometres, Mile, Grams, Pound, Calorie, Other, ENUM_END };

   // all static events should be here

   public static event Action<Languages> OnLanguageChanged;

   public static event Action<int> OnSubmenuButtonPressed;
   public static event Action<int> OnSubmenuChangedViaScrolling;
   public static event Action OnNewGoalAdded; // might not be needed
   public static event Action OnNewTaskAdded;
    public static event Action<SingleColorPicker> OnGoalColorPicked;
    public static event Action<SymbolPicker> OnGoalSymbolPicked;
    public static event Action<TaskData> OnTaskValueChanged;
    public static event Action<string> OnErrorHappened;
    public static event Action OnNewDayStartedDuringRuntime;

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
        OnErrorHappened += OnErrorHappenedCallback;

        goalManager = FindObjectOfType<GoalManager>();
        taskManager = FindObjectOfType<TaskManager>();

       


        symbols = symbols_e;
    }

    private void OnDestroy()
    {
        OnNewGoalAdded -= NewGoalAddedCallback;
        OnNewTaskAdded -= NewTaskAddedCallback;
        OnGoalOpened -= OnGoalOpenedCallback;
        OnErrorHappened -= OnErrorHappenedCallback;

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

    private void GoalActivityCheck(GoalData[] _goaldatas)
    {
        DateTime _today = DateTime.Now.Date;   // Convert.ToDateTime(testTime); //
        print(_today.ToString());

        if (DateTime.Now.Date == _today) { return; } // still the same day, we don't need to reset


        // reset goals
        for (int i = 0; i < _goaldatas.Length; i++)
        {
          //  if(_goaldatas[i].current > 0)
            {
                _goaldatas[i].AddDailyScore(_goaldatas[i].current,lastLogin.Date);
                _goaldatas[i].current = 0;


                // reset tasks
                for (int j = 0; j < _goaldatas[i].tasks.Count; j++)
                {
                    switch(_goaldatas[i].tasks[j].type)
                    {
                        case TaskType.Maximum:
                            ((MaximumTaskData)_goaldatas[i].tasks[j]).current = 0;
                            break;
                        case TaskType.Minimum:
                            ((MinimumTaskData)_goaldatas[i].tasks[j]).current = 0;
                        break;
                        case TaskType.Boolean:
                            ((BooleanTaskData)_goaldatas[i].tasks[j]).isDone = false;
                            break;
                        case TaskType.Optimum:
                            ((OptimumTaskData)_goaldatas[i].tasks[j]).current = 0;
                            break;
                        case TaskType.Interval:
                            ((IntervalTaskData)_goaldatas[i].tasks[j]).current = 0;
                            break;
                    }

                    _goaldatas[i].tasks[j].isActiveToday = false;

                    // check if tasks should be active today
                    if (_goaldatas[i].tasks[j].beingActiveType == TaskData.ActiveType.DayOfWeek)
                    {
                        for (int k = 0; k < _goaldatas[i].tasks[j].activeOnDays.Count; k++)
                        {
                            if (_today.DayOfWeek == (DayOfWeek)_goaldatas[i].tasks[j].activeOnDays[k])
                            {
                                _goaldatas[i].tasks[j].isActiveToday = true;
                            }
                        }
                        
                    }
                    else if(_goaldatas[i].tasks[j].beingActiveType == TaskData.ActiveType.EveryThDay)
                    {
                        
                        if(Convert.ToDateTime(_goaldatas[i].tasks[j].nextActiveDay).Date == _today) // next date is today
                        {
                            _goaldatas[i].tasks[j].isActiveToday = true;
                        }
                        else
                        {
                            DateTime _nextThDays = Convert.ToDateTime(_goaldatas[i].tasks[j].nextActiveDay).Date;
                            while (_nextThDays < _today)
                            {
                                _nextThDays = _nextThDays.AddDays(_goaldatas[i].tasks[j].activeEveryThDay);
                                if (_nextThDays == _today) // one of every th day is today
                                {
                                    _goaldatas[i].tasks[j].isActiveToday = true;
                                    _goaldatas[i].tasks[j].nextActiveDay = DateTime.Now.Date.AddDays(_goaldatas[i].tasks[j].activeEveryThDay).ToString();
                                    break;
                                }
                            }
                            
                        }
                    }
                }

                // add 0 points for each missed day

                /*
                for(int o = 1; o < (_today - lastLogin).Days;o++)
                {
                    _goaldatas[i].AddDailyScore(0, lastLogin.AddDays(o));
                }
                */
            }
        }
    }

    private void Start()
    {
        // awakebe vannak a feliratkozások, ezért csak a startban kapcsoljunk ki mindent


        
    
        string path = Path.Combine(Application.persistentDataPath, "dailyscoredata");
        if (File.Exists(path))
        {
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.IsReadOnly = false;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GoalData[] _savedGoals = formatter.Deserialize(stream) as GoalData[];
            foreach(GoalData _gd in _savedGoals)
            {
                print(_gd.current);
            }
            stream.Close();
            fileInfo.IsReadOnly = true;

            // check each goal if they should be active today

            
            lastLogin = Convert.ToDateTime(PlayerPrefs.GetString("lastLogin",DateTime.Now.ToString()));
            GoalActivityCheck(_savedGoals);

            goalManager.LoadGoals(_savedGoals);
        }
        else
        {

        }

        // in case we cannot reach OnApplicationQuit (crash or dead battery) we set the last login here as well, after we evaluate it earlier
        lastLogin = DateTime.Now;
        PlayerPrefs.SetString("lastLogin", lastLogin.ToString());


        introductionPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        createGoalPanel.SetActive(false);
        goalPanel.SetActive(false);
        createTaskPanel.SetActive(false);
        settingsPanel.SetActive(false);
        errorPanel.gameObject.SetActive(false);


        // set language if already saved one

        // decide starting page


        StartCoroutine(TimeChecker());

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
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

  


    public static void ErrorHappened(string _error)
    {
        OnErrorHappened?.Invoke(_error);
    }

    public void OnErrorHappenedCallback(string _error)
    {
        errorPanel.ShowError(_error);
        errorPanel.gameObject.SetActive(true);
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

    public static void GoalColorPicked(SingleColorPicker _scp)
    {
        OnGoalColorPicked?.Invoke(_scp);
    }

    public static void GoalSymbolPicked(SymbolPicker _sp)
    {
        OnGoalSymbolPicked?.Invoke(_sp);
    }

    public static void TaskValueChanged(TaskData _td)
    {
        OnTaskValueChanged?.Invoke(_td);
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

    public void RemoteCall_TurnOffErrorPanel()
    {
        errorPanel.gameObject.SetActive(false);
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
        foreach (GoalData _gd in _goalDatas)
        {
            print(_gd.current);
        }
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

        lastLogin = DateTime.Now.Date;
        PlayerPrefs.SetString("lastLogin", lastLogin.ToString());
    }



    public const int SAVEDTIPAMOUNT_FREE = 5;
    public const int SAVEDTIPAMOUNT_GOLD = 21;



    //Last Login
    public static DateTime lastLogin;

    IEnumerator TimeChecker()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            
            if(DateTime.Now.TimeOfDay == new TimeSpan(0,0,0))
            {
                // save goal daily scores

                // reset tasks
                AppManager.ErrorHappened(ErrorMessages.NewDayStarted());
                GoalActivityCheck(goalManager.GetGoals());
                lastLogin = DateTime.Now;
                PlayerPrefs.SetString("lastLogin", lastLogin.ToString());

                OnNewDayStartedDuringRuntime?.Invoke();
            }

            
        }
    }

}
