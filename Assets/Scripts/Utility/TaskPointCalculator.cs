using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TaskPointCalculator
{

    public static int GetPointsFromCurrentValue(TaskData _data)
    {
        switch (_data.type)
        {
            case AppManager.TaskType.Maximum:
                return GetPointsFromCurrentValue(_data as MaximumTaskData);

            case AppManager.TaskType.Minimum:
                return GetPointsFromCurrentValue(_data as MinimumTaskData);

            case AppManager.TaskType.Boolean:
                return GetPointsFromCurrentValue(_data as BooleanTaskData);

            case AppManager.TaskType.Optimum:
                return GetPointsFromCurrentValue(_data as OptimumTaskData);

            case AppManager.TaskType.Interval:
                return GetPointsFromCurrentValue(_data as IntervalTaskData);



           default:
                Debug.LogError("No datatype has been discovered");
                return 0;

        }
    }




    public static int GetPointsFromCurrentValue(MaximumTaskData _mtd)
    {
        return _mtd.current * _mtd.pointsGainedPerOne;
    }

    public static int GetPointsFromCurrentValue(MinimumTaskData _mtd)
    {
        if(_mtd.current == _mtd.targetValue)
        {
            return _mtd.pointsForStayingUnderTargetValue;
        }
        else if(_mtd.current < _mtd.targetValue)
        {
            return _mtd.pointsForStayingUnderTargetValue + ((_mtd.targetValue -_mtd.current) * _mtd.pointsLostPerOne);
        }
        else
        {
            return _mtd.pointsForStayingUnderTargetValue - ((_mtd.current - _mtd.targetValue) * _mtd.pointsLostPerOne);
        }
    }

    public static int GetPointsFromCurrentValue(BooleanTaskData _btd)
    {
        if(_btd.isDone)
        {
            return _btd.pointsGained;
        }
        else
        {
            return 0;
        }
    }

    public static int GetPointsFromCurrentValue(OptimumTaskData _otd)
    {
        if(_otd.current == _otd.targetValue)
        {
            return _otd.pointsForOptimum;
        }
        else
        {
            return _otd.pointsForOptimum - ( Mathf.Abs(_otd.current - _otd.targetValue) * _otd.pointsLostPerOneDifference);
        }
    }

    public static int GetPointsFromCurrentValue(IntervalTaskData _itd)
    {
        for(int i = 0; i < _itd.intervals.Length; i++)
        {
            if(_itd.current >= _itd.intervals[i].from  && _itd.current <= _itd.intervals[i].to)
            {
                return _itd.intervals[i].points;
            }
        }

        return 0;
    }
}
