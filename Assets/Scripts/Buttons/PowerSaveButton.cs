using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSaveButton : BehaviourButton
{
    [SerializeField] private bool isOn = false;
    [SerializeField] private GameObject thisLine = null;
    [SerializeField] private GameObject otherLine = null;
    [SerializeField] private GoalPanelScroll settingsPanelScroll = null;


    private void Awake()
    {

        CheckStatus();

        AppManager.OnPowerSavingModeChanged += CheckStatus;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        AppManager.OnPowerSavingModeChanged -= CheckStatus;
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
            AppManager.SetPowerSavingMode(isOn);


            SoundManager.PlaySound2();
        }
    }



    private void CheckStatus()
    {
        if (isOn)
        {
            if (PlayerPrefs.GetInt("PowerSaving", 0) == 0)
            {
                thisLine.SetActive(false);
            }
            else
            {
                thisLine.SetActive(true);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("PowerSaving", 0) == 1)
            {
                thisLine.SetActive(false);
            }
            else
            {
                thisLine.SetActive(true);
            }
        }
    }
}
