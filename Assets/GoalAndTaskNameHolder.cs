using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAndTaskNameHolder : MonoBehaviour
{
    [SerializeField] private GameObject goalNamePrefab = null;
    [SerializeField] private GameObject taskNamePrefab = null;

    private GoalManager goalManager = null;

    private List<GameObject> elements = new List<GameObject>();

    private void Awake()
    {
        AppManager.OnLanguageChanged += UpdateGoalAndTaskNames;
        AppManager.OnTaskEdited += UpdateGoalAndTaskNames;
        AppManager.OnNewGoalAdded += UpdateGoalAndTaskNames;

        goalManager = FindObjectOfType<GoalManager>();
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
    }

    private void UpdateGoalAndTaskNames(AppManager.Languages _l)
    {

        if (elements.Count > 0)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                Destroy(elements[i]);
            }

            elements.Clear();
        }

        GameObject _overallOption = Instantiate(goalNamePrefab, transform.position, Quaternion.identity, transform);
        _overallOption.GetComponent<GoalNameButton>().Overall();
        elements.Add(_overallOption);

        GoalData[] _goalDatas = goalManager.GetExistingGoals();

        for (int i = 0; i < _goalDatas.Length; i++)
        {
            GameObject _newGoalName = Instantiate(goalNamePrefab, transform.position, Quaternion.identity, transform);
            _newGoalName.GetComponent<GoalNameButton>().SetName(_goalDatas[i].name);
            elements.Add(_newGoalName);

            for (int j = 0; j < _goalDatas[i].tasks.Count; j++)
            {
                GameObject _newTaskName = Instantiate(taskNamePrefab, transform.position, Quaternion.identity, transform);
                _newTaskName.GetComponent<TaskNameButton>().SetName(_goalDatas[i].tasks[j].name);
                elements.Add(_newTaskName);
            }
        }
    }

    private void UpdateGoalAndTaskNames()
    {
        UpdateGoalAndTaskNames(AppManager.currentLanguage);
    }

}
