using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SubmenuButton : BehaviourButton
{
    [SerializeField] private int buttonId = 0;
    [Space]
    [SerializeField] private float sizeChangeAnimationSpeed = 1;
    [SerializeField] [Range(0, 100)] private int percentSizeIncreaseWhenSelected = 10;
    [Space]
    [SerializeField] private float colorChangeAnimationSpeed = 5;
    [SerializeField] private Color deselectedColor;
    [SerializeField] private Color selectedColor;

    private SubmenuScroll submenuScroll = null;

    private Image image = null;
    private RectTransform rectTransform;


    private Transform lineUnderButton;



    private Vector2 rt_originalSize;

  

    public int GetId()
    {
        return buttonId;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        rt_originalSize = rectTransform.sizeDelta;
        submenuScroll = FindObjectOfType<SubmenuScroll>();
        lineUnderButton = transform.Find("Line");


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
        lineUnderButton.gameObject.SetActive(false);
        LT_Animator.ColorTransition(rectTransform,selectedColor, colorChangeAnimationSpeed);
        LT_Animator.SizeTransition(rectTransform, rt_originalSize + new Vector2(0, rt_originalSize.y * ((float)percentSizeIncreaseWhenSelected/100)), sizeChangeAnimationSpeed);

    }

    public void Deselected()
    {
        lineUnderButton.gameObject.SetActive(true);
        LT_Animator.ColorTransition(rectTransform,deselectedColor, colorChangeAnimationSpeed);
        LT_Animator.SizeTransition(rectTransform, rt_originalSize, sizeChangeAnimationSpeed);
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
