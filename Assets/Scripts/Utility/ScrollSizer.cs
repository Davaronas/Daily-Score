using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollSizer
{
    private static float content_X;
    private static float content_Y;
    private static Vector2 contentSize;
    private static float goalsContentDynamicSize;
    private static float goalsContentParent_Y;

    private static RectTransform scrollContentParent;
    private static ScrollRect scrollRect;

    public static void Resize(RectTransform _scroll, float _oneUnitSize, int _units)
    {
        scrollContentParent = _scroll.parent.GetComponent<RectTransform>();
        goalsContentParent_Y = scrollContentParent.rect.size.y;
        scrollRect = scrollContentParent.GetComponent<ScrollRect>();


        content_X = _scroll.sizeDelta.x;
        goalsContentDynamicSize = _units * _oneUnitSize;

        if(goalsContentDynamicSize > goalsContentParent_Y)
        {
            content_Y = goalsContentDynamicSize;
            
        }
        else
        {
            content_Y = goalsContentParent_Y;
        }

        contentSize = new Vector2(content_X, content_Y);
        _scroll.sizeDelta = contentSize;
        

    }

    public static void AddSize(RectTransform _scroll, float _amount)
    {

    }

    public static void RemoveSize(RectTransform _scroll, float _amount)
    {

    }
}
