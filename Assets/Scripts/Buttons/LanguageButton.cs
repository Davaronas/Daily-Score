using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LanguageButton : BehaviourButton
{
    [SerializeField] private AppManager.Languages language;

    protected override void OnTouch()
    {
        
    }

    protected override void OnRelease()
    {

        AppManager.SetLanguage(language);

        // Go to introduction screen
    }
}
