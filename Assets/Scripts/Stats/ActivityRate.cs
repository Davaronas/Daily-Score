using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityRate : MonoBehaviour
{
    struct Activity
    {
        public int points;
        public DateTime[] dates;
    }

    private GoalManager goalManager = null;
    [SerializeField] private BarChartHolder activityRateBarChart = null;

   [SerializeField] private Color[] barColors = new Color[7];


    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
        AppManager.OnLanguageChanged += LangCallback;
    }


    private void Start()
    {
        Invoke(nameof(CalculateActivityRate), 1.5f);
    }

    private bool Contains(Activity _a, DateTime _d)
    {
        for (int i = 0; i < _a.dates.Length; i++)
        {
            if(_d == _a.dates[i])
            {
                return true;
            }
        }

        return false;
    }

    private void LangCallback(AppManager.Languages _l)
    {
        CalculateActivityRate();
    }

    public void CalculateActivityRate()
    {
        activityRateBarChart.Clear();

        GoalData[] _goalDatas = goalManager.GetExistingGoals();

        Activity[] _activities = new Activity[7];

        for (int i = 0; i < _activities.Length; i++)
        {
            _activities[i].points = 0;
        }

        for (int i = 0; i < _activities.Length; i++)
        {
            _activities[i].dates = new DateTime[4];


            for (int l = 0; l < _activities[i].dates.Length; l++)
            {
                for (int k = 0; k < _goalDatas.Length; k++)
                {
                    for (int j = _goalDatas[k].dailyScores.Count - 1; j > -1; j--)
                    {
                        if (_goalDatas[k].dailyScores[j].isRestDay) { continue; }

                        if (_goalDatas[k].dailyScores[j].GetDateTime().DayOfWeek == (DayOfWeek)i)
                        {
                            if (Contains(_activities[i], _goalDatas[k].dailyScores[j].GetDateTime()))
                            {
                                continue;
                            }

                            if (_activities[i].dates[l] == DateTime.MinValue)
                            {
                                _activities[i].dates[l] = _goalDatas[k].dailyScores[j].GetDateTime();

                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < _activities.Length; i++)
        {
            for (int j = 0; j < _activities[i].dates.Length; j++)
            {
                for (int k = 0; k < _goalDatas.Length; k++)
                {
                    for (int l = _goalDatas[k].dailyScores.Count - 1; l > -1; l--)
                    {
                        if (_goalDatas[k].dailyScores[j].isRestDay) { continue; }

                        if (_goalDatas[k].dailyScores[l].GetDateTime() == _activities[i].dates[j])
                        {
                            _activities[i].points += _goalDatas[k].dailyScores[l].amount;
                        }
                    }
                }
            }
        }

        List<BarChartInfoText> _infos = new List<BarChartInfoText>();
        float _avg = 0;

        for (int i = 0; i < _activities.Length; i++)
        {
            _avg += _activities[i].points;
        }

        _avg /= _activities.Length;

        BarChartInfoText _sunday = new BarChartInfoText();

        for (int i = 0; i < _activities.Length; i++)
        {
            if (i == 0)
            {
                _sunday = new BarChartInfoText((float)Math.Round(_activities[i].points / _avg, 2), RuntimeTranslator.TranslateDayOfWeek(((DayOfWeek)i)).Substring(0, 3));
            }
            else
            {
                _infos.Add(new BarChartInfoText((float)Math.Round(_activities[i].points / _avg, 2), RuntimeTranslator.TranslateDayOfWeek(((DayOfWeek)i)).Substring(0, 3)));
            }

        }

        _infos.Add(_sunday);


        int _nonzero = 0;
        if (_infos.Count > 1)
        {
            for (int i = 0; i < _infos.Count; i++)
            {
                if(_infos[i].point > 0)
                {
                    _nonzero++;
                }
            }

            if (_nonzero > 1)
            {
                activityRateBarChart.LoadData(_infos.ToArray(), barColors, true, true);
            }
        }




    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            CalculateActivityRate();
        }
    }
}
