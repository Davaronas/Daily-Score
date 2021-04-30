using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private RectTransform goalsScrollContentRectTransform = null;

    [SerializeField] private  TMP_InputField goalNameInputField = null;

    [SerializeField] private GameObject goalPrefab = null;

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
        goalNameInputField.text = "";
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






    public void RemoteCall_SetSelectedName()
    {
        enteredName = goalNameInputField.text;
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
