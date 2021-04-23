using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]

public class SubmenuScroll : BehaviourButton
{
    [SerializeField]private RectTransform submenusRectTransform = null;

    private RectTransform rectTransform = null;
    private ScrollRect scrollRect = null;
    private AutoSizeUI uiSizer = null;


    private Dictionary<SubmenuButton, Vector2> submenuPositions = new Dictionary<SubmenuButton, Vector2>();

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        scrollRect = GetComponent<ScrollRect>();
        uiSizer = FindObjectOfType<AutoSizeUI>();

        SubmenuButton[] _smButtons = FindObjectsOfType<SubmenuButton>();

        for (int i = 0; i < submenusRectTransform.transform.childCount; i++)
        {
            SubmenuButton _attachButton = null;

            foreach (SubmenuButton _smButton in _smButtons)
            {
                if (submenusRectTransform.GetChild(i).GetSiblingIndex() == _smButton.GetId())
                {
                    _attachButton = _smButton;
                    break;
                }
            }

            if (_attachButton == null)
            {
                Debug.LogError($"No matching button to a submenu: {i}");
            }

            print(submenusRectTransform.GetChild(i).position);
            submenuPositions.Add(_attachButton, submenusRectTransform.GetChild(i).position);
        }


        StartCoroutine(Initialize());
        
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        
    }

    public void WarpToPosition(int _submenuId = -1)
    {
        if(_submenuId == -1)
        {
            _submenuId = CalculateWarpPosition();
        }



        foreach(KeyValuePair<SubmenuButton,Vector2> _keyValuePair in submenuPositions)
        {
            if (_keyValuePair.Key.GetId() == _submenuId)
            {
                StartCoroutine(Warp(_keyValuePair.Key.GetId()));
                return;
                
            }
        }
        Debug.LogError($"The Submenu ID to warp to does not appear in the {nameof(submenuPositions)} list");



    }

    private int CalculateWarpPosition()
    {
        print("CalculateWarpPosition");
        return -1;
    }

    IEnumerator Warp(int _id)
    {
        // print("Warp to: " + _pos);

        // rectTransform.position = _pos;
        // Could use rect transform

        //  while((Vector2)transform.position != _pos)

        float _scrollToPointPercentage = 1 / (float)uiSizer.GetNumberOfMenus() * _id;
        if (_id > Mathf.CeilToInt((float)uiSizer.GetNumberOfMenus() / 2))
        {
           // _scrollToPointPercentage += 
        }

        scrollRect.horizontalNormalizedPosition = 1 / (float)uiSizer.GetNumberOfMenus() * _id;
        yield return new WaitForEndOfFrame();

    }
}
