using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnButton : BehaviourButton
{
    [SerializeField] private GoalPanelScroll settingsPanelScroll = null;
    [SerializeField] private bool on = true;
    private SoundManager soundManager = null;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }


    protected override void OnTouch()
    {
        if (!Application.isEditor)
        {
            settingsPanelScroll.FeedClickPosition(Input.GetTouch(0).position);
        }
        else
        {
            settingsPanelScroll.FeedClickPosition(Input.mousePosition);
        }
    }

    protected override void OnRelease()
    {
        Invoke(nameof(CheckIfDragging), Time.deltaTime);
    }

    private void CheckIfDragging()
    {

        if (settingsPanelScroll.allowInteraction)
        {
            if (on)
            {
                soundManager.RemoteCall_TurnOnAudio();
            }
            else
            {
                soundManager.RemoteCall_TurnOffAudio();
            }
        }
    }
}
