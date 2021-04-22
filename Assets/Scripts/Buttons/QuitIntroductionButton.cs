using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuitIntroductionButton : BehaviourButton
{
   
    protected override void OnTouch()
    {
        print("Down2");
    }

    protected override void OnRelease()
    {
        print("Up2");
        // Go to main menu

        // Save: first started the application
    }
}
