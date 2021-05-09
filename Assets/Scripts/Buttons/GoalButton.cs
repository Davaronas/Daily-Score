using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoalButton : BehaviourButton
{
    private RectTransform rectTransform = null;
    private Goal goal = null;

    private GoalManager goalManager = null;
    private SubmenuScroll submenuScroll = null;
    private ScrollRect submenuScrollRect = null;

    private Vector2 lastPosition = Vector2.zero;


    protected override void Start()
    {
        base.Start();

        rectTransform = GetComponent<RectTransform>();
        goal = GetComponent<Goal>();
        goalManager = FindObjectOfType<GoalManager>();
        submenuScroll = FindObjectOfType<SubmenuScroll>();
        submenuScrollRect = submenuScroll.GetComponent<ScrollRect>();
    }

    protected override void OnTouch()
    {
        if (!Application.isEditor)
        {
            if (Input.touchCount > 0)
            {
                lastPosition = Input.GetTouch(0).position;
            }
        }
        else
        {
            lastPosition = Input.mousePosition;
        }
    }

    protected override void OnRelease()
    {
        // hasonl�tsd �ssze az el�z�vel, �s d�ntsd el melyik akci�t akarta a felhaszn�l� csin�lni:
        // k�vetkez� men�pont, felfele/lefele g�rget�s, kinyit�s

        if (!Application.isEditor)
        {
            if (Input.touchCount > 0)
            {
                

                if (lastPosition.x < Input.GetTouch(0).position.x - 100)
                {
                    submenuScroll.WarpToPosition(1);
                }
                else
                {
                    goalManager.SetGoalPanelData(goal);
                }
            }
        }
        else
        {
            if(lastPosition.x < Input.mousePosition.x - 100 )
            {
                submenuScroll.WarpToPosition(1);
            }
            else
            {
                goalManager.SetGoalPanelData(goal);
            }
        }
    }

    protected override void OnDrag()
    {
        submenuScrollRect.OnDrag(new PointerEventData(EventSystem.current));
    }

}
