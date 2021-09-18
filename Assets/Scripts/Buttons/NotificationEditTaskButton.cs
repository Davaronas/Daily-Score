using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationEditTaskButton : BehaviourButton
{
    [SerializeField] private GoalPanelScroll settingsPanelScroll = null;

   [SerializeField] private SettingsNotificationPrefabUtility snpu = null;



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
            snpu.EditTask();
        }
    }
}
