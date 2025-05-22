using System.Collections.Generic;
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
    private Vector3 _posTemp;
    private Vector3 _posTemp2;
    Dictionary<Camera, Tweener> _cameras = new Dictionary<Camera, Tweener>();

    private void Start()
    {
        var temp = FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        _posTemp = temp[0].transform.position;
        _posTemp2 = temp[1].transform.position;
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        UILife[life].GetComponent<Image>().color = _noLifeColor;
        
        
        foreach (var cam in FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            
            
            var t =
            cam.DOShakePosition(0.5f, 5f, 10, 90).OnComplete(() =>
            {
                var temp = FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                temp[0].transform.position =  _posTemp;
                temp[1].transform.position = _posTemp2;
            }); 
            
            
            
            
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
