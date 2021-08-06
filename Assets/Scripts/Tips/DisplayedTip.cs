using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayedTip : MonoBehaviour
{
   [SerializeField] private GameObject saveTipButton;
    [SerializeField] private TMP_Text tipHeader;
    [SerializeField] private TMP_Text tipContent;

    [SerializeField] private RectTransform scrollContent;
    private ScrollRect scroll;



   [HideInInspector] public int tipId = -1;

    private void Awake()
    {
        scroll = scrollContent.parent.GetComponent<ScrollRect>();
        scroll.verticalNormalizedPosition = 1;
    }

    public void SetData(int _id, string _header, string _content)
    {
        scrollContent.offsetMin = new Vector2(scrollContent.offsetMin.x, -_content.Length);


        tipId = _id;
        tipHeader.text = _header;
        tipContent.text = _content;

        if (scroll == null)
        {
            scroll = scrollContent.parent.GetComponent<ScrollRect>();
            scroll.verticalNormalizedPosition = 1;
        }
    }

    private void OnDisable()
    {
        scroll.verticalNormalizedPosition = 1;
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
