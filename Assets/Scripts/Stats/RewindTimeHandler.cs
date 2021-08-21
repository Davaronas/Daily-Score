using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RewindableCharts { OverallBarChart, ContributionPieChart, Top3BarChart}


public class RewindTimeHandler : MonoBehaviour
{
    public int rewind = 0;
    private StatisticCalculator2 statCalc = null;
    private StatManager statManager = null;

    [SerializeField] private Image rewindButton = null;
    [SerializeField] private Image forwardButton = null;
    [SerializeField] private RewindableCharts rc;

    private void Awake()
    {
        statCalc = FindObjectOfType<StatisticCalculator2>();
        statManager = FindObjectOfType<StatManager>();

        forwardButton.color = Color.grey;

        if (rc == RewindableCharts.OverallBarChart)
        {
            if (!statCalc.CanRewindBarChart(0, 0))
            {
                rewindButton.color = Color.grey;
            }
        }
        else if(rc == RewindableCharts.ContributionPieChart)
        {

            if (!statCalc.CanRewindPieChart(0, 1))
            {
                rewindButton.color = Color.grey;
            }


        }

    }

    public void RewindBack()
    {
        if (rc == RewindableCharts.OverallBarChart)
        {
            if (statCalc.CanRewindBarChart(statManager.BarChartValue(), rewind))
            {
                rewind--;
                statManager.RewindChangedBarChart(rewind);
                forwardButton.color = Color.white;

                SoundManager.PlaySound3();
            }



            if (statCalc.CanRewindBarChart(statManager.BarChartValue(), rewind))
            {
                rewindButton.color = Color.white;
            }
            else
            {
                rewindButton.color = Color.grey;
            }
        }
        else if (rc == RewindableCharts.ContributionPieChart)
        {
            if (statManager.PieChartValue() != 3)
            {

                if (statCalc.CanRewindPieChart(statManager.PieChartValue(), rewind))
                {
                    rewind--;
                    statManager.RewindChangedPieChart(rewind);
                    forwardButton.color = Color.white;

                    SoundManager.PlaySound3();
                }


                if (statCalc.CanRewindPieChart(statManager.PieChartValue(), rewind))
                {
                    rewindButton.color = Color.white;
                }
                else
                {
                    rewindButton.color = Color.grey;
                }
            }
            else
            {
                rewindButton.color = Color.grey;
            }
        }
        else if(rc == RewindableCharts.Top3BarChart)
        {
            if (statManager.BarChartTop3Value() != 3)
            {
                if (statCalc.CanRewindBarChart(statManager.BarChartTop3Value(), rewind))
                {
                    rewind--;
                    statManager.RewindChangedBarChartTop3(rewind);
                    forwardButton.color = Color.white;

                    SoundManager.PlaySound3();
                }

                if (statCalc.CanRewindBarChart(statManager.BarChartTop3Value(), rewind))
                {
                    rewindButton.color = Color.white;
                }
                else
                {
                    rewindButton.color = Color.grey;
                }
            }
            else
            {
                rewindButton.color = Color.grey;
            }
        }

    }

    public void GoForward()
    {

        if (rewind == 0)
        {
            return;
        }

        rewind++;

        if (rewind == 0)
        {
            forwardButton.color = Color.grey;
        }
        else
        {
            rewindButton.color = Color.white;
        }


        if (rc == RewindableCharts.OverallBarChart)
        {
            statManager.RewindChangedBarChart(rewind);
        }
        else if (rc == RewindableCharts.ContributionPieChart)
        {
            statManager.RewindChangedPieChart(rewind);
        }
        else if(rc == RewindableCharts.Top3BarChart)
        {
            statManager.RewindChangedBarChartTop3(rewind);
        }

        SoundManager.PlaySound3();
    }

    public void ResetButtons()
    {
        rewind = 0;

        if (rc == RewindableCharts.OverallBarChart || rc == RewindableCharts.Top3BarChart)
        {
            if (!statCalc.CanRewindBarChart(0, 0))
            {
                rewindButton.color = Color.grey;
            }
            else
            {
                rewindButton.color = Color.white;
            }
        }
        else if(rc == RewindableCharts.ContributionPieChart)
        {
            if(!statCalc.CanRewindPieChart(0,0))
            {
                rewindButton.color = Color.grey;
            }
            else
            {
                rewindButton.color = Color.white;
            }
        }
        

        forwardButton.color = Color.grey;
    }

    public void DisableButtons()
    {
        rewindButton.color = Color.grey;
        forwardButton.color = Color.grey;

    }

    public int GetCurrentRewind()
    {
        return rewind;
    }
}
