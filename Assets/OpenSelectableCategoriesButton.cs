using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSelectableCategoriesButton : BehaviourButton
{
    [SerializeField] private GameObject selectablesPanel = null;


    protected override void OnTouch()
    {
        selectablesPanel.SetActive(!selectablesPanel.activeSelf);
    }

}
