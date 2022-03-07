using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPShop : MonoBehaviour
{
    private string goods = "com.balazs.dailyscore.gold";

    private AppManager appManager;

    private void Awake()
    {
        appManager = FindObjectOfType<AppManager>();

        /*
         SubscriptionManager _manager = new SubscriptionManager(new Product(goods), "");
         SubscriptionInfo _info = _manager.getSubscriptionInfo();

         if(_info.isSubscribed() == Result.True)
         {

         }
        */

        SubscriptionInfo _info = new SubscriptionManager("", goods, "").getSubscriptionInfo();
        bool _state = _info.isSubscribed() == Result.True ? true : false;
        appManager.SetGoldState(_state);
      
    }

   

    public void OnPurchaseComplete(Product gold)
    {

        SubscriptionManager _manager = new SubscriptionManager(gold, "");
        
        SubscriptionInfo _info = _manager.getSubscriptionInfo();




        if (gold.definition.id == goods)
        {
            //reward the player for buying gold
            Debug.Log("Purchase Successful!");
            appManager.SetGoldState(true);
        }
        else
        {
            Debug.Log("failed purchase...");
        }
    }

    public void OnPurchaseFailed(Product gold, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase of " + gold.definition.id + "failed due to " + reason);
    }

 
        



    
}
