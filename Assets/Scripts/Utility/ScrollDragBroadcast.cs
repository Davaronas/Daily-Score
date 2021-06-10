using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(ScrollRect))]
public abstract class ScrollDragBroadcast : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /*
    public Action OnDragBegin;
    public Action OnDragEnd;
    */
    public bool isBeingDragged { get; private set; } = false;




    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isBeingDragged = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
       
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isBeingDragged = false;
    }

   
}
