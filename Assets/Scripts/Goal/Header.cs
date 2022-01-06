using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Header : MonoBehaviour
{

   [SerializeField] private StarHandler starHandler = null;
   [SerializeField] private TargetValuesReachedBar targetReachedBar = null;

    private GoalManager goalManager = null;

    private float percent = 0;

    private void Awake()
    {
        AppManager.OnTaskValueChanged += TaskValueChangedCallback;
        AppManager.OnNewTaskAdded += TaskValueChangedCallback;
        AppManager.OnTaskEdited += TaskValueChangedCallback;
       // AppManager.OnLanguageChanged += TaskValueChangedCallback;
        AppManager.OnGoalDeleted += TaskValueChangedCallback;
        goalManager = FindObjectOfType<GoalManager>();
    }

    private void Start()
    {
        Invoke(nameof(InvokeStart), 1f);
    }

    private void InvokeStart()
    {
        percent = GetPercent();
        SetHeaderParts(percent);
    }

    private void OnDestroy()
    {
        AppManager.OnTaskValueChanged -= TaskValueChangedCallback;
        AppManager.OnGoalDeleted -= TaskValueChangedCallback;
        AppManager.OnNewTaskAdded -= TaskValueChangedCallback;
        AppManager.OnTaskEdited -= TaskValueChangedCallback;
       // AppManager.OnLanguageChanged -= TaskValueChangedCallback;
    }

    private void TaskValueChangedCallback(TaskData _td)
    {
        percent = GetPercent();
        SetHeaderParts(percent);
    }

    private void TaskValueChangedCallback(AppManager.Languages _l)
    {
        percent = GetPercent();
        SetHeaderParts(percent);
    }

    private void TaskValueChangedCallback()
    {
        percent = GetPercent();
        SetHeaderParts(percent);
    }

    private float GetPercent()
    {
        GoalData[] _gds = goalManager.GetExistingGoals();
        int _true = 0;
        int _false = 0;
        for (int i = 0; i < _gds.Length; i++)
        {

            bool[] _thisGoalTargetsReached = TaskPointCalculator.GetTargetValuesReached(_gds[i]);
            for (int j = 0; j < _thisGoalTargetsReached.Length; j++)
            {
                if (_thisGoalTargetsReached[j] == true)
                {
                    _true++;
                }
                else
                {
                    _false++;
                }
            }
        }

        if (_true + _false == 0)
        {
            return 0;
        }
        else
        {
            return (float)_true / (_true + _false);
        }
    }

    private void SetHeaderParts(float _p)
    {
        starHandler.AchievedStars(TaskPointCalculator.GetStarAmountFromPercent(_p));
        targetReachedBar.ChangePercent(_p);
    }

}
