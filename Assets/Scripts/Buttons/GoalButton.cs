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
        // hasonl�tsd �ssze az el�z�vel, �s d�ntsd el melyik akci�t akarta a felhaszn�l� csin�lni:
        // k�vetkez� men�pont, felfele/lefele g�rget�s, kinyit�s

        goalManager.SetGoalPanelData(goal);
    }

}
