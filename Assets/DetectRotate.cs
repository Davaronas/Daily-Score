using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectRotate : UIBehaviour
{
    private SubmenuScroll submenuScroll = null;

    private bool startHappened = false;

    private int warpPos = -1;

    protected override void Awake()
    {
        base.Awake();

        submenuScroll = FindObjectOfType<SubmenuScroll>();
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(WaitForFrameEnd());
    }



    IEnumerator WaitForFrameEnd()
    {
        yield return new WaitForEndOfFrame();
        startHappened = true;
    }

    protected override void OnRectTransformDimensionsChange()
    {
        print($"Dimension change on {gameObject.name}");



        if(submenuScroll == null ) { Debug.Log("null"); return; }

        if(!submenuScroll.gameObject.activeInHierarchy  ) { Debug.Log("Not active"); return; }

        if(!startHappened) { Debug.Log("Start not happened"); return; }

        warpPos = submenuScroll.CalculateWarpPosition();

        StartCoroutine(WaitForFrameEnd2());


        /*

        if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {


            return;
        }

        if(Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {


            return;
        }
        */
    }

    IEnumerator WaitForFrameEnd2()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        submenuScroll.WarpToPosition(warpPos);
    }
}
