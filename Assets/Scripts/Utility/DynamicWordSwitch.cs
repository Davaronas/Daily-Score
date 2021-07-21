using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicWordSwitch : MonoBehaviour
{
    private string originalText;
    private MultiLanguageText mlt = null;
    private TMP_Text text = null;

    private TaskManager taskManager;
    private TaskTypeComponents taskTypeComponents;

    private string lastTaskName;
    private string lastMetricName;
    private string lastTargetValue;

    private void Awake()
    {
        AppManager.OnLanguageChanged += LanguageChangedCallback;
        mlt = GetComponent<MultiLanguageText>();
        text = GetComponent<TMP_Text>();

        taskManager = FindObjectOfType<TaskManager>();
        taskTypeComponents = FindObjectOfType<TaskTypeComponents>();

        taskManager.MetricUpdateNeeded += SwitchMetricWord;
        taskManager.TaskNameUpdateNeeded += SwitchTasknameWord;
        taskManager.TargetValueUpdateNeeded += SwitchTargetValueWord;

        taskTypeComponents.MetricUpdateNeeded += SwitchMetricWord;
        taskTypeComponents.TargetValueUpdateNeeded += SwitchTargetValueWord;
    }

    private void OnDestroy()
    {
        AppManager.OnLanguageChanged -= LanguageChangedCallback;

        taskManager.MetricUpdateNeeded -= SwitchMetricWord;
        taskManager.TaskNameUpdateNeeded -= SwitchTasknameWord;
        taskManager.TargetValueUpdateNeeded -= SwitchTargetValueWord;

        taskTypeComponents.MetricUpdateNeeded -= SwitchMetricWord;
        taskTypeComponents.TargetValueUpdateNeeded -= SwitchTargetValueWord;
    }

    private void LanguageChangedCallback(AppManager.Languages _l)
    {
        originalText = mlt.languages[(int)_l];
    }


    private void SwitchMetricWord(string _s)
    {
        lastMetricName = _s;
        Replace();
    }

    private void SwitchTargetValueWord(string _s)
    {
        lastTargetValue = _s;
        Replace();
    }

    private void SwitchTasknameWord(string _s)
    {
        lastTaskName = _s;
        Replace();
    }

    private void Replace()
    {
        text.text = originalText.Replace("TASKNAME", lastTaskName);
        text.text = text.text.Replace("TARGET", lastTargetValue);
        text.text = text.text.Replace("METRIC", lastMetricName);
    }


}
