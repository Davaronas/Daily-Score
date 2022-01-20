using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SablonGoalButton : BehaviourButton
{
    [SerializeField]private GoalPanelScroll createGoalPanelScroll = null;
    [Space]
    [SerializeField] private string goalName = "Default";
    [SerializeField] private Color32 goalColor1 = Color.blue;
    [SerializeField] private Color32 goalColor2 = Color.blue;
    [SerializeField] private int goalSpriteId = 0;
    [Space]
    [SerializeField] private List<MaximumTaskData> maxTaskDatas = new List<MaximumTaskData>();
    [SerializeField] private List<MinimumTaskData> minTaskDatas = new List<MinimumTaskData>();
    [SerializeField] private List<BooleanTaskData> boolTaskDatas = new List<BooleanTaskData>();
    [SerializeField] private List<OptimumTaskData> optimumTaskDatas = new List<OptimumTaskData>();
    [SerializeField] private List<IntervalTaskData> intervalTaskDatas = new List<IntervalTaskData>();

    private List<TaskData> tasks = new List<TaskData>();

    private TMP_Text sablonGoalText = null;
    private GoalManager goalManager = null;

    private GoalData[] goals_ = null;

    private SablonGoalTranslation sgt = null;

    protected override void Start()
    {
        base.Start();
        sablonGoalText = GetComponentInChildren<TMP_Text>();

       
        
        sablonGoalText.text = goalName;

        goalManager = FindObjectOfType<GoalManager>();
        sgt = GetComponent<SablonGoalTranslation>();

        AppManager.OnNewGoalAdded += CheckGoalVisibility;
        AppManager.OnGoalDeleted += CheckGoalVisibility;
        AppManager.OnLanguageChanged += CheckGoalVisibility;

        Invoke(nameof(CheckGoalVisibility),0.5f);
    }



    protected override void OnDestroy()
    {
        base.OnDestroy();

        AppManager.OnNewGoalAdded -= CheckGoalVisibility;
        AppManager.OnGoalDeleted -= CheckGoalVisibility;
        AppManager.OnLanguageChanged -= CheckGoalVisibility;
    }

    private void CheckGoalVisibility()
    {
        goals_ = goalManager.GetExistingGoals();

        if (sgt != null)
        {
            switch (AppManager.currentLanguage)
            {
                case AppManager.Languages.English:
                    goalName = sgt.goalName[0];
                    break;
                case AppManager.Languages.Magyar:
                    goalName = sgt.goalName[1];
                    break;
                default:

                    break;
            }

            sablonGoalText.text = goalName;
        }

        for (int i = 0; i < goals_.Length; i++)
        {

            if (goals_[i].name == goalName)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        gameObject.SetActive(true);
    }

    private void CheckGoalVisibility(AppManager.Languages _l)
    {
        goals_ = goalManager.GetExistingGoals();
       // print(goals_.Length + " Length");

        if (sgt != null)
        {
            switch (AppManager.currentLanguage)
            {
                case AppManager.Languages.English:
                    goalName = sgt.goalName[0];
                    break;
                case AppManager.Languages.Magyar:
                    goalName = sgt.goalName[1];
                    break;
                default:

                    break;
            }

            sablonGoalText.text = goalName;
        }

        for (int i = 0; i < goals_.Length; i++)
        {

            if (goals_[i].name == goalName)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        gameObject.SetActive(true);
    }


    protected override void OnTouch()
    {

        if (!Application.isEditor)
        {
            createGoalPanelScroll.FeedClickPosition(Input.GetTouch(0).position);
        }
        else
        {
            createGoalPanelScroll.FeedClickPosition(Input.mousePosition);
        }

      

    }



    protected override void OnRelease()
    {
        Invoke(nameof(CheckIfDragging), Time.deltaTime);
    }

    private void CheckIfDragging()
    {

        if (createGoalPanelScroll.allowInteraction)
        {
            GoalColor[] _goalColors = new GoalColor[2];
            _goalColors[0] = goalColor1;
            _goalColors[1] = goalColor2;

            GoalData _newGoalData = new GoalData(goalName, _goalColors, goalSpriteId);

            foreach (MaximumTaskData _td in maxTaskDatas)
            {
                tasks.Add(_td);
            }

            foreach (MinimumTaskData _td in minTaskDatas)
            {
                tasks.Add(_td);
            }

            foreach (BooleanTaskData _td in boolTaskDatas)
            {
                tasks.Add(_td);
            }

            foreach (OptimumTaskData _td in optimumTaskDatas)
            {
                tasks.Add(_td);
            }

            foreach (IntervalTaskData _td in intervalTaskDatas)
            {
                tasks.Add(_td);
            }


            int taskLanguageIterator = 0;
            foreach (TaskData _td in tasks)
            {
                if (sgt != null)
                {

                    switch(AppManager.currentLanguage)
                    {
                        case AppManager.Languages.English:
                            _td.name = sgt.englishTaskNames[taskLanguageIterator];
                            break;
                        case AppManager.Languages.Magyar:
                            _td.name = sgt.hungarianTaskNames[taskLanguageIterator];
                            break;
                    }

                    taskLanguageIterator++;
                }


                _td.owner = _newGoalData.name;

                if (_td.beingActiveType == TaskData.ActiveType.DayOfWeek)
                {
                    for (int i = 0; i < _td.activeOnDays.Count; i++)
                    {

                        if (_td.activeOnDays[i] == (int)DateTime.Today.DayOfWeek)
                        {
                            _td.isActiveToday = true;
                        }
                    }
                }
                else
                {
                    _td.isActiveToday = true;
                }
                


                _newGoalData.tasks.Add(_td);
            }

            goalManager.AddGoal(_newGoalData);

            tasks.Clear();
        }
    }

   
}
