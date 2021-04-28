using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalButton : BehaviourButton
{
    private RectTransform rectTransform;

    private Vector2 lastPosition = Vector2.zero;

    protected override void Start()
    {
        base.Start();

        rectTransform = GetComponent<RectTransform>();
    }

    protected override void OnTouch()
    {
        lastPosition = rectTransform.anchoredPosition;
    }

    protected override void OnRelease()
    {
       // hasonlítsd össze az elõzõvel, és döntsd el melyik akciót akarta a felhasználó csinálni
    }

}
