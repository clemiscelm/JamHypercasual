using TMPro;
using UnityEngine;

public class Currencyupdate : MonoBehaviour
{
   public TextMeshProUGUI softCurrency;  
   public TextMeshProUGUI HardCurrency;  
   
   void Update()
   {
      HardCurrency.text = PlayerData.GetHardCurrency().ToString();
      softCurrency.text = PlayerData.GetSoftCurrency().ToString();
   }

   
}
