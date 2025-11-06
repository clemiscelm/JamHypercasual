using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _menuCanva;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _ShopButton;
    [SerializeField] private Button _BackShopButton;
    [SerializeField] private TMP_Text _levelIndicator;
    [SerializeField] private RectTransform _iconRect;

    [Header("Settings")] 
    [SerializeField] private CanvasGroup _settings;
    [SerializeField] private CanvasGroup _shop;

    private Quaternion _rotation;

    private void Awake()
    {
        _levelIndicator.text = PlayerData.GetPlayerCurrentLevel().ToString();
        _playButton.onClick.AddListener(Play);
        _settingsButton.onClick.AddListener(() => OpenSettings(true));
        _ShopButton.onClick.AddListener(() => OpenShop(true));
        _BackShopButton.onClick.AddListener(() => OpenShop(false));
        _rotation = _iconRect.transform.rotation;
        SetMainMenuActive(true);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        _ShopButton.onClick.RemoveAllListeners();
        _BackShopButton.onClick.RemoveAllListeners();
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
            _menuCanva.interactable = true;
            _menuCanva.blocksRaycasts = true;
            _iconRect.DORotate(Vector3.forward * -30, 2.5f).SetEase(Ease.InOutFlash).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            _menuCanva.interactable = false;
            _menuCanva.blocksRaycasts = false;
            _menuCanva.DOFade(0, .5f).SetEase(Ease.InCubic).OnComplete((() =>
            {
                _iconRect.DOKill();
                _iconRect.transform.rotation = _rotation;
                GameManager.Instance.StartGame();
            }));
        }
    }

    private void OpenSettings(bool value)
    {
        if (value)
        {
            _settings.DOFade(1, .2f).SetEase(Ease.InOutFlash);
            _menuCanva.interactable = true;
            _menuCanva.blocksRaycasts = true;
        }
        else
        {
            _settings.DOFade(0, .2f).SetEase(Ease.InOutFlash);
            _menuCanva.interactable = false;
            _menuCanva.blocksRaycasts = false;
        }
    }

    private void OpenShop(bool value)
    {
        if (value)
        {
            _menuCanva.gameObject.SetActive(false);
            _settings.gameObject.SetActive(false);
            _shop.gameObject.SetActive(true);
            _shop.DOFade(1, .2f).SetEase(Ease.InOutFlash);
            _shop.interactable = true;
            _shop.blocksRaycasts = true;
        }
        else
        {
            _menuCanva.gameObject.SetActive(true);
            _settings.gameObject.SetActive(true);
            _shop.gameObject.SetActive(false);
            _shop.DOFade(0, .2f).SetEase(Ease.InOutFlash);
            _shop.interactable = false;
            _shop.blocksRaycasts = false;
        }
    }
}