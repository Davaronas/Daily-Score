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

    [SerializeField] private GameObject disabledImage = null;
    [SerializeField] private TMP_Text selectedNameText = null;

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

        disabledImage.SetActive(false);



    }


    public void LoadData(BarChartInfo[] _infos, bool _useUnderTexts,bool _useAmounts)
    {
        bool _foundNonZero = false;
        for (int i = 0; i < _infos.Length; i++)
        {
            if(_infos[i].point != 0)
            {
                _foundNonZero = true;
            }
        }

        if(!_foundNonZero)
        {
            disabledImage.SetActive(true);
            selectedNameText.enabled = false;
        }
        else
        {
            disabledImage.SetActive(false);
            selectedNameText.enabled = true;
        }


        if (_useUnderTexts)
        {
            for (int i = 0; i < _infos.Length; i++)
            {
                LoadBar(_infos[i].point, _infos[i].description);
            }
        }
        else if(_useAmounts)
        {
            for (int i = 0; i < _infos.Length; i++)
            {
                LoadBar(_infos[i].point, _infos[i].description, false, true);
            }



            minTime.text = _infos[0].description;
            maxTime.text = _infos[_infos.Length - 1].description;
        }
        else
        {
            

            for (int i = 0; i < _infos.Length; i++)
            {
                LoadBar(_infos[i].point, _infos[i].description,false,false);
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
   

    private void LoadBar(float _points, string _time, bool _useUnderTexts = true, bool _useAmounts = true)
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

        if (_useUnderTexts)
        {
            _newBar.SetProperties(barColor, _points, _time); // set time!
        }
        else if(_useAmounts)
        {
            _newBar.SetProperties(barColor, _points);
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
