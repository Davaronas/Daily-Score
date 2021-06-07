using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropdown : MonoBehaviour
{
    bool isOn;
    [SerializeField] GameObject onStateObj;
    [SerializeField] GameObject offStateObj;

    private void OnEnable()
    {
        isOn = false;
        onStateObj.SetActive(false);
        //offStateObj.SetActive(true);
    }

    public void OnToggleButtonClick()
    {
        if (isOn)
        {
            // make this off
            isOn = false;
            onStateObj.SetActive(false);
            offStateObj.SetActive(true);
        }
        else
        {
            // make this on
            isOn = true;
            onStateObj.SetActive(true);
            offStateObj.SetActive(false);
        }
    }
}
