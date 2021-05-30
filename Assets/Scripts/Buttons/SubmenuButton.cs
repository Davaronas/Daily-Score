using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SubmenuButton : BehaviourButton
{
    [SerializeField] private int buttonId = 0;

   [SerializeField] private SubmenuScroll submenuScroll = null;

   [SerializeField] private Image image = null;

    

   




  

    public int GetId()
    {
        return buttonId;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        submenuScroll = FindObjectOfType<SubmenuScroll>();


        AppManager.OnSubmenuButtonPressed += SubmenuChanged;
        AppManager.OnSubmenuChangedViaScrolling += SubmenuChanged;

        // print(image + " " + submenuScroll);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    
    
        AppManager.OnSubmenuButtonPressed -= SubmenuChanged;
        AppManager.OnSubmenuChangedViaScrolling -= SubmenuChanged;
    }

    protected override void OnTouch()
    {
        SubmenuChanged(buttonId);
        submenuScroll.WarpToPosition(buttonId);
        AppManager.SubmenuButtonPressed(buttonId);
    }

    public void Selected()
    {
        image.color = Color.gray;
    }

    public void Deselected()
    {

        image.color = Color.white;
    }

    private void SubmenuChanged(int _id)
    {




        // this means awake was not called first, this should never run, but just in case
        if (image == null || submenuScroll == null)
        {
            print(buttonId);
            Debug.LogError($"This event was not supposed to be called" +
               $" before the Awake of the main menu: {nameof(SubmenuChanged)} ");
            return;
        }



        if (_id == buttonId) 
        {
            Selected();
        }
        else
        {
            Deselected();
        }



        
    }
}
