using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(ScrollRect))]

public class SubmenuScroll : BehaviourButton
{
    [SerializeField]private RectTransform submenusRectTransform = null;
    float oneSubmenuPercentage = 0f;

    private RectTransform rectTransform = null;
    private ScrollRect scrollRect = null;
    private AutoSizeUI uiSizer = null;


    private Dictionary<SubmenuButton, Vector2> submenuPositions = new Dictionary<SubmenuButton, Vector2>();

    
    


    protected override void OnRelease()
    {
        AppManager.SubmenuChangedViaScrolling(WarpToPosition());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Start()
    {
        base.Start();

        rectTransform = GetComponent<RectTransform>();
        scrollRect = GetComponent<ScrollRect>();
        uiSizer = FindObjectOfType<AutoSizeUI>();

        oneSubmenuPercentage = 1f / (uiSizer.GetNumberOfMenus() - 1);


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

            Transform _child = submenusRectTransform.GetChild(i);
            SubmenuPosition _positioner = _child.GetComponentInChildren<SubmenuPosition>();

            if (_positioner == null)
            {
                Debug.LogError($"No submenu positioner on {gameObject.name}");
            }

            _positioner.transform.SetParent(transform.root, true);

            submenuPositions.Add(_attachButton, _positioner
                .GetComponent<RectTransform>().anchoredPosition);

            Destroy(_positioner.gameObject);
        }

        AppManager.SubmenuChangedViaScrolling(WarpToPosition(0));
    }

   

    public int WarpToPosition(int _submenuId = -1)
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
                return _submenuId;
                
            }
        }
        Debug.LogError($"The Submenu ID to warp to does not appear in the {nameof(submenuPositions)} list: {_submenuId}");


        return -1;
    }

    public int CalculateWarpPosition()
    {
        int _closest = 1;
        float _distance = Mathf.Infinity;
        for (int i = 0; i < uiSizer.GetNumberOfMenus(); i++)
        {

            if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - (i * oneSubmenuPercentage)) < _distance)
            {
                //    print("Closest: " + _closest);
                _distance = Mathf.Abs(scrollRect.horizontalNormalizedPosition - (i * oneSubmenuPercentage));
                _closest = i;
            }
        }

        return _closest;
    }

    IEnumerator Warp(int _id)
    {
        

        scrollRect.horizontalNormalizedPosition = _id * oneSubmenuPercentage;

        yield return new WaitForEndOfFrame();

    }
}
