using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingAverage : MonoBehaviour
{

    private StatisticCalculator2 statCalc = null;
    private BarChartHolder barChart = null;
  [SerializeField]  private LineDrawer lineDrawer = null;
    private Vector2[] positions;

    private void Awake()
    {
        statCalc = FindObjectOfType<StatisticCalculator2>();
        barChart = GetComponent<BarChartHolder>();

        AppManager.OnBarChartCategorySelected += UpdateRollingAverageLineGraph;

        Invoke(nameof(StartRollingAverageChart), 1f);
    }


    private void OnDestroy()
    {
        AppManager.OnBarChartCategorySelected -= UpdateRollingAverageLineGraph;
    }


    private void UpdateRollingAverageLineGraph(string _n,CategorySelectableBarCharts _type)
    {
        if(_type == CategorySelectableBarCharts.RollingAverage)
        {
            positions = barChart.GetRollingAverageEndPoints();
            Invoke(nameof(Calculate), Time.deltaTime*3);
        }
    }

    public void Calculate()
    {
        

        
        lineDrawer.UpdatePositions(positions);
    }

    private void StartRollingAverageChart()
    {
       statCalc.RollingAverageBarChartCalculation();

        positions = barChart.GetRollingAverageEndPoints();
        lineDrawer.UpdatePositions(positions);
    }

    public Vector2[] GetPositions()
    {
        if (barChart != null)
        {
            return barChart.GetRollingAverageEndPoints();
        }
        else
        {
            return new Vector2[] { };
        }
    }

}
