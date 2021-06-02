using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[DisallowMultipleComponent]
public class InteractionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, // IDragHandler,
                                                   IPointerEnterHandler, IPointerExitHandler //IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private  UnityEvent OnTouchEvent;
    [SerializeField] private UnityEvent OnReleaseEvent;
    //  [SerializeField] private UnityEvent OnDragEvent;

    [HideInInspector] public Action OnTouchAction;
    [HideInInspector] public Action OnReleaseAction;
    [HideInInspector] public Action OnBeginDragAction;
    [HideInInspector] public Action OnEndDragAction;
    [HideInInspector] public Action OnDragAction;
    [HideInInspector] public Action OnPointerEnterAction;
    [HideInInspector] public Action OnPointerExitAction;



    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouchEvent?.Invoke();
        OnTouchAction?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
        OnReleaseEvent?.Invoke();
        OnReleaseAction?.Invoke();
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAction?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitAction?.Invoke();
    }

    /*
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragAction?.Invoke();
    }
    */

    /*
    public void OnDrag(PointerEventData eventData)
    {
        OnDragAction?.Invoke();
    }
    */

    /*
    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragAction?.Invoke();
    }
    */

   
}
    
