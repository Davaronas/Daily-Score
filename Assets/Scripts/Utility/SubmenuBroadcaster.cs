using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmenuBroadcaster : ScrollDragBroadcast
{
    [SerializeField] private ScrollRect submenuScrollRect = null;
    private SubmenuScroll submenuScroll = null;

    private ScrollRect scrollRect = null;


    private void Awake()
    {
        if (submenuScrollRect == null)
        {
            Debug.LogError("SubmenuScroll not set up on GoalsScroll");
            return;
        }
        submenuScroll = submenuScrollRect.GetComponent<SubmenuScroll>();
        scrollRect = GetComponent<ScrollRect>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // base.OnBeginDrag(eventData);

        scrollRect.enabled = false;
        submenuScrollRect.OnBeginDrag(eventData);
    }


    public override void OnDrag(PointerEventData eventData)
    {
      //base.OnDrag(eventData);

      // submenuScrollRect.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        //base.OnEndDrag(eventData);

        submenuScrollRect.OnEndDrag(eventData);
        AppManager.SubmenuChangedViaScrolling(submenuScroll.WarpToPosition());
    }
}
