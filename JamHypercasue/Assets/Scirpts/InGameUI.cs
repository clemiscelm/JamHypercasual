using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;
    
    [SerializeField] private TMP_Text _scoreText;
    private int _combo;

    private int Combo
    {
        get => _combo;
        set
        {
            _combo = value;
            _scoreText.text = _combo.ToString();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [Button]
    public void IncrementCombo()
    {
        Combo++;
        (_scoreText.transform as RectTransform).DOPunchScale(Vector3.one * 1.5f, .2f).SetEase(Ease.InOutCubic);
    }

    [Button]
    public void LooseCombo()
    {
        if(Combo == 0)
            return;
        
        Combo = 0;
        (_scoreText.transform as RectTransform).DOScale(Vector3.one, .2f).SetEase(Ease.InOutCubic);
    }
}
