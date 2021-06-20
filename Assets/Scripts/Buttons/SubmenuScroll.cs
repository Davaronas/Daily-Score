using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


[RequireComponent(typeof(ScrollRect))]

public class SubmenuScroll : BehaviourButton, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]private RectTransform submenusRectTransform = null;
    [Space]
    [Space]
    [Space]
    [SerializeField][Range(0.000001f,0.001f)] private float warpCalculationDifferenceTreshold = 0.0005f;
    [SerializeField] [Range(0.005f,0.2f)] private float warpSpeed = 0.01f;
    [SerializeField][Range(0.01f,0.5f)] private float warpAfterReleaseTime = 0.1f;




    float oneSubmenuPercentage = 0f;

    private RectTransform rectTransform = null;
    private ScrollRect scrollRect = null;
    private AutoSizeUI uiSizer = null;

    private int currentPos = -1;


    private Dictionary<SubmenuButton, Vector2> submenuPositions = new Dictionary<SubmenuButton, Vector2>();

    // Aid variables
    private float warpPercentage_ = 0;
    private float horizontalPos_ = 0;
    private bool breakWarp_ = false;
    private bool onEnableUnlocked_ = false;

    public void DEBUG_ChangeWarpSpeed(TMPro.TMP_InputField _if)
    {
        float _warpSpeed;
        if (float.TryParse(_if.text, out _warpSpeed))
        {
            warpSpeed = _warpSpeed;
        }
        else
        {
            _if.text = "";
        }
       
    }

    public void DEBUG_ChangeWarpAfterReleaseTIme(TMPro.TMP_InputField _if)
    {
        float _time;
        if(float.TryParse(_if.text,out _time))
        {
            warpAfterReleaseTime = _time;
        }
        else
        {
            _if.text = "";
        }

        
    }

    


    protected override void OnTouch()
    {
       
    }


    

    protected override void OnRelease()
    {
       
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        breakWarp_ = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        breakWarp_ = false;
        Invoke(nameof(StartWarping), warpAfterReleaseTime);
    }

  

    public void EnableDrag()
    {
        scrollRect.enabled = true;
    }



    private void StartWarping()
    {
        if(!breakWarp_)
        AppManager.SubmenuChangedViaScrolling(WarpToPosition());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnEnable()
    {
        if (onEnableUnlocked_)
            WarpToPosition(-1, true);
        else
            onEnableUnlocked_ = true;
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

        AppManager.SubmenuChangedViaScrolling(WarpToPosition(0,true));
    }

   

    public int WarpToPosition(int _submenuId = -1, bool _instantWarp = false)
    {
        if(_submenuId == -1)
        {
            _submenuId = CalculateWarpPosition();
        }



        foreach(KeyValuePair<SubmenuButton,Vector2> _keyValuePair in submenuPositions)
        {
            if (_keyValuePair.Key.GetId() == _submenuId)
            {
                if (!_instantWarp)
                    StartCoroutine(Warp(_keyValuePair.Key.GetId()));
                else
                    InstantWarp(_keyValuePair.Key.GetId());
                
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

    private void InstantWarp(int _id)
    {
        currentPos = _id;
        warpPercentage_ = currentPos * oneSubmenuPercentage;
        scrollRect.horizontalNormalizedPosition = warpPercentage_;
    }

    IEnumerator Warp(int _id)
    {
       


        currentPos = _id;
        warpPercentage_ = currentPos * oneSubmenuPercentage;

        while (true)
        {
            horizontalPos_ = scrollRect.horizontalNormalizedPosition;
            scrollRect.horizontalNormalizedPosition =
                 Mathf.Lerp(horizontalPos_, warpPercentage_, warpSpeed);



            if(Mathf.Abs(horizontalPos_ - warpPercentage_) < warpCalculationDifferenceTreshold)
            {
                scrollRect.horizontalNormalizedPosition = warpPercentage_;
                break;
            }
            else if(breakWarp_)
            {
                break;
            }
            
            yield return new WaitForEndOfFrame();
        }



        //scrollRect.horizontalNormalizedPosition = warpPercentage_;



       // Vector3.Lerp(scrollRect.transform.position, transform.position)

        yield return new WaitForEndOfFrame();

    }

    public int GetCurrentPosition()
    {
        return currentPos;
    }

   
}
