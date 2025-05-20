using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private GameObject[] UILife;
    private int life = 3;
    public void TakeDamage(int damage)
    {
        life -= damage;
        UILife[life].SetActive(false);

        if (life <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        GameManager.Instance.Explode();
    }
    public void Restart()
    {
        life = 3;
        for (int i = 0; i < UILife.Length; i++)
        {
            UILife[i].SetActive(true);
        }
    }
    public void AddLife()
    {
        if (life < 3)
        {
            UILife[life].SetActive(true);
            life++;
        }
    }
}
