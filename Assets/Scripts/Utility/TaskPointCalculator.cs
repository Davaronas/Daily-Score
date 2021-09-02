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

    public static bool[] GetTargetValuesReached(GoalData _gd)
    {
        List<bool> _targetsReached = new List<bool>();

        for (int i = 0; i < _gd.tasks.Count; i++)
        {
            if (_gd.tasks[i].type != AppManager.TaskType.Interval)
            {
                _targetsReached.Add(IsTargetValueReached(_gd.tasks[i]));
            }
        }

        return _targetsReached.ToArray();
    }

    public static bool IsTargetValueReached(TaskData _data)
    {
        switch (_data.type)
        {
            case AppManager.TaskType.Maximum:
                return IsTargetValueReached(_data as MaximumTaskData);

            case AppManager.TaskType.Minimum:
                return IsTargetValueReached(_data as MinimumTaskData);

            case AppManager.TaskType.Boolean:
                return IsTargetValueReached(_data as BooleanTaskData);

            case AppManager.TaskType.Optimum:
                return IsTargetValueReached(_data as OptimumTaskData);

            case AppManager.TaskType.Interval:
                return IsTargetValueReached(_data as IntervalTaskData);



            default:
                Debug.LogError("No datatype has been discovered");
                return false;

        }
    }


    public static bool IsTargetValueReached(MaximumTaskData _mtd)
    {
        if(_mtd.current >= _mtd.targetValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsTargetValueReached(MinimumTaskData _mtd)
    {
        if(_mtd.current < _mtd.targetValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsTargetValueReached(BooleanTaskData _btd)
    {
        if(_btd.isDone)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool IsTargetValueReached(OptimumTaskData _otd)
    {
        if(_otd.current == _otd.targetValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsTargetValueReached(IntervalTaskData _itd)
    {
        return false;
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
        float _amount;
        if (_mtd.current > _mtd.targetValue && _mtd.overachievePercentBonus > 0)
        {
            _amount = Mathf.RoundToInt(_mtd.current * _mtd.pointsGainedPerOne * (1 + ((float)_mtd.overachievePercentBonus / 100)));
        }
        else
        {
            _amount = _mtd.current * _mtd.pointsGainedPerOne;
        }

        if (_amount > 0)
        {
            if (_mtd.completedTargetDaysIn_a_Row > _mtd.streakStartsAfterDays)
            {
                switch (_mtd.completedTargetDaysIn_a_Row - _mtd.streakStartsAfterDays)
                {
                    case 1:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FIRSTDAYSTREAKMULTIPLIER);
                        break;
                    case 2:
                        _amount = Mathf.RoundToInt(_amount * AppManager.SECONDDAYSTREAKMULTIPLIER);
                        break;
                    case 3:
                        _amount = Mathf.RoundToInt(_amount * AppManager.THIRDDAYSTREAKMULTIPLIER);
                        break;
                    case 4:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FOURTHDAYSTREAKMULTIPLIER);
                        break;
                    case 5:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FIFTHDAYSTREAKMULTIPLIER);
                        break;
                    default:
                        _amount =  Mathf.RoundToInt
                            (_amount * Mathf.Clamp
                            ((AppManager.FIFTHDAYSTREAKMULTIPLIER + 
                            (((_mtd.completedTargetDaysIn_a_Row - _mtd.streakStartsAfterDays) - 5)* AppManager.MORETHANFIVEDAYSTREAKMULTIPLIERADD)),
                            0,AppManager.MAXSTREAKMULTIPLIER));
                        break;
                }
            }
        }

        return Mathf.FloorToInt(_amount);
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
        float _amount;
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

        if (_amount > 0)
        {
            if (_mtd.completedTargetDaysIn_a_Row > _mtd.streakStartsAfterDays)
            {
                switch (_mtd.completedTargetDaysIn_a_Row - _mtd.streakStartsAfterDays)
                {
                    case 1:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FIRSTDAYSTREAKMULTIPLIER);
                        break;
                    case 2:
                        _amount = Mathf.RoundToInt(_amount * AppManager.SECONDDAYSTREAKMULTIPLIER);
                        break;
                    case 3:
                        _amount = Mathf.RoundToInt(_amount * AppManager.THIRDDAYSTREAKMULTIPLIER);
                        break;
                    case 4:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FOURTHDAYSTREAKMULTIPLIER);
                        break;
                    case 5:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FIFTHDAYSTREAKMULTIPLIER);
                        break;
                    default:
                        _amount = Mathf.RoundToInt
                            (_amount * Mathf.Clamp
                            ((AppManager.FIFTHDAYSTREAKMULTIPLIER +
                            (((_mtd.completedTargetDaysIn_a_Row - _mtd.streakStartsAfterDays) - 5) * AppManager.MORETHANFIVEDAYSTREAKMULTIPLIERADD)),
                            0, AppManager.MAXSTREAKMULTIPLIER));
                        break;
                }
            }
        }

        return Mathf.FloorToInt(_amount);
    }

    public static int GetPointsFromCurrentValue(BooleanTaskData _btd)
    {
        if(_btd.isDone)
        {
            int _amount = _btd.pointsGained;
                if (_btd.completedTargetDaysIn_a_Row > _btd.streakStartsAfterDays)
                {
                    switch (_btd.completedTargetDaysIn_a_Row - _btd.streakStartsAfterDays)
                    {
                        case 1:
                            _amount = Mathf.RoundToInt(_amount * AppManager.FIRSTDAYSTREAKMULTIPLIER);
                            break;
                        case 2:
                            _amount = Mathf.RoundToInt(_amount * AppManager.SECONDDAYSTREAKMULTIPLIER);
                            break;
                        case 3:
                            _amount = Mathf.RoundToInt(_amount * AppManager.THIRDDAYSTREAKMULTIPLIER);
                            break;
                        case 4:
                            _amount = Mathf.RoundToInt(_amount * AppManager.FOURTHDAYSTREAKMULTIPLIER);
                            break;
                        case 5:
                            _amount = Mathf.RoundToInt(_amount * AppManager.FIFTHDAYSTREAKMULTIPLIER);
                            break;
                        default:
                            _amount = Mathf.RoundToInt
                                (_amount * Mathf.Clamp
                                ((AppManager.FIFTHDAYSTREAKMULTIPLIER +
                                (((_btd.completedTargetDaysIn_a_Row - _btd.streakStartsAfterDays) - 5) * AppManager.MORETHANFIVEDAYSTREAKMULTIPLIERADD)),
                                0, AppManager.MAXSTREAKMULTIPLIER));
                            break;
                    }
                }
            

            return _amount;
        }
        else
        {
            return 0;
        }
    }

    public static int GetPointsFromCurrentValue(OptimumTaskData _otd)
    {
        float _amount = 0;

        if(_otd.current == _otd.targetValue)
        {
            _amount = _otd.pointsForOptimum;
        }
        else
        {
            _amount = _otd.pointsForOptimum - ( Mathf.Abs(_otd.current - _otd.targetValue) * _otd.pointsLostPerOneDifference);
        }

        if (_amount > 0)
        {
            if (_otd.completedTargetDaysIn_a_Row > _otd.streakStartsAfterDays)
            {
                switch (_otd.completedTargetDaysIn_a_Row - _otd.streakStartsAfterDays)
                {
                    case 1:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FIRSTDAYSTREAKMULTIPLIER);
                        break;
                    case 2:
                        _amount = Mathf.RoundToInt(_amount * AppManager.SECONDDAYSTREAKMULTIPLIER);
                        break;
                    case 3:
                        _amount = Mathf.RoundToInt(_amount * AppManager.THIRDDAYSTREAKMULTIPLIER);
                        break;
                    case 4:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FOURTHDAYSTREAKMULTIPLIER);
                        break;
                    case 5:
                        _amount = Mathf.RoundToInt(_amount * AppManager.FIFTHDAYSTREAKMULTIPLIER);
                        break;
                    default:
                        _amount = Mathf.RoundToInt
                            (_amount * Mathf.Clamp
                            ((AppManager.FIFTHDAYSTREAKMULTIPLIER +
                            (((_otd.completedTargetDaysIn_a_Row - _otd.streakStartsAfterDays) - 5) * AppManager.MORETHANFIVEDAYSTREAKMULTIPLIERADD)),
                            0, AppManager.MAXSTREAKMULTIPLIER));
                        break;
                }
            }
        }

        return Mathf.FloorToInt(_amount);
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
