using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(ScrollRect))]
public class ScrollDragBroadcast : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /*
    public Action OnDragBegin;
    public Action OnDragEnd;
    */

    [SerializeField] private ScrollRect submenuScrollRect = null;
    private SubmenuScroll submenuScroll = null;

    public bool isBeingDragged { get; private set; } = false;

    private void Awake()
    {
        if(submenuScrollRect == null)
        {
            Debug.LogError("SubmenuScroll not set up on GoalsScroll");
            return;
        }
        submenuScroll = submenuScrollRect.GetComponent<SubmenuScroll>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isBeingDragged = true;

        submenuScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        submenuScrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isBeingDragged = false;

        submenuScrollRect.OnEndDrag(eventData);
        submenuScroll.WarpToPosition();
    }

   
}
