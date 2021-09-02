using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntervalSummaryPrefabUtility : MonoBehaviour
{
    [SerializeField] private TMP_Text leftSideText;
    [SerializeField] private TMP_Text rightSideText;

    int from;
    int to;
    AppManager.TaskMetricType metric;
    string customMetric = "";
    int points;


    public void FillOutTexts(int _from, int _to, AppManager.TaskMetricType _metric, int _points)
    {
        from = _from;
        to = _to;
        metric = _metric;
        points = _points;

        leftSideText.text = from + " - " + to + " " + RuntimeTranslator.TranslateTaskMetricType(metric) + ":";
        rightSideText.text = points + " " + RuntimeTranslator.TranslatePointsWord();
    }

    public void UpdateNumbers(int _from, int _to, int _points)
    {
        from = _from;
        to = _to;
        points = _points;

        leftSideText.text = from + " - " + to + " " + RuntimeTranslator.TranslateTaskMetricType(metric) + ":";
        rightSideText.text = points + " " + RuntimeTranslator.TranslatePointsWord();
    }

    public void UpdateMetric(AppManager.TaskMetricType _metric)
    {
        metric = _metric;

        leftSideText.text = from + " - " + to + " " + RuntimeTranslator.TranslateTaskMetricType(metric) + ":";
        rightSideText.text = points + " " + RuntimeTranslator.TranslatePointsWord();
    }






    public void FillOutTexts(int _from, int _to, string _metric, int _points)
    {
        from = _from;
        to = _to;
        customMetric = _metric;
        points = _points;

        leftSideText.text = from + " - " + to + " " + customMetric + ":";
        rightSideText.text = points + " " + RuntimeTranslator.TranslatePointsWord();
    }


    public void UpdateMetric(string _metric)
    {
        customMetric = _metric;

        leftSideText.text = from + " - " + to + " " + customMetric + ":";
        rightSideText.text = points + " " + RuntimeTranslator.TranslatePointsWord();
    }
}
