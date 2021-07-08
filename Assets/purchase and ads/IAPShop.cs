using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPShop : MonoBehaviour
{
    private string goods = "com.balazs.dailyscore.gold";

    public void OnPurchaseComplete(Product gold)
    {
        if (gold.definition.id == goods)
        {
            //reward the player for buying gold
            Debug.Log("Purchase Successful!");
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
