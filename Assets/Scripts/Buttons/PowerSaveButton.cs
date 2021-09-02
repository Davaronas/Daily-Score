using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSaveButton : BehaviourButton
{
    [SerializeField] private bool isOn = false;
    [SerializeField] private GameObject thisLine = null;
    [SerializeField] private GameObject otherLine = null;



    private void Awake()
    {

        CheckStatus();

        AppManager.onPowerSavingModeChanged += CheckStatus;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        AppManager.onPowerSavingModeChanged -= CheckStatus;
    }

    protected override void OnTouch()
    {
        AppManager.SetPowerSavingMode(isOn);
        

        SoundManager.PlaySound2();
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
