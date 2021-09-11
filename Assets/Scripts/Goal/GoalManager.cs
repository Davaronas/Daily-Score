using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private RectTransform goalsScrollContentRectTransform = null;
   
    [SerializeField] private TMP_InputField createGoal_goalNameInputField = null;
    [Space]
    [SerializeField] private GameObject goalPrefab = null;
    [SerializeField] private GameObject askToDeleteGoalPanel = null;
    [SerializeField] private TMP_Text askToDeleteGoal_goalName = null;

    [Space]
    [Space]
    [SerializeField] private TMP_Text goalMenuNameField = null;
    [SerializeField] private Image goalMenuSymbolImage = null;
    [SerializeField] private Image goalMenuPanel = null;
    [SerializeField] private RectTransform goalMenuScrollRectTransform = null;
    [SerializeField] private GameObject restDayPanel = null;
    [SerializeField] [Range(0f, 1f)] private float goalMenuPanel_whiteStrengthOnGoalColor = 0.5f;

    private List<Goal> goals = new List<Goal>();
    private List<GoalData> deletedGoals = new List<GoalData>();
    private string enteredName = "";
    private GoalColor[] selectedColors;
    private ColorType colorType;
    private bool isColorSelected = false;
    private int spriteId = -1;
    private bool isSpriteSelected = false;

    private Goal currentlySelectedGoal = null;
    private Goal goalToDelete = null;

    private TaskManager taskManager = null;


    private VerticalLayoutGroup goalsContentLayoutGroup = null;

    // aid variables
    private float goalPrefab_Y_Size_;


    private void Awake()
    {
        //  AppManager.OnNewGoalAdded += NewGoalAdded;
        taskManager = FindObjectOfType<TaskManager>();
        goalsContentLayoutGroup = goalsScrollContentRectTransform.GetComponent<VerticalLayoutGroup>();
        goalPrefab_Y_Size_ = goalPrefab.GetComponent<RectTransform>().sizeDelta.y;
        restDayPanel.SetActive(false);
        
    }

    private void OnDestroy()
    {
        // AppManager.OnNewGoalAdded -= NewGoalAdded;
    }

    private void OnDisable()
    {
        enteredName = "";
        createGoal_goalNameInputField.text = "";
        isColorSelected = false;
        isSpriteSelected = false;
        askToDeleteGoalPanel.SetActive(false);
    }

    public void LoadGoals(GoalData[] _goals)
    {
        for (int i = 0; i < _goals.Length; i++)
        {
            if (!_goals[i].isDeleted)
            {
                Goal _newGoal =
                Instantiate(goalPrefab, Vector3.zero, Quaternion.identity, goalsScrollContentRectTransform.transform).GetComponent<Goal>();
                _newGoal.isPrefab = false;
                //  print(_goals[i].current);
                _newGoal.SetData(_goals[i]);
                goals.Add(_newGoal);
            }
            else
            {
                deletedGoals.Add(_goals[i]);
            }
        }


        ResizeGoalsContent();

    }

    public void SetSpriteId(int _spriteId)
    {
        spriteId = _spriteId;
        isSpriteSelected = true;
    }


    public void SetSelectedColor(Color32 _color)
    {
        colorType = ColorType.Simple;
        selectedColors = new GoalColor[1];
        selectedColors[0] = _color;
        isColorSelected = true;
    }

    public void SetSelectedColor(Color32[] _colors)
    {
        if (_colors.Length == 2)
        {
            colorType = ColorType.Gradient;
            selectedColors = new GoalColor[2];
            selectedColors[0] = _colors[0];
            selectedColors[1] = _colors[1];
            isColorSelected = true;
        }
        else if(_colors.Length == 4)
        {
            Debug.Log("Four corner gradient not set up yet");
        }
    }

    public void RestDayActivated()
    {
        restDayPanel.SetActive(true);
    }

    public void Reset()
    {
        for (int i = 0; i < goals.Count; i++)
        {
            Destroy(goals[i].gameObject);
        }

        goals.Clear();
    }
    public void OpenGoalPanel(Goal _goal)
    {
        goalMenuNameField.text = _goal.goalName;
        goalMenuSymbolImage.sprite = AppManager.GetSpriteFromId(_goal.symbolId);

        GoalData _goaldata = _goal.GetGoalData();

        /*
        byte[] _rgb = new byte[3];
        _rgb[0] = (byte)Mathf.RoundToInt((_goaldata.color[0].r * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);
        _rgb[1] = (byte)Mathf.RoundToInt((_goaldata.color[0].g * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);
        _rgb[2] = (byte)Mathf.RoundToInt((_goaldata.color[0].b * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);

        goalMenuPanel.color = new Color32(_rgb[0], _rgb[1], _rgb[2], 255);
        */

        // gradient?

        currentlySelectedGoal = _goal;
        AppManager.GoalOpened(_goal);

        SoundManager.PlaySound2();

    }

    
    public void AskToDeleteGoal(Goal _goal)
    {
        // ask panel

        //   DeleteGoal(_goal);
        goalToDelete = _goal;
        askToDeleteGoal_goalName.text = _goal.GetGoalData().name;
        ShowAskToDeleteGoalPanel();
    }

    private void ShowAskToDeleteGoalPanel()
    {
        askToDeleteGoalPanel.SetActive(true);
    }

    private void HideAskToDeleteGoalPanel()
    {
        askToDeleteGoalPanel.SetActive(false);
    }

    public void RemoteCall_DeleteGoal()
    {
        deletedGoals.Add(goalToDelete.GetGoalData());
        goalToDelete.GetGoalData().isDeleted = true;
        goals.Remove(goalToDelete);
        Destroy(goalToDelete.gameObject);
        askToDeleteGoalPanel.SetActive(false);

        AppManager.GoalDeleted();

        SoundManager.PlaySound5();
    }

    public void RemoteCall_CancelDeleteGoal()
    {
        HideAskToDeleteGoalPanel();

        SoundManager.PlaySound6();
    }


    public void RemoteCall_SetSelectedName()
    {
        enteredName = createGoal_goalNameInputField.text;
    }


    public void RemoteCall_CreateNewGoal()
    {
        if (enteredName == "")
        { AppManager.ErrorHappened(ErrorMessages.NameNotEntered()); return; }

        if(enteredName.Length > AppManager.MAXNAMESIZE)
        { AppManager.ErrorHappened(ErrorMessages.NameTooLong()); return; }

        if (!isColorSelected)
        { AppManager.ErrorHappened(ErrorMessages.ColorNotSelected_CreateGoalPanel()); return; }

        if (!isSpriteSelected)
        { AppManager.ErrorHappened(ErrorMessages.SymbolNotSelected_CreateGoalPanel()); return; }

        if(IsNameAlreadyTaken(enteredName))
        { AppManager.ErrorHappened(ErrorMessages.NameIsTaken()); return; }    


        Goal _newGoal =
        Instantiate(goalPrefab, Vector3.zero, Quaternion.identity, goalsScrollContentRectTransform.transform).GetComponent<Goal>();
        GoalData _gd = new GoalData(enteredName, selectedColors,spriteId);
        _newGoal.isPrefab = false;
        _newGoal.SetData(_gd);

        goals.Add(_newGoal);

        ResizeGoalsContent();

        AppManager.NewGoalAdded();

        SoundManager.PlaySound4();
    }

    public void AssignTaskToCurrentGoal(TaskData _data)
    {
        currentlySelectedGoal.AddTask(_data);
        _data.owner = currentlySelectedGoal.GetGoalData().name;
    }

    private void ResizeGoalsContent()
    {
        ScrollSizer.Resize(goalsScrollContentRectTransform, goalPrefab_Y_Size_ + (goalsContentLayoutGroup.spacing * 2), goals.Count);
    }

    public GoalData[] GetExistingGoals()
    {
        List<GoalData> _goalDatas = new List<GoalData>();

        for(int i = 0; i < goals.Count;i++)
        {
            _goalDatas.Add(goals[i].GetGoalData());
        }
        return _goalDatas.ToArray();
    }

    public GoalData[] GetAllGoals()
    {
        List<GoalData> _goalDatas = new List<GoalData>();

        for (int i = 0; i < goals.Count; i++)
        {
            _goalDatas.Add(goals[i].GetGoalData());
        }

        for (int j = 0; j < deletedGoals.Count; j++)
        {
            _goalDatas.Add(deletedGoals[j]);
        }

        return _goalDatas.ToArray();
    }

    public bool GetLastModificationTime(out GoalData _lastModifiedGoalData)
    {
        _lastModifiedGoalData = null;

        

        if(goals.Count == 0) { return false; }

        for(int i = 0; i < goals.Count;i++)
        {
            if(goals[i].GetGoalData().GetLastModificationTime() > _lastModifiedGoalData.GetLastModificationTime())
            {
                _lastModifiedGoalData = goals[i].GetGoalData();
            }
        }

        return true;
    }

    public Goal GetCurrentlySelectedGoal()
    {
        return currentlySelectedGoal;
    }

    public bool SearchGoalByName(string _name, out GoalData _gd)
    {
        _gd = null;

        for (int i = 0; i < goals.Count; i++)
        {
            if(goals[i].GetGoalData().name == _name)
            {
                _gd = goals[i].GetGoalData();
                return true;
            }
        }

        return false;
    }

    public Color32[] GetGoalColors()
    {
        List<Color32> _colors = new List<Color32>();
        for (int i = 0; i < goals.Count; i++)
        {
           _colors.Add(goals[i].GetGoalData().color[0]);
        }

        return _colors.ToArray();
    }

    public GoalData SearchGoalByName(string _name)
    {
        GoalData _gd;
        _gd = null;

        for (int i = 0; i < goals.Count; i++)
        {
            if (goals[i].GetGoalData().name == _name)
            {
                _gd = goals[i].GetGoalData();
            }
        }
        return _gd;
    }

    public bool IsNameAlreadyTaken(string _name)
    {
        GoalData[] _gds = GetExistingGoals();


        for (int i = 0; i < _gds.Length; i++)
        {
            if(_gds[i].name == _name)
            {
                return true;
            }

            for (int j = 0; j < _gds[i].tasks.Count; j++)
            {
                if(_gds[i].tasks[j].name == _name)
                {
                    return true;
                }
            }
        }

        return false;
    }
   
}
