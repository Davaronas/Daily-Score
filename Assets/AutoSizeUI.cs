using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AutoSizeUI : MonoBehaviour
{
    [SerializeField] private RectTransform ui = null;
    [SerializeField] private int numberOfMenus = 5;

    public int GetNumberOfMenus()
    {
        return numberOfMenus;
    }

    // Start is called before the first frame update
    void Awake()
    {
        Vector2 _uiSize = ui.sizeDelta;
        _uiSize.x = Screen.width * numberOfMenus;
        ui.sizeDelta = _uiSize;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Application.isEditor) { return;}

        Vector2 _uiSize = ui.sizeDelta;
        _uiSize.x = Screen.width * numberOfMenus;
        ui.sizeDelta = _uiSize;
    }
}
