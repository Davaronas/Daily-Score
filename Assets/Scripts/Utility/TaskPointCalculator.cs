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




    /*
    public static int GetPointsFromCurrentValue(MaximumTaskData _mtd, out float _floatCurrent)
    {

        if (_mtd.current > _mtd.targetValue && _mtd.overachievePercentBonus > 0)
        {
            _floatCurrent = _mtd.current * _mtd.pointsGainedPerOne * (1 + ((float)_mtd.overachievePercentBonus/100));
        }
        else
        {
            _floatCurrent = _mtd.current * _mtd.pointsGainedPerOne;
        }


        return Mathf.RoundToInt(_floatCurrent);
    }
    */

    public static int GetPointsFromCurrentValue(MaximumTaskData _mtd)
    {
        int _amount;
        if (_mtd.current > _mtd.targetValue && _mtd.overachievePercentBonus > 0)
        {
            _amount = Mathf.RoundToInt(_mtd.current * _mtd.pointsGainedPerOne * (1 + ((float)_mtd.overachievePercentBonus / 100)));
        }
        else
        {
            _amount = _mtd.current * _mtd.pointsGainedPerOne;
        }


        return _amount;
    }

    public static int GetPointsFromCurrentValue(MinimumTaskData _mtd, out float _floatCurrent)
    {

        if(_mtd.current == _mtd.targetValue)
        {
            _floatCurrent = _mtd.pointsForStayingUnderTargetValue;
        }
        else if(_mtd.current < _mtd.targetValue)
        {
            if(_mtd.underTargetValuePercentBonus > 0)
            {
                _floatCurrent = _mtd.pointsForStayingUnderTargetValue + ((_mtd.targetValue - _mtd.current) * _mtd.pointsLostPerOne) * (1 + ((float)_mtd.underTargetValuePercentBonus / 100));
            }
            else
            {
                _floatCurrent = _mtd.pointsForStayingUnderTargetValue + ((_mtd.targetValue -_mtd.current) * _mtd.pointsLostPerOne);
            }
        }
        else
        {
           _floatCurrent = _mtd.pointsForStayingUnderTargetValue - ((_mtd.current - _mtd.targetValue) * _mtd.pointsLostPerOne);
        }

        return Mathf.RoundToInt(_floatCurrent);
    }

    public static int GetPointsFromCurrentValue(MinimumTaskData _mtd)
    {
        int _amount;
        if (_mtd.current == _mtd.targetValue)
        {
            _amount = _mtd.pointsForStayingUnderTargetValue;
        }
        else if (_mtd.current < _mtd.targetValue)
        {
            if (_mtd.underTargetValuePercentBonus > 0)
            {
                _amount = Mathf.RoundToInt(_mtd.pointsForStayingUnderTargetValue + ((_mtd.targetValue - _mtd.current) * _mtd.pointsLostPerOne) * (1 + ((float)_mtd.underTargetValuePercentBonus / 100)));
            }
            else
            {
                _amount = _mtd.pointsForStayingUnderTargetValue + ((_mtd.targetValue - _mtd.current) * _mtd.pointsLostPerOne);
            }
        }
        else
        {
            _amount = _mtd.pointsForStayingUnderTargetValue - ((_mtd.current - _mtd.targetValue) * _mtd.pointsLostPerOne);
        }

        return _amount;
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
