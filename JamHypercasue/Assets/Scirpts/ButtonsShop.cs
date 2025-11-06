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

    private void Awake()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(ClickButton);
        }
    }

    private void OnDestroy()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public void ClickButton()
    {
        if(CanBuySoft(10))
            PlayerData.DeacriseSoftCurrency(10);
    }
}
