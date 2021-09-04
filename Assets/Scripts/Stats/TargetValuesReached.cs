using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetValuesReached : MonoBehaviour
{

    private GoalManager goalManager = null;
    [SerializeField] private PieChart targetValuesReachedPieChart = null;

    [SerializeField] private Color reachedColor1 = Color.green;
    [SerializeField] private Color notReachedColor1 = Color.red;
    [SerializeField] private Color reachedColor2 = Color.green;
    [SerializeField] private Color notReachedColor2 = Color.red;


    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
    }

    void Start()
    {
        Invoke(nameof(CalculateTargetValuesReached), 2f);
    }


    public void CalculateTargetValuesReached()
    {

        targetValuesReachedPieChart.Clear();
        GoalData[] _goalDatas = goalManager.GetExistingGoals();

        int _reached = 0;
        int _notReached = 0;

        for (int i = 0; i < _goalDatas.Length; i++)
        {
            for (int j = 0; j < _goalDatas[i].dailyScores.Count; j++)
            {
                for (int l = 0; l < _goalDatas[i].dailyScores[j].targetsReached.Length; l++)
                {
                    if (_goalDatas[i].dailyScores[j].targetsReached[l] == true)
                    {
                        _reached++;
                    }
                    else
                    {
                        _notReached++;
                    }
                }
            }
        }

        PieChartInfo _reachedInfo = new PieChartInfo(_reached, "", reachedColor1,reachedColor2);
        PieChartInfo _notReachedInfo = new PieChartInfo(_notReached, "", notReachedColor1,notReachedColor2);

        targetValuesReachedPieChart.LoadData(new PieChartInfo[] { _reachedInfo, _notReachedInfo }, "", true);

    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            CalculateTargetValuesReached();
        }
    }
}
