using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayedTip : MonoBehaviour
{
   [SerializeField] private GameObject saveTipButton;
    [SerializeField] private TMP_Text tipHeader;
    [SerializeField] private TMP_Text tipContent;



   [HideInInspector] public int tipId = -1;


    public void SetData(int _id, string _header, string _content)
    {
        tipId = _id;
        tipHeader.text = _header;
        tipContent.text = _content;
    }


    public int GetHeldTipId()
    {
        return tipId;
    }

    public void SetSaveButtonState(bool _state)
    {
        saveTipButton.SetActive(_state);
    }
}
