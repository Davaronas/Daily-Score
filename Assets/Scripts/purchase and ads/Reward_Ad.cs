using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class Reward_Ad : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    string gameId = "4205264";
    string mySurfacingId = "Rewarded_iOS";
#else
    string gameId = "4205265";
   // 4226553
    string mySurfacingId = "Rewarded_Android";
#endif
    bool TestMode = true;

    public static Action OnTipVideoSuccesfullyWatched;

    void Start()
    {
        print(gameId);
        Advertisement.Initialize(gameId, TestMode);
    }

    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(mySurfacingId))
        {
            Advertisement.Show(mySurfacingId);
            
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    public void ShowAdvertisementVideo()
    {
        if (Advertisement.IsReady(mySurfacingId))
        {
            //  Advertisement.Show(mySurfacingId);
            print("Banner");
            Advertisement.Banner.Load();
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish (string surfacingId, ShowResult showResult) {

        if(surfacingId != "Rewarded_Android") { return; }

        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            OnTipVideoSuccesfullyWatched?.Invoke();
            // Reward the user for watching the ad to completion.
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady (string surfacingId) {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId) {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string surfacingId) {
        // Optional actions to take when the end-users triggers an ad.
    } 

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy() {
        Advertisement.RemoveListener(this);
    }
}
