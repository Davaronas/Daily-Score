using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindTimeHandler : MonoBehaviour
{
    public int rewind = 0;
    private StatisticCalculator2 statCalc = null;
    private StatManager statManager = null;

   [SerializeField] private Image rewindButton = null;
   [SerializeField] private Image forwardButton = null;

    private void Awake()
    {
        statCalc = FindObjectOfType<StatisticCalculator2>();
        statManager = FindObjectOfType<StatManager>();

        forwardButton.color = Color.grey;

        if(!statCalc.CanRewind(0,0))
        {
            rewindButton.color = Color.grey;
        }
    }

    public void RewindBack()
    {
        if(statCalc.CanRewind(statManager.BarChartValue(),rewind))
        {
            

            
            rewind--;
            statManager.RewindChanged(rewind);
            /*
            switch(statManager.BarChartValue())
            {
                case 0: statCalc.RewindWeek(rewind); //week

                    break;
                case 1: statCalc.RewindMonth(rewind); // month

                    break;
            }
            */

            forwardButton.color = Color.white;
        }
        else
        {
            rewindButton.color = Color.grey;
        }
    }

    public void GoForward()
    {
        if(rewind == 0)
        {
            return;
        }

        rewind++;
        statManager.RewindChanged(rewind);





        /*
        switch (statManager.BarChartValue())
        {
            case 0:
                statCalc.RewindWeek(rewind); //week

                break;
            case 1:
                statCalc.RewindMonth(rewind); // month

                break;
        }
        */



        if (rewind == 0)
        {
            forwardButton.color = Color.grey;
        }
        else
        {
            rewindButton.color = Color.white;
        }
    }

    public void ResetButtons()
    {
        rewind = 0;

        if (!statCalc.CanRewind(0, 0))
        {
            rewindButton.color = Color.grey;
        }
        else
        {
            rewindButton.color = Color.white;
        }

        forwardButton.color = Color.grey;
    }

    public int GetCurrentRewind()
    {
        return rewind;
    }
}
