using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarChartPrefabUtility : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private Image picto;
    [SerializeField] private RectTransform endPoint;
    [SerializeField] private bool isRestDayBar = false;


    public void SetProperties(Color _color, float _amount, string _when)
    {
        
            if (!isRestDayBar)
                image.color = _color;

            if (_amount > 0)
            {
                if(!isRestDayBar)
                amountText.text = _amount.ToString();
            }
            else
            {
                amountText.text = "";
            }
            dayText.text = _when.ToString();

            if (picto != null)
            {
                Destroy(picto.gameObject);
            }
    }

    public void RestDayBar()
    {

    }

    public void SetProperties(Color _color)
    {
        if (!isRestDayBar)
            image.color = _color;

        if (!isRestDayBar)
            Destroy(amountText.gameObject);

        Destroy(dayText.gameObject);
        if (picto != null)
        {
            Destroy(picto.gameObject);
        }
    }

    public void SetProperties(Color _color, float _amount)
    {
        if (!isRestDayBar)
            image.color = _color;

        if (_amount > 0)
        {
            if (!isRestDayBar)
                amountText.text = _amount.ToString();
        }
        else
        {
            if (!isRestDayBar)
                amountText.text = "";
        }
        Destroy(dayText.gameObject);
        if (picto != null)
        {
            Destroy(picto.gameObject);
        }
    }

    public void SetProperties(float _amount, Color32 _color, int _pictoId)
    {
        if (_amount > 0)
        {
            if (!isRestDayBar)
                amountText.text = _amount.ToString();
        }
        else
        {
            if (!isRestDayBar)
                amountText.text = "";
        }
        //   print(_color);
        if (!isRestDayBar)
            image.color = _color;

        picto.sprite = AppManager.GetSpriteFromId(_pictoId);
        Destroy(dayText.gameObject);
    }


    public void SetSize(float _size)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.x, _size);
    }

    public Vector2 GetEndPoint()
    {
        if(IsValidPosition(endPoint.position))
        {
            return endPoint.position;

        }
        else
        {
            return new Vector2(0, 0);
        }
       
    }

    private bool IsValidPosition(Vector3 _pos)
    {
        if (float.IsNaN(_pos.x))
        {
            return false;
        }

        if (float.IsNaN(_pos.y))
        {
            return false;
        }

        if (float.IsNaN(_pos.z))
        {
            return false;
        }

        return true;
    }
}
