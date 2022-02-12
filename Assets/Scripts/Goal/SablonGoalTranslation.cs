using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SablonGoalTranslation : MonoBehaviour
{
    [Serializable]
    public struct UnitOfMeasureTranslation
    {
        public int taskId;
        public string en_Translation;
        public string hun_Translation;
    }

    [SerializeField]
    public string[] goalName = new string[(int)AppManager.Languages.ENUM_END];
    [Space]
    [SerializeField]
    public List<string> englishTaskNames = new List<string>();
    [Space]
    [SerializeField] 
    public List<string> hungarianTaskNames = new List<string>();

    [Space]
    [SerializeField] public List< UnitOfMeasureTranslation> unitOfMeasureTranslations;
  
}
