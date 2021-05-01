using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalButton : BehaviourButton
{
    private RectTransform rectTransform = null;
    private Goal goal = null;

    private GoalManager goalManager = null;

    private Vector2 lastPosition = Vector2.zero;

    protected override void Start()
    {
        base.Start();

        rectTransform = GetComponent<RectTransform>();
        goal = GetComponent<Goal>();
        goalManager = FindObjectOfType<GoalManager>();
    }

    protected override void OnTouch()
    {
        lastPosition = rectTransform.anchoredPosition;
    }

    protected override void OnRelease()
    {
        // hasonlítsd össze az elõzõvel, és döntsd el melyik akciót akarta a felhasználó csinálni:
        // következõ menüpont, felfele/lefele görgetés, kinyitás

        goalManager.SetGoalPanelData(goal);
    }

}
