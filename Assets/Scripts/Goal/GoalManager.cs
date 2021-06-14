using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private RectTransform goalsScrollContentRectTransform = null;
   
    [SerializeField] private TMP_InputField createGoal_goalNameInputField = null;
    [Space]
    [SerializeField] private GameObject goalPrefab = null;

    [Space]
    [Space]
    [SerializeField] private TMP_Text goalMenuNameField = null;
    [SerializeField] private Image goalMenuSymbolImage = null;
    [SerializeField] private Image goalMenuPanel = null;
    [SerializeField] private RectTransform goalMenuScrollRectTransform = null;
    [SerializeField] [Range(0f, 1f)] private float goalMenuPanel_whiteStrengthOnGoalColor = 0.5f;

    private List<Goal> goals = new List<Goal>();
    private string enteredName = "";
    private GoalColor[] selectedColors;
    private ColorType colorType;
    private bool isColorSelected = false;
    private int spriteId = -1;
    private bool isSpriteSelected = false;

    private Goal currentlySelectedGoal = null;

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
    }

    public void LoadGoals(GoalData[] _goals)
    {
        for (int i = 0; i < _goals.Length; i++)
        {
            Goal _newGoal =
            Instantiate(goalPrefab, Vector3.zero, Quaternion.identity, goalsScrollContentRectTransform.transform).GetComponent<Goal>();
            _newGoal.isPrefab = false;
            _newGoal.SetData(_goals[i]);
            goals.Add(_newGoal);
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


    public void OpenGoalPanel(Goal _goal)
    {
        goalMenuNameField.text = _goal.goalName;
        goalMenuSymbolImage.sprite = AppManager.GetSpriteFromId(_goal.symbolId);

        GoalData _goaldata = _goal.GetGoalData();

        byte[] _rgb = new byte[3];
        _rgb[0] = (byte)Mathf.RoundToInt((_goaldata.color[0].r * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);
        _rgb[1] = (byte)Mathf.RoundToInt((_goaldata.color[0].g * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);
        _rgb[2] = (byte)Mathf.RoundToInt((_goaldata.color[0].b * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);

        goalMenuPanel.color = new Color32(_rgb[0], _rgb[1], _rgb[2], 255);

        // gradient?

        currentlySelectedGoal = _goal;
        AppManager.GoalOpened(_goal);

    }

    



    public void RemoteCall_SetSelectedName()
    {
        enteredName = createGoal_goalNameInputField.text;
    }


    public void RemoteCall_CreateNewGoal()
    {
        if (enteredName == "" || !isColorSelected || !isSpriteSelected) { return; }


        Goal _newGoal =
        Instantiate(goalPrefab, Vector3.zero, Quaternion.identity, goalsScrollContentRectTransform.transform).GetComponent<Goal>();
        GoalData _gd = new GoalData(enteredName, selectedColors,spriteId);
        _newGoal.isPrefab = false;
        _newGoal.SetData(_gd);

        goals.Add(_newGoal);

        ResizeGoalsContent();

        AppManager.NewGoalAdded();
    }

    public void AssignTaskToCurrentGoal(TaskData _data)
    {
        currentlySelectedGoal.AddTask(_data);
    }

    private void ResizeGoalsContent()
    {
        ScrollSizer.Resize(goalsScrollContentRectTransform, goalPrefab_Y_Size_ + (goalsContentLayoutGroup.spacing * 2), goals.Count);
    }

    public GoalData[] GetGoals()
    {
        List<GoalData> _goalDatas = new List<GoalData>();

        for(int i = 0; i < goals.Count;i++)
        {
            _goalDatas.Add(goals[i].GetGoalData());
        }
        return _goalDatas.ToArray();
    }

    public Goal GetCurrentlySelectedGoal()
    {
        return currentlySelectedGoal;
    }
   
}
