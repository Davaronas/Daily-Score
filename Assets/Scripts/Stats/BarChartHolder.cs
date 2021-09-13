using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct BarChartInfoText
{
   public float point;
  public  string description;
    public BarChartInfoText(float _pont, string _desc)
    {
        point = _pont;
        description = _desc;
    }
}

public struct BarChartInfoImage
{
    public float point;
    public int pictoId;

    public BarChartInfoImage(float _point, int _id)
    {
        point = _point;
        pictoId = _id;
    }
}



public class BarChartHolder : MonoBehaviour
{
    [SerializeField] private Color barColor = Color.white;
    [SerializeField] private GameObject barPrefab = null;
    [SerializeField] private GameObject restDayBarPrefab = null;
    [SerializeField] private TMP_Text minText = null;
    [SerializeField] private TMP_Text maxText = null;
    [SerializeField] private TMP_Text minTime = null;
    [SerializeField] private TMP_Text maxTime = null;

    [SerializeField] private GameObject disabledImage = null;
    [SerializeField] private TMP_Text selectedNameText = null;
    [SerializeField] private TMP_Text timeText = null;

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

        disabledImage.SetActive(true);



    }


    public void LoadData(BarChartInfoText[] _infos, bool _useUnderTexts,bool _useAmounts)
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
            if (selectedNameText != null)
            {
                selectedNameText.enabled = false;
            }
        }
        else
        {
           
            disabledImage.SetActive(false);
            if (selectedNameText != null)
            {
                selectedNameText.enabled = true;
            }
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


    public void LoadData(BarChartInfoText[] _infos, Color[] _barColors, bool _useUnderTexts, bool _useAmounts )
    {
        bool _foundNonZero = false;
        for (int i = 0; i < _infos.Length; i++)
        {
          //  print(_infos[i].point);
            if (_infos[i].point != 0 && !float.IsNaN(_infos[i].point))
            {
               
                _foundNonZero = true;
            }
        }

        if (!_foundNonZero)
        {
            disabledImage.SetActive(true);
            if (selectedNameText != null)
            {
                selectedNameText.enabled = false;
            }
        }
        else
        {

            disabledImage.SetActive(false);
            if (selectedNameText != null)
            {
                selectedNameText.enabled = true;
            }
        }


        if (_useUnderTexts)
        {
            for (int i = 0; i < _infos.Length; i++)
            {
                LoadBar(_infos[i].point, _infos[i].description, _barColors[i], true,true);
            }
        }
        else if (_useAmounts)
        {
            for (int i = 0; i < _infos.Length; i++)
            {
                LoadBar(_infos[i].point, _infos[i].description, _barColors[i], false, true);
            }



            minTime.text = _infos[0].description;
            maxTime.text = _infos[_infos.Length - 1].description;
        }
        else
        {


            for (int i = 0; i < _infos.Length; i++)
            {
                LoadBar(_infos[i].point,_infos[i].description, _barColors[i], false, false);
            }



            minTime.text = _infos[0].description;
            maxTime.text = _infos[_infos.Length - 1].description;
        }
    }

    public void LoadDataWithImages(BarChartInfoImage[] _infos, Color[] _barColors, string _time)
    {
        bool _foundNonZero = false;
        for (int i = 0; i < _infos.Length; i++)
        {
            if (_infos[i].point != 0)
            {
                _foundNonZero = true;
            }
        }

        if (!_foundNonZero)
        {
            disabledImage.SetActive(true);
            if (selectedNameText != null)
            {
                selectedNameText.enabled = false;
            }
        }
        else
        {

            disabledImage.SetActive(false);
            if (selectedNameText != null)
            {
                selectedNameText.enabled = true;
            }
        }

        if(timeText != null)
        {
            timeText.text = _time;
        }

        for (int i = 0; i < _infos.Length; i++)
        {
            if (_infos[i].point > 0)
            {
                LoadBar(_infos[i].point, _barColors[i], _infos[i].pictoId);
            }
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


        if (_points > int.MaxValue)
        {
            _points = int.MaxValue;
        }





        if(_points > max)
        {
            if(_points != int.MaxValue)
            max = _points;
        }

        if(_points < min)
        {
            min = _points;
        }

        BarChartPrefabUtility _newBar = null;
        if (_points != int.MaxValue)
        {
            _newBar = Instantiate(barPrefab, transform.position, Quaternion.identity, transform).GetComponent<BarChartPrefabUtility>();
        }
        else
        {
            _newBar = Instantiate(restDayBarPrefab, transform.position, Quaternion.identity, transform).GetComponent<BarChartPrefabUtility>();
        }

      

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
            if (_bar.Value != int.MaxValue)
            {
                _bar.Key.SetSize((_bar.Value / max) * holderHeight);
            }
            else
            {
                _bar.Key.SetSize(holderHeight);
            }
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

    private void LoadBar(float _points, string _time, Color _barColor, bool _useUnderTexts = true, bool _useAmounts = true)
    {


        if (_points > int.MaxValue)
        {
            _points = int.MaxValue;
        }


        if (_points > max)
        {
            if(_points != int.MaxValue)
            max = _points;
        }

        if (_points < min)
        {
            min = _points;
        }



        BarChartPrefabUtility _newBar = null;
        if (_points != int.MaxValue)
        {
            _newBar = Instantiate(barPrefab, transform.position, Quaternion.identity, transform).GetComponent<BarChartPrefabUtility>();
        }
        else
        {
            _newBar = Instantiate(restDayBarPrefab, transform.position, Quaternion.identity, transform).GetComponent<BarChartPrefabUtility>();
        }


        bars.Add(_newBar, _points);

        if (_useUnderTexts)
        {
            _newBar.SetProperties(_barColor, _points, _time); // set time!
        }
        else if (_useAmounts)
        {
            _newBar.SetProperties(_barColor, _points);
        }
        else
        {
            _newBar.SetProperties(_barColor);
        }



        foreach (KeyValuePair<BarChartPrefabUtility, float> _bar in bars)
        {
            if (_bar.Value != int.MaxValue)
            {

                _bar.Key.SetSize((_bar.Value / max) * holderHeight);
            }
            else
            {
                _bar.Key.SetSize(holderHeight);
            }
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

    private void LoadBar(float _points, Color _barColor, int _id)
    {
        if (_points > int.MaxValue)
        {
            _points = int.MaxValue;
        }




        if (_points > max)
        {
            if(_points != int.MaxValue)
            max = _points;
        }

        if (_points < min)
        {
            min = _points;
        }



        BarChartPrefabUtility _newBar = null;
        if (_points != int.MaxValue)
        {
            _newBar = Instantiate(barPrefab, transform.position, Quaternion.identity, transform).GetComponent<BarChartPrefabUtility>();
        }
        else
        {
            _newBar = Instantiate(restDayBarPrefab, transform.position, Quaternion.identity, transform).GetComponent<BarChartPrefabUtility>();
        }
        bars.Add(_newBar, _points);

      
            _newBar.SetProperties(_points,_barColor,_id); // set time!
      



        foreach (KeyValuePair<BarChartPrefabUtility, float> _bar in bars)
        {

            if (_bar.Value != int.MaxValue)
            {
                _bar.Key.SetSize((_bar.Value / max) * holderHeight);
            }
            else
            {
                _bar.Key.SetSize(holderHeight);
            }
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

    public Vector2[] GetRollingAverageEndPoints()
    {
        List<Vector2> _positions = new List<Vector2>();

        foreach (KeyValuePair<BarChartPrefabUtility, float> _bar in bars)
        {

            _positions.Add(_bar.Key.GetEndPoint());
        }

        return _positions.ToArray();
    }
}
