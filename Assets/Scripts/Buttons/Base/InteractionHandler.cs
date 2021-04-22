using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class InteractionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,
                                                   IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UnityEvent OnTouchEvent;
    [SerializeField] private UnityEvent OnReleaseEvent;
    //  [SerializeField] private UnityEvent OnDragEvent;

    [HideInInspector] public Action OnTouchAction;
    [HideInInspector] public Action OnReleaseAction;
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

    public void OnDrag(PointerEventData eventData)
    {
         OnDragAction?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAction?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitAction?.Invoke();
    }
}
    
