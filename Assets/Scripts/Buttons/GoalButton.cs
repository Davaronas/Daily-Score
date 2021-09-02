using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoalButton : BehaviourButton
{
    private RectTransform rectTransform = null;
    private Goal goal = null;
    private Image image;

    private GoalManager goalManager = null;
    private SubmenuScroll submenuScroll = null;
    private ScrollRect submenuScrollRect = null;

    [SerializeField]private SubmenuBroadcaster goalsScrollRectBroadcaster = null;
    [SerializeField] private float holdSpeed = 0.02f;
    [SerializeField] private Color clickedColor = Color.white;

    private bool isHolding = false;
    private float filling;

    private Vector2 lastPosition = Vector2.zero;


    protected override void Start()
    {
        base.Start();

        rectTransform = GetComponent<RectTransform>();
        goal = GetComponent<Goal>();
        image = GetComponent<Image>();
        goalManager = FindObjectOfType<GoalManager>();
        submenuScroll = FindObjectOfType<SubmenuScroll>();
        submenuScrollRect = submenuScroll.GetComponent<ScrollRect>();

        if(goalsScrollRectBroadcaster == null)
        {
            Debug.LogError("Drag broadcaster not setup on goal prefab");
        }
    }

    private void Update()
    {
        if(isHolding && !goalsScrollRectBroadcaster.isBeingDragged)
        {
            filling += holdSpeed * Time.deltaTime;
            if(filling >= 1)
            {
                goalManager.AskToDeleteGoal(goal);
                filling = 0;
                isHolding = false;
                image.color = Color.white;

                SoundManager.PlaySound2();

                Handheld.Vibrate();
            }
        }
    }

    protected override void OnTouch()
    {
        isHolding = true;
        image.color = clickedColor;

        if (!Application.isEditor)
        {
            goalsScrollRectBroadcaster.FeedClickPosition(Input.GetTouch(0).position);
        }
        else
        {
            goalsScrollRectBroadcaster.FeedClickPosition(Input.mousePosition);
        }

        
    }

    protected override void OnRelease()
    {
        if (!goalsScrollRectBroadcaster.isBeingDragged && isHolding)
        {
            goalManager.OpenGoalPanel(goal);
        }
        isHolding = false;
        filling = 0;
        image.color = Color.white;
    }

    

}
