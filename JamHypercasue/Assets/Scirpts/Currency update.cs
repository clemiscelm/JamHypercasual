using TMPro;
using UnityEngine;

public class Currencyupdate : MonoBehaviour
{
   public TextMeshProUGUI softCurrency;  
   public TextMeshProUGUI HardCurrency;  
   
   void Update()
   {
      HardCurrency.text = "Gems: " + PlayerData.GetHardCurrency().ToString();
      softCurrency.text = "Coins: " + PlayerData.GetSoftCurrency().ToString();
   }

   
}
