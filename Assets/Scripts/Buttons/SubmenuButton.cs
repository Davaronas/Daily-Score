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

    

    public SubmenuButton()
    {
       // AppManager.OnSubmenuButtonPressed += SubmenuChanged; Nullreferenciát ad az egész gameobjectre. ?????
    }

    

    private void OnDestroy()
    {
        AppManager.OnSubmenuButtonPressed -= SubmenuChanged;
    }

  

    public int GetId()
    {
        return buttonId;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        submenuScroll = FindObjectOfType<SubmenuScroll>();
        AppManager.OnSubmenuButtonPressed += SubmenuChanged; // Míért csak itt mûködik ?

        // print(image + " " + submenuScroll);
    }

    protected override void OnTouch()
    {
        Selected();
        submenuScroll.WarpToPosition(buttonId);
        AppManager.SubmenuButtonPressed(buttonId);
    }

    public void Selected()
    {
        image.color = Color.gray;
    }

    public void Deselected()
    {
        /*
        if (image == null || submenuScroll == null)
        {
            print(buttonId);
            Debug.LogError($"This event was not supposed to be called" +
               $" before the Awake of the main menu: {nameof(SubmenuChanged)} ");
            Debug.LogWarning(image == null);
            Debug.LogWarning(submenuScroll == null);
            return;
        }
        */




        image.color = Color.white;
    }

    private void SubmenuChanged(int _id)
    {
        

       

        // this means awake was not called first, this should never run, but just in case
        



        if (_id == buttonId) 
        {
            return; 
        }




        Deselected();
    }
}
