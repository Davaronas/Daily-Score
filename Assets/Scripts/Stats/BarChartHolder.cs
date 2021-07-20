using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct BarChartInfo
{
   public float point;
  public  string description;
    public BarChartInfo(float _pont, string _desc)
    {
        point = _pont;
        description = _desc;
    }
}

public class BarChartHolder : MonoBehaviour
{
    [SerializeField] private Color barColor = Color.white;
    [SerializeField] private GameObject barPrefab = null;
    [SerializeField] private TMP_Text minText = null;
    [SerializeField] private TMP_Text maxText = null;
    [SerializeField] private TMP_Text minTime = null;
    [SerializeField] private TMP_Text maxTime = null;

    private RectTransform minText_RT = null;
    private RectTransform maxText_RT = null;





    private Dictionary<BarChartPrefabUtility, float> bars = new Dictionary<BarChartPrefabUtility, float>();

    private float holderHeight = 0;
    private float max = 0;
    private float min = Mathf.Infinity;

    private void Awake()
    {
        holderHeight = GetComponent<RectTransform>().rect.height;
        minText_RT = minText.GetComponent<RectTransform>();
        maxText_RT = maxText.GetComponent<RectTransform>();

        
       
        
    }


    public void LoadData(BarChartInfo[] _infos, bool _displayTexts)
    {
        if (_displayTexts)
        {
            for (int i = 0; i < _infos.Length; i++)
            {
                LoadBar(_infos[i].point, _infos[i].description);
            }
        }
        else
        {
            

            for (int i = 0; i < _infos.Length; i++)
            {
                LoadBar(_infos[i].point, _infos[i].description,false);
            }

           

            minTime.text = _infos[0].description;
            maxTime.text = _infos[_infos.Length - 1].description;
        }
    }

    public void Clear()
    {
        foreach (KeyValuePair<BarChartPrefabUtility, float> _bar in bars)
        {

            Destroy(_bar.Key.gameObject);
        }

        bars.Clear();

        min = Mathf.Infinity;
        max = 0;

        maxText.text = "0";
        minText.text = "0";

        maxText_RT.anchoredPosition = new Vector2(-5, 0);
        minText_RT.anchoredPosition = new Vector2(-5, 0);

        minTime.text = "";
        maxTime.text = "";
      
    }
   

    private void LoadBar(float _points, string _time, bool _useTexts = true)
    {

        if(_points > max)
        {
            max = _points;
        }

        if(_points < min)
        {
            min = _points;
        }

        

        BarChartPrefabUtility _newBar = Instantiate(barPrefab, transform.position, Quaternion.identity, transform).GetComponent<BarChartPrefabUtility>();
        bars.Add(_newBar,_points);

        if (_useTexts)
        {
            _newBar.SetProperties(barColor, _points, _time); // set time!
        }
        else
        {
            _newBar.SetProperties(barColor);
        }



        foreach(KeyValuePair<BarChartPrefabUtility,float> _bar in bars)
        {

            _bar.Key.SetSize((_bar.Value / max) * holderHeight);
        }

        maxText.text = max.ToString();

        if (min != Mathf.Infinity)
        {
            minText.text = min.ToString();
        }
        else
        {
            minText.text = "0";
        }

        maxText_RT.anchoredPosition = new Vector2(-5, holderHeight);
        minText_RT.anchoredPosition = new Vector2(-5, min / max * holderHeight);

    }
}
