using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionHandler))]

public abstract class BehaviourButton : MonoBehaviour
{
    private InteractionHandler interactionHandler = null;

    void Start()
    {
        interactionHandler = GetComponent<InteractionHandler>();
        if (interactionHandler == null) { Debug.LogError($"InteractioHandler is null on {gameObject.name}"); return; }

        interactionHandler.OnTouchAction += OnTouch;
        interactionHandler.OnReleaseAction += OnRelease;
        interactionHandler.OnDragAction += OnDrag;
        interactionHandler.OnPointerEnterAction += OnPointerEnter;
        interactionHandler.OnPointerExitAction += OnPointerExit;

    }

    private void OnDestroy()
    {
        if (interactionHandler == null) { Debug.LogError($"InteractioHandler is null on {gameObject.name}"); return; }

        interactionHandler.OnTouchAction -= OnTouch;
        interactionHandler.OnReleaseAction -= OnRelease;
        interactionHandler.OnDragAction -= OnDrag;
        interactionHandler.OnPointerEnterAction -= OnPointerEnter;
        interactionHandler.OnPointerExitAction -= OnPointerExit;
    }


    virtual protected void OnTouch()
    {

    }

    virtual protected void OnRelease()
    {

    }

    virtual protected void OnDrag()
    {

    }

    virtual protected void OnPointerEnter()
    {

    }

    virtual protected void OnPointerExit()
    {

    }
}
