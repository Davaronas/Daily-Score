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


        AppManager.OnAppLayerChangedToMainMenu += UpdateScreenSubmenuPosition;
        submenuScroll = FindObjectOfType<SubmenuScroll>();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        AppManager.OnAppLayerChangedToMainMenu -= UpdateScreenSubmenuPosition;
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(EnableDetectionAfterStart());
    }



    IEnumerator EnableDetectionAfterStart()
    {
        yield return new WaitForEndOfFrame();
        startHappened = true;
    }



    protected override void OnRectTransformDimensionsChange()
    {
        print($"Dimension change on {gameObject.name}");


        UpdateScreenSubmenuPosition();


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

    private void UpdateScreenSubmenuPosition()
    {
        if (submenuScroll == null) { return; }

        if (!submenuScroll.gameObject.activeInHierarchy) { return; }

        if (!startHappened) { return; }

        warpPos = submenuScroll.GetCurrentPosition();

        StartCoroutine(SubmenuScrollWarpAfterFrame());
    }

    IEnumerator SubmenuScrollWarpAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        submenuScroll.WarpToPosition(warpPos);
    }
}
