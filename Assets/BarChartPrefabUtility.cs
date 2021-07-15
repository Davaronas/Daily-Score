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


    public void SetProperties(Color _color, float _amount, string _when)
    {
        image.color = _color;
        amountText.text = _amount.ToString();
        dayText.text = _when.ToString();
    }

    public void SetProperties(Color _color)
    {
        image.color = _color;
        Destroy(amountText.gameObject);
        Destroy(dayText.gameObject);
    }

    public void SetSize(float _size)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.x, _size);
    }
}
