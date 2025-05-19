using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _menuCanva;
    [SerializeField] private Button _playButton;
    [SerializeField] private TMP_Text _levelIndicator;

    private void Awake()
    {
        _levelIndicator.text = PlayerData.GetPlayerCurrentLevel().ToString();
        _playButton.onClick.AddListener(Play);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
    }


    private void Play()
    {
        SetMainMenuActive(false);
    }
    
    private void SetMainMenuActive(bool value)
    {
        if (value)
        {
            _menuCanva.DOFade(1, .5f).SetEase(Ease.InCubic);
        }
        else
        {
            _menuCanva.DOFade(0, .5f).SetEase(Ease.InCubic);
        }
    }
}