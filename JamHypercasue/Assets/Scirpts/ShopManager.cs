using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class ShopManager : MonoBehaviour
{
    
    public void buyLarge()
    {
        PlayerData.IncriseHardCurrency(255);
    }
    public void buyMeduim()
    {
        PlayerData.IncriseHardCurrency(45);
    }
    public void buySmal()
    {
        PlayerData.IncriseHardCurrency(15);
    }
    
}
