using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTimeButton : BehaviourButton
{
    public bool isRewind = false;
    [SerializeField] private RewindTimeHandler rth = null;

    protected override void OnTouch()
    {
        if(isRewind)
        {
            rth.RewindBack();
            print("ANYÁD");
        }
        else
        {
            rth.GoForward();
        }
    }

    protected override void OnRelease()
    {
       
    }
}
