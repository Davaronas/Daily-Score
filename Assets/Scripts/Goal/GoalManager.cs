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
    private Color32 selectedColor = new Color32();
    private bool isColorSelected = false;
    private int spriteId = -1;
    private bool isSpriteSelected = false;

    private Goal currentlySelectedGoal = null;

    private TaskManager taskManager = null;

    private void Awake()
    {
        //  AppManager.OnNewGoalAdded += NewGoalAdded;
        taskManager = FindObjectOfType<TaskManager>();
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
            _newGoal.SetData(_goals[i]);
            goals.Add(_newGoal);
        }
        
    }

    public void SetSpriteId(int _spriteId)
    {
        spriteId = _spriteId;
        isSpriteSelected = true;
    }


    public void SetSelectedColor(Color32 _color)
    {
        selectedColor = _color;
        isColorSelected = true;
    }


    public void SetGoalPanelData(Goal _goal)
    {
        goalMenuNameField.text = _goal.goalName;
        goalMenuSymbolImage.sprite = AppManager.GetSpriteFromId(_goal.symbolId);


        byte[] _rgb = new byte[3];
        _rgb[0] = (byte)Mathf.RoundToInt((_goal.goalColor.r * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);
        _rgb[1] = (byte)Mathf.RoundToInt((_goal.goalColor.g * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);
        _rgb[2] = (byte)Mathf.RoundToInt((_goal.goalColor.b * (1 - goalMenuPanel_whiteStrengthOnGoalColor) + 255 * (1 + goalMenuPanel_whiteStrengthOnGoalColor)) / 2);

        goalMenuPanel.color = new Color32(_rgb[0], _rgb[1], _rgb[2], 255);

        

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
        GoalData _gd = new GoalData(enteredName, selectedColor,spriteId);
        _newGoal.SetData(_gd);

        // resize goalsScrollContentRectTransform to fit the content

        goals.Add(_newGoal);
        AppManager.NewGoalAdded();
    }

    public void AssignTaskToCurrentGoal(TaskData _data)
    {
        currentlySelectedGoal.AddTask(_data);
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
