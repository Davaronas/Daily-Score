using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SymbolPicker : BehaviourButton
{
    public int spriteId = -1;
    private GoalManager goalManager = null;

    private RectTransform rectTransform = null;

    private Vector2 originalSize;

    [SerializeField] private float animationSpeed = 0.5f;
    [SerializeField] [Range(0, 100)] private int sizePercentIncrease = 20;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        if (goalManager == null)
        {
            Debug.LogError("GoalManager doesn't exist");
        }

        AppManager.OnGoalSymbolPicked += OnSymbolPicked;

        rectTransform = GetComponent<RectTransform>();
        
    }

    protected override void Start()
    {
        base.Start();
        originalSize = rectTransform.sizeDelta;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AppManager.OnGoalSymbolPicked -= OnSymbolPicked;
    }

    protected override void OnTouch()
    {
        goalManager.SetSpriteId(spriteId);

        SoundManager.PlaySound2();

        AppManager.GoalSymbolPicked(this);
    }

    private void OnDisable()
    {
        rectTransform.sizeDelta = originalSize;
    }

    private void OnSymbolPicked(SymbolPicker _sp)
    {
        if(_sp == this)
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
