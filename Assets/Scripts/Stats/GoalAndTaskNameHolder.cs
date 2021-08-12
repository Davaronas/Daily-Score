using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalAndTaskNameHolder : MonoBehaviour
{
    [SerializeField] private GameObject goalNamePrefab = null;
    [SerializeField] private GameObject taskNamePrefab = null;

    [SerializeField] private SubmenuBroadcaster categorySelectorBroadcaster = null;

    [SerializeField] private TMP_Text selectedCategoryNameText = null;
    [SerializeField] private GameObject panel = null;

    [SerializeField] private RectTransform content = null;
    [SerializeField] private RectTransform scroll = null;

    private GoalManager goalManager = null;

    private bool overallSelected = true;

    private List<GameObject> elements = new List<GameObject>();

    private void Awake()
    {
        AppManager.OnLanguageChanged += UpdateGoalAndTaskNames;
        AppManager.OnTaskEdited += UpdateGoalAndTaskNames;
        AppManager.OnNewGoalAdded += UpdateGoalAndTaskNames;
        AppManager.OnGoalDeleted += UpdateGoalAndTaskNames;
        AppManager.OnNewTaskAdded += UpdateGoalAndTaskNames;

        AppManager.OnBarChartCategorySelected += UpdateName;
        AppManager.OnLanguageChanged += UpdateNameToOverallIfCan;

        goalManager = FindObjectOfType<GoalManager>();

        selectedCategoryNameText.text = RuntimeTranslator.TranslateOverallWord();

        panel.SetActive(false);
    }

  

    private void OnEnable()
    {
        if (elements.Count == 0)
        {
            UpdateGoalAndTaskNames();
        }

       
    }

    private void OnDestroy()
    {
        AppManager.OnLanguageChanged -= UpdateGoalAndTaskNames;
        AppManager.OnTaskEdited -= UpdateGoalAndTaskNames;
        AppManager.OnNewGoalAdded -= UpdateGoalAndTaskNames;
        AppManager.OnGoalDeleted -= UpdateGoalAndTaskNames;
        AppManager.OnNewTaskAdded -= UpdateGoalAndTaskNames;

        AppManager.OnBarChartCategorySelected -= UpdateName;
        AppManager.OnLanguageChanged -= UpdateNameToOverallIfCan;
    }

    private void UpdateGoalAndTaskNames(AppManager.Languages _l)
    {

        //content.offsetMin = new Vector2(scroll.offsetMin.x, 0);
        //content.offsetMax = new Vector2(scroll.offsetMax.x, 0);

        content.offsetMin = new Vector2(0, 0);
        content.offsetMax = new Vector2(0, 0);

        if (elements.Count > 0)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                Destroy(elements[i]);
            }

            elements.Clear();
        }

        GameObject _overallOption = Instantiate(goalNamePrefab, transform.position, Quaternion.identity, transform);
        _overallOption.GetComponent<GoalNameButton>().Overall(categorySelectorBroadcaster);
        elements.Add(_overallOption);

        GoalData[] _goalDatas = goalManager.GetExistingGoals();

        for (int i = 0; i < _goalDatas.Length; i++)
        {
            GameObject _newGoalName = Instantiate(goalNamePrefab, transform.position, Quaternion.identity, transform);
            _newGoalName.GetComponent<GoalNameButton>().SetName(_goalDatas[i].name,categorySelectorBroadcaster);
            elements.Add(_newGoalName);

            for (int j = 0; j < _goalDatas[i].tasks.Count; j++)
            {
                GameObject _newTaskName = Instantiate(taskNamePrefab, transform.position, Quaternion.identity, transform);
                _newTaskName.GetComponent<TaskNameButton>().SetName(_goalDatas[i].tasks[j].name,categorySelectorBroadcaster);
                elements.Add(_newTaskName);
            }
        }
    }

    private void UpdateGoalAndTaskNames()
    {
        UpdateGoalAndTaskNames(AppManager.currentLanguage);
    }

    private void UpdateName(string _n)
    {
        if (_n == "")
        {
            selectedCategoryNameText.text = RuntimeTranslator.TranslateOverallWord();
            overallSelected = true;
        }
        else
        {
            selectedCategoryNameText.text = _n;
            overallSelected = false;
        }
    }

    private void UpdateNameToOverallIfCan(AppManager.Languages _l)
    {
        if(overallSelected)
        {
            selectedCategoryNameText.text = RuntimeTranslator.TranslateOverallWord();
        }
    }

}
