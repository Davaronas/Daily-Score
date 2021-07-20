using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalColorChecker : MonoBehaviour
{
    private GoalManager goalManager = null; 
    [SerializeField] private Image[] colorObjects = null;
     private Dictionary<Image, ImageColoring> goalColors = new Dictionary<Image, ImageColoring>();
    [SerializeField] private Color32 disabledColor;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        for (int i = 0; i < colorObjects.Length; i++)
        {
            goalColors.Add(colorObjects[i], colorObjects[i].GetComponent<ImageColoring>());
        }
    }


    private void OnDisable()
    {
       foreach(KeyValuePair<Image,ImageColoring> _k in goalColors)
        {
            _k.Key.raycastTarget = true;
            _k.Key.color = Color.white;
        }
    }

    private void OnEnable()
    {
        Color32[] _existingColors = goalManager.GetGoalColors();
        foreach (KeyValuePair<Image, ImageColoring> _k in goalColors)
        {
            for (int i = 0; i < _existingColors.Length; i++)
            {
                if (_k.Value.color1.Equals(_existingColors[i]))
                {
                    _k.Key.raycastTarget = false;
                    _k.Key.color = disabledColor;
                    break;
                }
            }

        }
    }


}
