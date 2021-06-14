using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class StatisticCalculator : MonoBehaviour
{
     public GoalManager goalManager = FindObjectOfType<GoalManager>();
    // Start is called before the first frame update
    void Start()
    {
        
        GoalData[] goalDatas = goalManager.GetGoals();
        
    }

    
}
