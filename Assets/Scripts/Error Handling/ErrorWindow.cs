using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text errorText;
    private RectTransform rectTransform;



    public void ShowError(string _error)
    {

        errorText.text = _error;
    }
}
