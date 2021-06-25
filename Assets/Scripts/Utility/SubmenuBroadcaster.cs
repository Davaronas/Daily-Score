using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum DragType { Horizontal, Vertical};

public class SubmenuBroadcaster : ScrollDragBroadcast, IPointerDownHandler
{
    [SerializeField] private ScrollRect submenuScrollRect = null;
    private SubmenuScroll submenuScroll = null;

    private ScrollRect scrollRect = null;

    private Vector2 mousePosStart_;
    private Vector2 mousePosStartDrag_;

    private DragType dragType;


    private void Awake()
    {
        if (submenuScrollRect == null)
        {
            Debug.LogError("SubmenuScroll not set up on GoalsScroll");
            return;
        }
        submenuScroll = submenuScrollRect.GetComponent<SubmenuScroll>();
        scrollRect = GetComponent<ScrollRect>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(Input.touchCount > 0) mousePosStart_ = Input.GetTouch(0).position;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        mousePosStartDrag_ = Input.GetTouch(0).position;

        if(Mathf.Abs(mousePosStart_.x - mousePosStartDrag_.x) > Mathf.Abs(mousePosStart_.y - mousePosStartDrag_.y))
        {
            // we want to go horizontal

            scrollRect.StopMovement();
            scrollRect.enabled = false;
            submenuScrollRect.OnBeginDrag(eventData);
            
            dragType = DragType.Horizontal;
        }
        else
        {
            // we want to go vertical

            submenuScrollRect.StopMovement();
            submenuScrollRect.enabled = false;
            dragType = DragType.Vertical;
        }

        base.OnBeginDrag(eventData);
        submenuScroll.OnBeginDrag(eventData);
    }

    public void FeedClickPositionFromGoalButton(Vector2 _touchPos)
    {
        mousePosStart_ = _touchPos;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(dragType == DragType.Horizontal)
       submenuScrollRect.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        submenuScrollRect.enabled = true;
        scrollRect.enabled = true;
        submenuScrollRect.OnEndDrag(eventData);
        submenuScroll.OnEndDrag(eventData);
      //  AppManager.SubmenuChangedViaScrolling(submenuScroll.WarpToPosition());
    }

    
}
