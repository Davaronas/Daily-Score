using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public struct PieChartInfo
{
    public float point;
    public string description;
    public Color32 color;
    public PieChartInfo(float _pont, string _desc, GoalColor _color)
    {
        point = _pont;
        description = _desc;
        color = _color;
    }
}

public struct Pie
{
    public Image image;
   public PieChartInfo info;

    public Pie (Image _image, PieChartInfo _info)
    {
        image = _image;
        info = _info;
    }
}

public class PieChart : MonoBehaviour
{

    [SerializeField] private GameObject piePrefab = null;
    [SerializeField] private TMP_Text descriptionText = null;
    [SerializeField] private TMP_Text percentText = null;
    [SerializeField] private Transform elementHolder = null;
    [SerializeField] private GameObject elementPrefab = null;

    private RectTransform rectTransform;
    private float max = 0;
    private float sum = 0;

    private List<Pie> pies = new List<Pie>();
    


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();




        
      
        
    }


    [ContextMenu("Fill test data")]
    public void TestLoad()
    {
    //   rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x);

        PieChartInfo[] _test = new PieChartInfo[4];
        _test[0] = new PieChartInfo(200, "Kacsa", (Color32)Color.red);
        _test[1] = new PieChartInfo(250, "Kiskacsa", (Color32)Color.blue);
        _test[2] = new PieChartInfo(180, "Liba", (Color32)Color.green);
        _test[3] = new PieChartInfo(300, "Gácsér", (Color32)Color.cyan);

        LoadData(_test);
    }





    public void Clear()
    {
        foreach(Pie _pie in pies)
        {
            Destroy(_pie.image.gameObject);
        }

        pies.Clear();
        max = 0;
        sum = 0;

        descriptionText.text = "";
        percentText.text = "";

        foreach(Transform _child in elementHolder)
        {
            Destroy(_child.gameObject);
        }
    }

    public void LoadData(PieChartInfo[] _infos)
    {
        if(_infos.Length == 0) { return; }

        int _maxIndex = 0;
        bool _allZero = true;
        for (int i = 0; i < _infos.Length; i++)
        {
            if(_infos[i].point != 0)
            {
                _allZero = false;
            }

            sum += _infos[i].point;
            if(_infos[i].point > max)
            {
                max = _infos[i].point;
                _maxIndex = i;
            }
        }

        if (_allZero) { return; }

        float _fill = 0;
        for (int j = 0; j < _infos.Length; j++)
        {
            RectTransform _newPie = Instantiate(piePrefab, transform.position, Quaternion.identity, transform).GetComponent<RectTransform>();
            _newPie.sizeDelta = rectTransform.rect.size;
        //    _newPie.anchoredPosition.Set()

          

            Image _pieImage = _newPie.GetComponent<Image>();
            _pieImage.color = _infos[j].color;
            _pieImage.fillAmount = _fill + _infos[j].point / sum;
            _pieImage.transform.SetSiblingIndex(0);

            pies.Add(new Pie(_pieImage, _infos[j]));

            _fill += _infos[j].point / sum;

            GameObject _newElement = Instantiate(elementPrefab, transform.position, Quaternion.identity, elementHolder);
            _newElement.GetComponent<TMP_Text>().text = _infos[j].description + " " + Math.Round(_infos[j].point / sum * 100, 1) + "%";
            _newElement.GetComponentInChildren<Image>().color = _infos[j].color;

            if(j == _maxIndex)
            {
                descriptionText.text = _infos[j].description;
                percentText.text = Math.Round(_infos[j].point / sum * 100,1) + " %";
            }
        }


        // write out all the names below with colored circles
    }




}
