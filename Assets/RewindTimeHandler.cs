using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTimeHandler : MonoBehaviour
{
    public int rewind = 0;
    private StatisticCalculator2 statCalc = null;

    private void Awake()
    {
        statCalc = FindObjectOfType<StatisticCalculator2>();
    }

    public void RemoteCall_RewindBack()
    {
        if(statCalc.CanRewind())
        {

        }

        rewind--;
    }

    public void RemoteCall_GoForward()
    {
        if(statCalc.CanGoForwardInTime())
        {

        }
    }
}
