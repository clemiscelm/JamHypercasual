using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsShop : MonoBehaviour
{
    public Button[] buttons;
    public List<Button> buttonsBuy;
    int idSkinChose = 0;
    public bool CanBuyHard(int price)
    {
        if(price > PlayerData.GetHardCurrency())
            return false;
        return true;
    }

    private void Start()
    {
        setup();
    }

    public void setup()
    {
        
        
        if (PlayerPrefs.GetInt("Skin_1") == 1)
        {
            buttonsBuy.Add(buttons[4]);
            buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            buttons[4].GetComponentsInChildren<Image>()[1].enabled = false;
        }
        if (PlayerPrefs.GetInt("Skin_2") == 1)
        {
            buttonsBuy.Add(buttons[1]);
            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            buttons[1].GetComponentsInChildren<Image>()[1].enabled = false;
        }
        if (PlayerPrefs.GetInt("Skin_3") == 1)
        {
            buttonsBuy.Add(buttons[0]);
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            buttons[0].GetComponentsInChildren<Image>()[1].enabled = false;
        }
        if (PlayerPrefs.GetInt("Skin_4") == 1)
        {
            buttonsBuy.Add(buttons[3]);
            buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            buttons[3].GetComponentsInChildren<Image>()[1].enabled = false;
        }
        if (PlayerPrefs.GetInt("Skin_5") == 1)
        {
            buttonsBuy.Add(buttons[2]);
            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            buttons[2].GetComponentsInChildren<Image>()[1].enabled = false;
        }
        
    }

    public bool CanBuySoft(int price)
    {
        if (price > PlayerData.GetSoftCurrency())
            return false;
        return true;
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
        PlayerData.DeacriseSoftCurrency(price);
        if (price == 5000)
        {
            if (PlayerPrefs.GetInt("Skin_3") == 1)
            {
                GameManager.Instance.IDskinChose = 3;
                foreach (var button in buttonsBuy)
                {
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
                }
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
                buttons[0].GetComponentsInChildren<Image>()[1].enabled = false;
                return;
            }
            if (!CanBuySoft(price))
                return;
                print("buy");
            
            buttonsBuy.Add(buttons[0]);
            foreach (var button in buttonsBuy)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            }
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
            buttons[0].GetComponentsInChildren<Image>()[1].enabled = false;
            GameManager.Instance.IDskinChose = 3;
            //skin sayan rouge
            PlayerPrefs.SetInt("Skin_3", 1);
        }
        else if (price == 10000)
        {
            if (PlayerPrefs.GetInt("Skin_2") == 1)
            {
                GameManager.Instance.IDskinChose = 2;
                foreach (var button in buttonsBuy)
                {
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
                }
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
                buttons[1].GetComponentsInChildren<Image>()[1].enabled = false;
                return;
            }
            if (!CanBuySoft(price))
                return;
            buttonsBuy.Add(buttons[1]);
            foreach (var button in buttonsBuy)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            }
            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
            buttons[1].GetComponentsInChildren<Image>()[1].enabled = false;
            GameManager.Instance.IDskinChose = 2;
            PlayerPrefs.SetInt("Skin_2", 1);
            //skin sayan gris
        }
    }

    public void ClickButtonLuffy()
    {

        
        int price = 95;
        if (PlayerPrefs.GetInt("Skin_5") == 1)
        {
            GameManager.Instance.IDskinChose = 5;
            foreach (var button in buttonsBuy)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            }
            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
            buttons[2].GetComponentsInChildren<Image>()[1].enabled = false;
                print("change");
            
            return;
        }
        if (!CanBuyHard(price))
            return;
                print("buy");
        
        buttonsBuy.Add(buttons[2]);
        foreach (var button in buttonsBuy)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
        }
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
        buttons[2].GetComponentsInChildren<Image>()[1].enabled = false;
        PlayerPrefs.SetInt("Skin_5", 1);
        GameManager.Instance.IDskinChose = 5;
        PlayerData.DeacriseHardCurrency(price);
    }
    public void ClickButtonSayanGold()
    {
        if (PlayerPrefs.GetInt("Skin_4") == 1)
        {
            GameManager.Instance.IDskinChose = 4;
            foreach (var button in buttonsBuy)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            }
            buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
            buttons[3].GetComponentsInChildren<Image>()[1].enabled = false;
            return;
        }

        int price = 75;
        if (!CanBuyHard(price))
            return;
        buttonsBuy.Add(buttons[3]);
        foreach (var button in buttonsBuy)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
        }
        buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
        buttons[3].GetComponentsInChildren<Image>()[1].enabled = false;
        GameManager.Instance.IDskinChose = 4;
        
        PlayerPrefs.SetInt("Skin_4", 1);
        PlayerData.DeacriseHardCurrency(price);
    }
    public void ClickButtonNaruto()
    {
        if (PlayerPrefs.GetInt("Skin_1") == 1)
        {
            GameManager.Instance.IDskinChose = 1;
            foreach (var button in buttonsBuy)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
            }
            buttons[4].GetComponentsInChildren<Image>()[1].enabled = false;
            buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
            GameManager.Instance.IDskinChose = 4;
            
            return;
        }

        int price = 200;
        if (!CanBuyHard(price))
            return;
        buttonsBuy.Add(buttons[4]);
        foreach (var button in buttonsBuy)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "CHOSE";
        }
        print("here");
        buttons[4].GetComponentsInChildren<Image>()[1].enabled = false;
        buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
        GameManager.Instance.IDskinChose = 4;
        PlayerPrefs.SetInt("Skin_1", 1);
        GameManager.Instance.IDskinChose = 1;
        PlayerData.DeacriseHardCurrency(price);
    }

    public void ClickButtonAdd()
    {
        AdManager.Instance.ShowRewardedAd();
        PlayerData.IncriseSoftCurrency(10);
    }

    
    
}
