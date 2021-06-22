using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SingleColorPicker : BehaviourButton
{
    

    private GoalManager goalManager = null;
    private Color32[] col;

    private Image image;
    private ImageColoring gradient;


    private RectTransform rectTransform = null;

    private Vector2 originalSize;

    [SerializeField] private float animationSpeed = 0.5f;
    [SerializeField] [Range(0, 100)] private int sizePercentIncrease = 20;


    private void Awake()
    {

        AppManager.OnGoalColorPicked += OnColorPicked;

        goalManager = FindObjectOfType<GoalManager>();
        if (goalManager == null)
        {
            Debug.LogError("GoalManager doesn't exist, can't send color");
        }

        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;

        // four corner gradient?

        TryGetComponent<ImageColoring>(out gradient);
        if(gradient != null)
        {
            col = new Color32[2];
            col[0] = gradient.color1;
            col[1] = gradient.color2;
        }
        else
        {
            TryGetComponent<Image>(out image);
            if(image != null)
            {
                col = new Color32[1];
                col[0] = image.color;
            }
        }
    }

    protected override void OnDestroy()
    {
        AppManager.OnGoalColorPicked -= OnColorPicked;
    }

    private void OnDisable()
    {
        rectTransform.sizeDelta = originalSize;
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

        AppManager.GoalColorPicked(this);
    }

    private void OnColorPicked(SingleColorPicker _scp)
    {
        if (_scp == this)
        {
            LT_Animator.SizeTransition(rectTransform, originalSize + new Vector2(originalSize.x * ((float)sizePercentIncrease / 100), originalSize.y * ((float)sizePercentIncrease / 100)), animationSpeed);
         //   LT_Animator.RotateAround(rectTransform, 0.2f);
        }
        else
        {
            LT_Animator.SizeTransition(rectTransform, originalSize, animationSpeed);
        }
    }


}
