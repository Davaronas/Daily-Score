using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SingleColorPicker : BehaviourButton
{
    

    private GoalManager goalManager = null;
    private Color32[] col;

    private Image image;
    private UIGradient gradient;

   

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        if (goalManager == null)
        {
            Debug.LogError("GoalManager doesn't exist, can't send color");
        }

        // four corner gradient?

        TryGetComponent<UIGradient>(out gradient);
        if(gradient != null)
        {
            print("Gradient: "+ gameObject.name);
            col = new Color32[2];
            col[0] = gradient.m_color1;
            col[1] = gradient.m_color2;
        }
        else
        {
            print("SImple: "+ gameObject.name);
            TryGetComponent<Image>(out image);
            if(image != null)
            {
                col = new Color32[1];
                col[0] = image.color;
            }
        }

       
    }

    protected override void OnTouch()
    {
        if(col.Length == 1)
        {
            goalManager.SetSelectedColor(col[0]);
        }
        else
        {
            goalManager.SetSelectedColor(col);
        }

        
    }


}
