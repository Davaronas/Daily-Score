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

    private Color activeColor = Color.white;
    private Color inactiveColor = new Color(255, 255, 255,80);

    private void Awake()
    {
        statCalc = FindObjectOfType<StatisticCalculator2>();
        statManager = FindObjectOfType<StatManager>();

        forwardButton.color = inactiveColor;

        if (rc == RewindableCharts.OverallBarChart)
        {
            if (!statCalc.CanRewindBarChartTop3(0, 0))
            {
                rewindButton.color = inactiveColor;
            }
        }
        else if(rc == RewindableCharts.ContributionPieChart)
        {

            if (!statCalc.CanRewindPieChart(0, 1))
            {
                rewindButton.color = inactiveColor;
            }


        }

    }

    public void RewindBack()
    {
        if (rc == RewindableCharts.OverallBarChart)
        {
            if (statCalc.CanRewindBarChartOverall(statManager.BarChartValue(), rewind))
            {
                rewind--;
                statManager.RewindChangedBarChart(rewind);
                forwardButton.color = activeColor;
                print(statManager.BarChartValue());

                SoundManager.PlaySound3();
            }



            if (statCalc.CanRewindBarChartOverall(statManager.BarChartValue(), rewind))
            {
                rewindButton.color = activeColor;
            }
            else
            {
                rewindButton.color = inactiveColor;
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
                    forwardButton.color = activeColor;

                    SoundManager.PlaySound3();
                }


                if (statCalc.CanRewindPieChart(statManager.PieChartValue(), rewind))
                {
                    rewindButton.color = activeColor;
                }
                else
                {
                    rewindButton.color = inactiveColor;
                }
            }
            else
            {
                rewindButton.color = inactiveColor;
            }
        }
        else if(rc == RewindableCharts.Top3BarChart)
        {
            if (statManager.BarChartTop3Value() != 3)
            {
                if (statCalc.CanRewindBarChartTop3(statManager.BarChartTop3Value(), rewind))
                {
                    rewind--;
                    statManager.RewindChangedBarChartTop3(rewind);
                    forwardButton.color = activeColor;

                    SoundManager.PlaySound3();
                }

                if (statCalc.CanRewindBarChartTop3(statManager.BarChartTop3Value(), rewind))
                {
                    rewindButton.color = activeColor;
                }
                else
                {
                    rewindButton.color = inactiveColor;
                }
            }
            else
            {
                rewindButton.color = inactiveColor;
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
            forwardButton.color = inactiveColor;
        }
        else
        {
            rewindButton.color = activeColor;
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
            if (!statCalc.CanRewindBarChartTop3(0, 0))
            {
                rewindButton.color = inactiveColor;
            }
            else
            {
                rewindButton.color = activeColor;
            }
        }
        else if(rc == RewindableCharts.ContributionPieChart)
        {
            if(!statCalc.CanRewindPieChart(0,0))
            {
                rewindButton.color = inactiveColor;
            }
            else
            {
                rewindButton.color = activeColor;
            }
        }
        

        forwardButton.color = inactiveColor;
    }

    public void DisableButtons()
    {
        rewindButton.color = inactiveColor;
        forwardButton.color = inactiveColor;

    }

    public int GetCurrentRewind()
    {
        return rewind;
    }
}
