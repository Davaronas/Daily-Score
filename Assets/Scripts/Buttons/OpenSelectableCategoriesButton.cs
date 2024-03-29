using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSelectableCategoriesButton : BehaviourButton
{
    [SerializeField] private GameObject selectablesPanel = null;


    private void Awake()
    {
        AppManager.OnBarChartCategorySelected += ChangeStateCallback;
        AppManager.OnSubmenuChangedViaScrolling += Deactivate;
        AppManager.OnSubmenuButtonPressed += Deactivate;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        AppManager.OnBarChartCategorySelected -= ChangeStateCallback;
        AppManager.OnSubmenuChangedViaScrolling -= Deactivate;
        AppManager.OnSubmenuButtonPressed -= Deactivate;
    }

    protected override void OnTouch()
    {
        ChangeState();

        SoundManager.PlaySound2();
    }

    private void ChangeStateCallback(string _n, CategorySelectableBarCharts _c)
    {
        InvokeChangeState();
    }

    private void InvokeChangeState()
    {
        Invoke(nameof(ChangeState), 0.05f);
    }

    private void ChangeState()
    {
        selectablesPanel.SetActive(!selectablesPanel.activeSelf);
    }

    private void Deactivate(int _i)
    {
        if(_i != 2)
        selectablesPanel.SetActive(false);
    }

}
