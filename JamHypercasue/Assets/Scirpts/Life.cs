using DG.Tweening;
using IIMEngine.SFX;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    [SerializeField] private GameObject[] UILife;
    [SerializeField] private Color _lifeColor;
    [SerializeField] private Color _noLifeColor;
    private int life = 3;
    public void TakeDamage(int damage)
    {
        life -= damage;
        UILife[life].GetComponent<Image>().color = _noLifeColor;
        foreach (var cam in FindObjectsByType<Camera>(UnityEngine.FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            cam.DOShakePosition(0.5f, 5f, 10, 90);
        }
        SFXsManager.Instance.PlaySound("LostLive");
        if (life <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        SFXsManager.Instance.PlaySound("GameOver");
        GameManager.Instance.Explode();
    }
    public void Restart()
    {
        life = 3;
        for (int i = 0; i < UILife.Length; i++)
        {
            UILife[i].GetComponent<Image>().color = _lifeColor;
        }
    }
    public void AddLife()
    {
        if (life < 3)
        {
            UILife[life].GetComponent<Image>().color = _lifeColor;
            life++;
        }
    }
}
