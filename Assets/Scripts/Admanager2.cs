using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Admanager2 : MonoBehaviour, IUnityAdsListener

{
    string placement = "rewardedVideo";

    IEnumerator Start()
    {
        Advertisement.AddListener(this); //Ez nem tudom mi Xdd
        Advertisement.Initialize("4112611", true); //A szám benne az az app Google Play ID-ja amit egy külön weblapon kell beaddolnalak

        while (!Advertisement.IsReady(placement))
            yield return null;

        Advertisement.Show(placement);
    }
    //IUntiyAdListener ezt kellene behoznia és beszédes hogy melyik.
    public void OnUnityAdsDidFinnish(string placementId, ShowResult showResult)
    {
        if(showResult == ShowResult.Finished)
        {
            //Ide kell az ajándék
        }
        else if(showResult ==showResult.Failed)
        {
            //Oh no! Üzenet
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {
    }
    public void OnUnityAdsDidError(string message)
    {
    }

}
