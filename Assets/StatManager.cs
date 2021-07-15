using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown chart1_dropdown;

    private StatisticCalculator2 statCalculator = null;

    private void Awake()
    {
        statCalculator = FindObjectOfType<StatisticCalculator2>();
        chart1_dropdown.ClearOptions();

        List<string> _options = new List<string>();
        _options.Add(RuntimeTranslator.TranslateWeeklyWord());
        _options.Add(RuntimeTranslator.TranslateMonthlyWord());

        chart1_dropdown.AddOptions(_options);

        chart1_dropdown.value = 0;

    }

    public void RemoteCall_BarChartValueChanged()
    {
        switch(chart1_dropdown.value)
        {
            case 0:
                statCalculator.Weeklygraph();
                break;
            case 1:

                break;
        }
    }

}
