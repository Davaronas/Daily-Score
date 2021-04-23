using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class InteractionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,
                                                   IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private  UnityEvent OnTouchEvent;
    [SerializeField] private UnityEvent OnReleaseEvent;
    //  [SerializeField] private UnityEvent OnDragEvent;

    [HideInInspector] public event Action OnTouchAction;
    [HideInInspector] public event Action OnReleaseAction;
    [HideInInspector] public event Action OnDragAction;
    [HideInInspector] public event Action OnPointerEnterAction;
    [HideInInspector] public event Action OnPointerExitAction;



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
    
