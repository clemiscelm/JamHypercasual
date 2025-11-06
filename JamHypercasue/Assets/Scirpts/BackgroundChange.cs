using UnityEngine;
using UnityEngine.UI;

public class BackgroundChange : MonoBehaviour
{
    public GameObject[] backgrounds;
    public GameObject[] content;
    

    public void changeBackGround(bool state)
    {
        if (state)
        {
            backgrounds[0].SetActive(false);
            backgrounds[1].SetActive(true);
            content[0].SetActive(true);
            content[1].SetActive(false);
        }
        else
        {
            backgrounds[0].SetActive(true);
            backgrounds[1].SetActive(false);
            content[0].SetActive(false);
            content[1].SetActive(true);
            
        }
    }

    
}
