using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class SablonGoalButton : BehaviourButton
{
    [SerializeField]private GoalPanelScroll createGoalPanelScroll = null;
    [Space]
    [SerializeField] private string goalName = "Default";
    [SerializeField] private Color32 goalColor1 = Color.blue;
    [SerializeField] private Color32 goalColor2 = Color.blue;
    [Space]
    [SerializeField] private Color32 inactiveColor;
    [SerializeField] private Color32 inactiveTextColor;


    [Space]
    [SerializeField] private int goalSpriteId = 0;
    [Space]
    [SerializeField] private List<MaximumTaskData> maxTaskDatas = new List<MaximumTaskData>();
    [SerializeField] private List<MinimumTaskData> minTaskDatas = new List<MinimumTaskData>();
    [SerializeField] private List<BooleanTaskData> boolTaskDatas = new List<BooleanTaskData>();
    [SerializeField] private List<OptimumTaskData> optimumTaskDatas = new List<OptimumTaskData>();
    [SerializeField] private List<IntervalTaskData> intervalTaskDatas = new List<IntervalTaskData>();

    private List<TaskData> tasks = new List<TaskData>();

    private TMP_Text sablonGoalText = null;
    private Image sablonGoalImage = null;
    private GoalManager goalManager = null;

    private GoalData[] goals_ = null;

    private SablonGoalTranslation sgt = null;

    private bool canCreate = true;

    protected override void Start()
    {
        base.Start();
        sablonGoalText = GetComponentInChildren<TMP_Text>();
        sablonGoalImage = GetComponent<Image>();

       
        
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
                sablonGoalText.color = inactiveTextColor;
                sablonGoalImage.color = inactiveColor;
                canCreate = false;
                return;
            }
        }

        sablonGoalImage.color = Color.white;
        sablonGoalText.color = Color.white;
        canCreate = true;
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
                sablonGoalText.color = inactiveTextColor;
                sablonGoalImage.color = inactiveColor;
                canCreate = false;
                return;
            }
        }

        sablonGoalImage.color = Color.white;
        sablonGoalText.color = Color.white;
        canCreate = true;
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
            if (!canCreate) { return; }

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

            if(sgt != null)
            {
                foreach (SablonGoalTranslation.UnitOfMeasureTranslation _t in sgt.unitOfMeasureTranslations)
                {
                    switch (AppManager.currentLanguage)
                    {
                        case AppManager.Languages.English:
                            switch(tasks[_t.taskId].type)
                            {
                                case AppManager.TaskType.Maximum:
                                    ((MaximumTaskData)tasks[_t.taskId]).customMetricName = _t.en_Translation;
                                    break;
                                case AppManager.TaskType.Minimum:
                                    ((MinimumTaskData)tasks[_t.taskId]).customMetricName = _t.en_Translation;
                                    break;
                                case AppManager.TaskType.Boolean:
                                    Debug.LogError("Boolean task type doesn't contain a metric");
                                    break;
                                case AppManager.TaskType.Optimum:
                                   ((OptimumTaskData)tasks[_t.taskId]).customMetricName = _t.en_Translation;
                                    break;
                                case AppManager.TaskType.Interval:
                                    ((IntervalTaskData)tasks[_t.taskId]).customMetricName = _t.en_Translation;
                                    break;

                            }
                           
                                break;
                        case AppManager.Languages.Magyar:
                            switch (tasks[_t.taskId].type)
                            {
                                case AppManager.TaskType.Maximum:
                                    ((MaximumTaskData)tasks[_t.taskId]).customMetricName = _t.hun_Translation;
                                    break;
                                case AppManager.TaskType.Minimum:
                                    ((MinimumTaskData)tasks[_t.taskId]).customMetricName = _t.hun_Translation;
                                    break;
                                case AppManager.TaskType.Boolean:
                                    Debug.LogError("Boolean task type doesn't contain a metric");
                                    break;
                                case AppManager.TaskType.Optimum:
                                    ((OptimumTaskData)tasks[_t.taskId]).customMetricName = _t.hun_Translation;
                                    break;
                                case AppManager.TaskType.Interval:
                                    ((IntervalTaskData)tasks[_t.taskId]).customMetricName = _t.hun_Translation;
                                    break;

                            }
                            break;
                    }
                }
            }

            goalManager.AddGoal(_newGoalData);

            tasks.Clear();
        }
    }

   
}
