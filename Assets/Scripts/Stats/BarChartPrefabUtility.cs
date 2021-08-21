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


    public void SetProperties(Color _color, float _amount, string _when)
    {
        image.color = _color;
        amountText.text = _amount.ToString();
        dayText.text = _when.ToString();

        if (picto != null)
        {
            Destroy(picto.gameObject);
        }
    }

    public void SetProperties(Color _color)
    {
        image.color = _color;
        Destroy(amountText.gameObject);
        Destroy(dayText.gameObject);
        if (picto != null)
        {
            Destroy(picto.gameObject);
        }
    }

    public void SetProperties(Color _color, float _amount)
    {
        image.color = _color;
        amountText.text = _amount.ToString();
        Destroy(dayText.gameObject);
        if (picto != null)
        {
            Destroy(picto.gameObject);
        }
    }

    public void SetProperties(float _amount, Color32 _color, int _pictoId)
    {
        amountText.text = _amount.ToString();
     //   print(_color);
        image.color = _color;
        picto.sprite = AppManager.GetSpriteFromId(_pictoId);
        Destroy(dayText.gameObject);
    }


    public void SetSize(float _size)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.x, _size);
    }
}
