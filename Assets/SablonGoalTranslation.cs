using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SablonGoalTranslation : MonoBehaviour
{


    [SerializeField]
    public string[] goalName = new string[(int)AppManager.Languages.ENUM_END];
    [Space]
    [SerializeField]
    public List<string> englishTaskNames = new List<string>();
    [Space]
    [SerializeField] 
    public List<string> hungarianTaskNames = new List<string>();


  
}
