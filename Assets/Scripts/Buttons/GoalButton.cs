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

    [SerializeField]private SubmenuBroadcaster goalsScrollRectBroadcaster = null;

    private Vector2 lastPosition = Vector2.zero;


    protected override void Start()
    {
        base.Start();

        rectTransform = GetComponent<RectTransform>();
        goal = GetComponent<Goal>();
        goalManager = FindObjectOfType<GoalManager>();
        submenuScroll = FindObjectOfType<SubmenuScroll>();
        submenuScrollRect = submenuScroll.GetComponent<ScrollRect>();

        if(goalsScrollRectBroadcaster == null)
        {
            Debug.LogError("Drag broadcaster not setup on goal prefab");
        }
    }

   
    protected override void OnRelease()
    {
        if (!goalsScrollRectBroadcaster.isBeingDragged)
        {
            goalManager.OpenGoalPanel(goal);
        }

        /*
        // hasonlítsd össze az elõzõvel, és döntsd el melyik akciót akarta a felhasználó csinálni:
        // következõ menüpont, felfele/lefele görgetés, kinyitás

        if (!Application.isEditor)
        {
            if (Input.touchCount > 0)
            {
                

                if (lastPosition.x < Input.GetTouch(0).position.x - 100)
                {
                    submenuScroll.WarpToPosition(1);
                    AppManager.SubmenuChangedViaScrolling(1);
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
                AppManager.SubmenuChangedViaScrolling(1);
            }
            else
            {
                goalManager.SetGoalPanelData(goal);
            }
        
        }
        */
    }

    

}
