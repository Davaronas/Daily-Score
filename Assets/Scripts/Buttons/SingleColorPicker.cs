using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SingleColorPicker : BehaviourButton
{
    public bool setColorRuntime = false;
    public Color32 col = new Color32();

    private GoalManager goalManager = null;


   

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        if (goalManager == null)
        {
            Debug.LogError("GoalManager doesn't exist, can't send color");
        }

        if (setColorRuntime)
        {
            if(TryGetComponent<Image>(out Image _image))
            {
                col = _image.color;
            }
        }
    }

    protected override void OnTouch()
    {
        goalManager.SetSelectedColor(col);
    }


}
