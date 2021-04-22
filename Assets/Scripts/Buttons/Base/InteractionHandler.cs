using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class InteractionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private UnityEvent OnTouchEvent;
    [SerializeField] private UnityEvent OnReleaseEvent;
    //  [SerializeField] private UnityEvent OnDragEvent;

    [HideInInspector] public Action OnTouchAction;
    [HideInInspector] public Action OnReleaseAction;


    public void OnPointerDown(PointerEventData eventData)
    {
        print("Down");
        OnTouchEvent?.Invoke();
        OnTouchAction?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("Up");
        OnReleaseEvent?.Invoke();
        OnReleaseAction?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OnDrag?.Invoke();
    }
}
    
