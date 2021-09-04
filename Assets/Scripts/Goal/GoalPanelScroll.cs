using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoalPanelScroll : ScrollDragBroadcast
{
    private Vector2 mousePosStart_;
    private Vector2 mousePosStartDrag_;

    [SerializeField] private float thresholdDistance = 3f;

   [HideInInspector] public bool allowOpenTask = true;

    private void Start()
    {
        allowOpenTask = true;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        if (!Application.isEditor)
        {
            mousePosStartDrag_ = Input.GetTouch(0).position;
        }
        else
        {
            mousePosStartDrag_ = Input.mousePosition;
        }


        if ((mousePosStart_ - mousePosStartDrag_).magnitude < thresholdDistance)
        {
            allowOpenTask = true;
        }
        else
        {
            allowOpenTask = false;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        allowOpenTask = true;
    }

    public void FeedClickPosition(Vector2 _touchPos)
    {
        mousePosStart_ = _touchPos;
    }
}
