using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private RectTransform goalsScrollContentRectTransform = null;

   
    private void Awake()
    {
        AppManager.OnNewGoalAdded += NewGoalAdded;
    }

    private void OnDestroy()
    {
        AppManager.OnNewGoalAdded -= NewGoalAdded;
    }

    private void NewGoalAdded()
    {
        if (gameObject != null)
        {
            print("New goal added");
        }
    }
}
