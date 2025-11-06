using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsShop : MonoBehaviour
{
    public Button[] buttons;
    public bool CanBuyHard(int price)
    {
        if(price <= PlayerData.GetHardCurrency())
            return false;
        return true;
    }

    public bool CanBuySoft(int price)
    {
        if (price <= PlayerData.GetSoftCurrency())
            return false;
        return true;
    }

    

    private void OnDestroy()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public void ClickButtonHard(int price)
    {
        if (!CanBuyHard(price))
            return;
        
        PlayerData.DeacriseHardCurrency(price);
        if(price == 20)
            PlayerData.IncriseSoftCurrency(250);
        else if(price == 10)
            PlayerData.IncriseSoftCurrency(100);
        else if(price == 80)
            PlayerData.IncriseSoftCurrency(1000);
    }
    public void ClickButtonSoft(int price)
    {
        if (!CanBuySoft(price))
            return;
        
        PlayerData.DeacriseSoftCurrency(price);
        if (price == 5000)
        {
            if(PlayerPrefs.GetInt("Skin_3") == 1)
                return;

            //skin sayan rouge
            PlayerPrefs.SetInt("Skin_3", 1);
        }
        else if (price == 10000)
        {
            if(PlayerPrefs.GetInt("Skin_2") == 1)
                return;

            PlayerPrefs.SetInt("Skin_2", 1);
            //skin sayan gris
        }
    }

    public void ClickButtonLuffy()
    {
        
        if(PlayerPrefs.GetInt("Skin_5") == 1)
            return;
        
        int price = 95;
        if (!CanBuyHard(price))
            return;
        
        PlayerPrefs.SetInt("Skin_5", 1);
        PlayerData.DeacriseHardCurrency(price);
    }
    public void ClickButtonSayanGold()
    {
        if(PlayerPrefs.GetInt("Skin_4") == 1)
            return;

        int price = 75;
        if (!CanBuyHard(price))
            return;
        
        PlayerPrefs.SetInt("Skin_4", 1);
        PlayerData.DeacriseHardCurrency(price);
    }
    public void ClickButtonNaruto()
    {
        if(PlayerPrefs.GetInt("Skin_1") == 1)
            return;

        int price = 200;
        if (!CanBuyHard(price))
            return;
        
        PlayerPrefs.SetInt("Skin_1", 1);
        PlayerData.DeacriseHardCurrency(price);
    }

    public void ClickButtonAdd()
    {
        AdManager.Instance.ShowRewardedAd();
        PlayerData.IncriseSoftCurrency(10);
    }

    
    
}
