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

    }

    private void OnDestroy()
    {
        if (interactionHandler == null) { Debug.LogError($"InteractioHandler is null on {gameObject.name}"); return; }

        interactionHandler.OnTouchAction -= OnTouch;
        interactionHandler.OnReleaseAction -= OnRelease;
    }


    virtual protected void OnTouch()
    {

    }

    virtual protected void OnRelease()
    {

    }
}
