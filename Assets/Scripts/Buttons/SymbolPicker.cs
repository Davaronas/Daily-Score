using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SymbolPicker : BehaviourButton
{
    public int spriteId = -1;
    private GoalManager goalManager = null;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        if (goalManager == null)
        {
            Debug.LogError("GoalManager doesn't exist, can't send color");
        }
    }

    protected override void OnTouch()
    {
        goalManager.SetSpriteId(spriteId);
    }
}
