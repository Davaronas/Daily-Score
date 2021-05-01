using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private RectTransform goalsScrollContentRectTransform = null;
    [SerializeField] private  TMP_InputField createGoal_goalNameInputField = null;
    [Space]
    [SerializeField] private GameObject goalPrefab = null;
    [Space]
    [Space]
    [SerializeField] private TMP_Text goalMenuNameField = null;
    [SerializeField] private Image goalMenuSymbolImage = null;
    [SerializeField] private Image goalMenuPanel = null;
    [SerializeField] private RectTransform goalMenuScrollRectTransform = null;

    private string enteredName = "";
    private Color32 selectedColor = new Color32();
    private bool isColorSelected = false;
    private int spriteId = -1;
    private bool isSpriteSelected = false;
   
    private void Awake()
    {
      //  AppManager.OnNewGoalAdded += NewGoalAdded;
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

        AppManager.GoalOpened(_goal);
    }





    public void RemoteCall_SetSelectedName()
    {
        enteredName = createGoal_goalNameInputField.text;
    }

    
    public void RemoteCall_CreateNewGoal()
    {
        if(enteredName == "" || !isColorSelected || !isSpriteSelected) { return; }

        Goal _newGoal =
        Instantiate(goalPrefab, Vector3.zero, Quaternion.identity, goalsScrollContentRectTransform.transform).GetComponent<Goal>();
        _newGoal.SetData(enteredName, selectedColor, spriteId);

        // resize goalsScrollContentRectTransform to fit the content

        AppManager.NewGoalAdded();
    }



}
