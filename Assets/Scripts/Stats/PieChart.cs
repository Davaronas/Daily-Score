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
    public Color32 color1;
    public Color32 color2;
    public PieChartInfo(float _pont, string _desc, GoalColor _color1, GoalColor _color2)
    {
        point = _pont;
        description = _desc;
        color1 = _color1;
        color2 = _color2;
    }

    public PieChartInfo(float _pont, string _desc, Color _color1, Color _color2)
    {
        point = _pont;
        description = _desc;
        color1 = _color1;
        color2 = _color2;
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

    [SerializeField] private GameObject disabledImage = null;
    [SerializeField] private TMP_Text timeText = null;

    private RectTransform rectTransform;
    private float max = 0;
    private float sum = 0;

    private List<Pie> pies = new List<Pie>();

    private Vector2 pieChartSize;
    


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();


        disabledImage.SetActive(true);

        
      
        
    }



    private void Start()
    {
        pieChartSize = rectTransform.rect.size;
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

        if (elementHolder != null)
        {
            foreach (Transform _child in elementHolder)
            {
                Destroy(_child.gameObject);
            }
        }
    }

    public void LoadData(PieChartInfo[] _infos, string _from_to)
    {

        if(_infos.Length == 0)
        {
            disabledImage.SetActive(true);
            timeText.enabled = false;
            return; 
        }


        timeText.text = _from_to;

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

        if (_allZero)
        {
            disabledImage.SetActive(true);
            return;
        }
        else
        {
            disabledImage.SetActive(false);
        }

        float _fill = 0;
        for (int j = 0; j < _infos.Length; j++)
        {
            RectTransform _newPie = Instantiate(piePrefab, transform.position, Quaternion.identity, transform).GetComponent<RectTransform>();
            _newPie.sizeDelta = pieChartSize;
         //   StartCoroutine(SizeCheck(_newPie));
            //    _newPie.anchoredPosition.Set()



            Image _pieImage = _newPie.GetComponent<Image>();
            _pieImage.color = _infos[j].color1;
            _pieImage.fillAmount = _fill + _infos[j].point / sum;
            _pieImage.transform.SetSiblingIndex(0);

            pies.Add(new Pie(_pieImage, _infos[j]));

            _fill += _infos[j].point / sum;

            if (_infos[j].point > 0)
            {
                GameObject _newElement = Instantiate(elementPrefab, transform.position, Quaternion.identity, elementHolder);
                _newElement.GetComponent<TMP_Text>().text = _infos[j].description + " " + Math.Round(_infos[j].point / sum * 100, 1) + "%";
                ImageColoring _ic = _newElement.GetComponentInChildren<ImageColoring>();
                _ic.color1 = _infos[j].color1;
                _ic.color2 = _infos[j].color2;
                _ic.UpdateTexture();
               
            }

            if(j == _maxIndex)
            {
                descriptionText.text = _infos[j].description;
                percentText.text = Math.Round(_infos[j].point / sum * 100,1) + " %";
            }
        }


      
    }



    public void LoadData(PieChartInfo[] _infos, string _from_to, bool _useFirstNumberInMiddle)
    {

        if (_infos.Length == 0)
        {
            disabledImage.SetActive(true);
            timeText.enabled = false;
            return;
        }

        if (timeText != null)
        {
            timeText.text = _from_to;
        }

        

        bool _allZero = true;
        for (int i = 0; i < _infos.Length; i++)
        {
            if (_infos[i].point != 0)
            {
                _allZero = false;
            }

            sum += _infos[i].point;
            if (_infos[i].point > max)
            {
                max = _infos[i].point;
  
            }
        }

        if (_allZero)
        {
            disabledImage.SetActive(true);
            return;
        }
        else
        {
            disabledImage.SetActive(false);
        }

       
        

        float _fill = 0;
        for (int j = 0; j < _infos.Length; j++)
        {
            RectTransform _newPie = Instantiate(piePrefab, transform.position, Quaternion.identity, transform).GetComponent<RectTransform>();
            _newPie.sizeDelta = pieChartSize;
            
          //  StartCoroutine(SizeCheck(_newPie));
 



            Image _pieImage = _newPie.GetComponent<Image>();
            _pieImage.color = _infos[j].color1;
            _pieImage.fillAmount = _fill + _infos[j].point / sum;
            _pieImage.transform.SetSiblingIndex(0);

            pies.Add(new Pie(_pieImage, _infos[j]));

            _fill += _infos[j].point / sum;


            if (elementHolder != null)
            {
                if (_infos[j].point > 0)
                {
                    GameObject _newElement = Instantiate(elementPrefab, transform.position, Quaternion.identity, elementHolder);
                    _newElement.GetComponent<TMP_Text>().text = _infos[j].description + " " + Math.Round(_infos[j].point / sum * 100, 1) + "%";
                    ImageColoring _ic = _newElement.GetComponentInChildren<ImageColoring>();
                    _ic.color1 = _infos[j].color1;
                    _ic.color2 = _infos[j].color2;
                    _ic.UpdateTexture();
                }
            }

            if (_useFirstNumberInMiddle)
            {

                if (j == 0)
                {
                    descriptionText.text = "";
                    percentText.text = Math.Round(_infos[j].point / sum * 100, 1) + "%";
                }
            }

        }

     
    }


    IEnumerator SizeCheck(RectTransform _newPie)
    {
        yield return new WaitForSeconds(1f);

        if(_newPie != null)
        _newPie.sizeDelta = rectTransform.rect.size;
        
    }

}
