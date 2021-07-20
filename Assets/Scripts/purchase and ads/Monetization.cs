using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Monetization : MonoBehaviour
{
#if UNITY_IOS
    string gameId = "4205264";
    string ad = "Interstitial_iOS";
#else
    string gameId = "4205265";
    string ad = "Interstitial_Android";
#endif
    bool TestMode = true;

    void Start()
    {
       // print(gameId);
        Advertisement.Initialize(gameId, TestMode);
    }

    public void PlayAd()
    {
        if (Advertisement.IsReady(ad))
        {
            Advertisement.Show(ad);         
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }

    }
    
}
