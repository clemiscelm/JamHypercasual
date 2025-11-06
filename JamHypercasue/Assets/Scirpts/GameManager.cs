using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using IIMEngine.SFX;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Blade blade;
    [SerializeField] private Spawner spawner;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] public float time = 0f;
    [SerializeField] private float maxTime = 30f;
    [SerializeField] private float BasemaxTime = 30f;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI TextLevel;
    public bool isGameRunning = false;
    private bool canNewGame = false;
    private bool needNewGame = false;
    private bool isLose = false;
    private bool canSkip = false;
    private bool SkipAdAfterRewared = false;
    private bool levelSkipped = false;
    [SerializeField] private GameObject[] winGameObjects;
    int level = 0;

    [Header("Pause")] 
    [SerializeField] private CanvasGroup _pauseCanva;
    [SerializeField] private Button[] _pauseButtons;
    [SerializeField] private Button[] _unpauseButtons;
    [SerializeField] private Button _returnToMainMenu;
    [SerializeField] private GameObject[] _objectsToPause;

    [Header("Lose")] 
    [SerializeField] private CanvasGroup _gameOverMenu;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _reviveButton;
    [SerializeField] private TMP_Text _scoreGameover;
    public int IDskinChose = 0;
    

    private int[] _availableSkins;
    public int[] AvailableSkins => _availableSkins; 

    public int score { get; private set; } = 0;
    
    
    private void Awake()
    {
        InitSkins();
        _restartButton.onClick.AddListener(NeedNewGameNonRewared);
        //_restartButton.onClick.AddListener(NewGame);
        _reviveButton.onClick.AddListener(NeedNewGameRewarded);
        _reviveButton.onClick.AddListener(NewGame);
        _returnToMainMenu.onClick.AddListener(ReturnToMainMenu);
        
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }

        foreach (Button el in _pauseButtons)
        {
            el.onClick.AddListener(Pause);
        }
        
        foreach (Button el in _unpauseButtons)
        {
            el.onClick.AddListener(UnPause);
        }
    }

    public void InitSkins()
    {
        List<int> skins = new List<int>();
        List<int> skinsId = new List<int>();
        skins.Add(PlayerPrefs.GetInt("Skin_1"));
        skins.Add(PlayerPrefs.GetInt("Skin_2"));
        skins.Add(PlayerPrefs.GetInt("Skin_3"));
        skins.Add(PlayerPrefs.GetInt("Skin_4"));
        skins.Add(PlayerPrefs.GetInt("Skin_5"));
        for (int i = 0; i < skins.Count; i++)
        {
            if (skins[i] == 1)
            {
                skinsId.Add(i);
            }
        }
        _availableSkins = skinsId.ToArray();
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
        
        _restartButton.onClick.RemoveAllListeners();
        _reviveButton.onClick.RemoveAllListeners();
        _returnToMainMenu.onClick.RemoveAllListeners();
        
        foreach (Button el in _pauseButtons)
        {
            el.onClick.RemoveAllListeners();
        }
        
        foreach (Button el in _unpauseButtons)
        {
            el.onClick.RemoveAllListeners();
        }
    }

    private void Start()
    {
         SFXsManager.Instance.PlaySound("Ambiance");
         int lvl = PlayerData.GetPlayerCurrentLevel();
        if (lvl != 1)
        {
            if(lvl % 5 == 0)
            {
                spawner.resetChance();
                maxTime = BasemaxTime;
                spawner.bombChance += 0.01f * lvl;
                spawner.luckyChance -= 0.01f * lvl;
                spawner.comboChance += 0.01f * lvl;
                spawner.minSpawnDelay -= 0.005f * lvl;
                spawner.maxSpawnDelay -= 0.005f * lvl;
                maxTime += 2f * lvl;
                if(spawner.bombChance > 0.25f)
                    spawner.bombChance = 0.25f;
                if (spawner.luckyChance < 0)
                    spawner.luckyChance = 0;
                if (spawner.comboChance > 0.25f)
                    spawner.comboChance = 0.25f;
                    
            }
        }
    }

    private void Update()
    {
        scoreText.text = time.ToString() + " / " + maxTime.ToString();
        if ((time >= maxTime && !isLose) || canSkip)
        {
            time = maxTime;
            PlayerData.InccrementPlayerLevel();
            SFXsManager.Instance.PlaySound("Level Up");
            resetGame();
            blade.gameObject.SetActive(false);
            spawner.gameObject.SetActive(false);
            foreach (var go in winGameObjects)
            {
                go.SetActive(true);
            }
            canNewGame = true;
        }

        if(canNewGame && !AdManager.Instance.AdDisplaying)
        {
            if (Input.GetMouseButtonDown(0) && !AdManager.Instance.AdDisplaying)
            {
                needNewGame = true;
                canNewGame = false;
                if (PlayerPrefs.GetString("LastDate") != DateTime.Today.ToString())
                {
                    PlayerPrefs.SetInt("AdSes", PlayerPrefs.GetInt("AdSes") + 1);
                    if((PlayerPrefs.GetInt("AdSes") == 3))
                    {
                        PlayerPrefs.SetString("LastDate", DateTime.Today.ToString());
                        PlayerPrefs.SetInt("AdSes", 0);
                    }
                    Debug.Log($"Daily levels {PlayerPrefs.GetInt("AdSes")}");
                }
                else
                {
                    if(!SkipAdAfterRewared)
                        AdManager.Instance.ShowAd();
                    SkipAdAfterRewared = false;
                }
                
                foreach (var go in winGameObjects)
                {
                    go.SetActive(false);
                }
                
            }
        }
        slider.value = time / maxTime;
        if(needNewGame && !AdManager.Instance.AdDisplaying)
        {
            NewGame();
        }
        canSkip = false;

        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(0);
        }
        
    }

    public void StartGame()
    {
        needNewGame = true;
        canNewGame = false;
    }

    private void resetGame()
    {
        time = 0f;
        FindAnyObjectByType<Life>().Restart();
        isGameRunning = false;
        level++;
        
        TextLevel.text = PlayerData.GetPlayerCurrentLevel().ToString();
        
        if (PlayerData.GetPlayerCurrentLevel() == 1)
            maxTime = 10;
        else if(PlayerData.GetPlayerCurrentLevel() == 2)
            maxTime = 15;
        
        if(PlayerData.GetPlayerCurrentLevel() % 5 == 0)
        {
            spawner.bombChance += 0.01f;
            spawner.minSpawnDelay -= 0.05f;
            spawner.maxSpawnDelay -= 0.05f;
            maxTime += 5f;
            if(spawner.bombChance > 0.25f)
                spawner.bombChance = 0.25f;
        }
        ClearScene();
    }
    private void NewGame()
    {
        if(needNewGame == false)
        {
            return;
        }
        
        AnalyticsManager.Instance.OnLevelPass(PlayerPrefs.GetInt(ProjectConst.PlayerLevel), levelSkipped);
        levelSkipped = false;
        
        var fruits = FindObjectsByType<Fruit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        var Metalfruits = FindObjectsByType<MetalFruit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var fruit in fruits)
        {
            fruit.isTamere = true;
            Destroy(fruit.gameObject);
        }
        foreach (var fruit in Metalfruits)
        {
            fruit.isTamere = true;
            Destroy(fruit.gameObject);
        }
        
        needNewGame = false;
        resetGame();
        Time.timeScale = 1f;
        isGameRunning = true;
        blade.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        blade.enabled = true;
        spawner.enabled = true;

        _gameOverMenu.DOFade(0, 0.2f).SetEase(Ease.InFlash);
        _gameOverMenu.interactable = false;
        _gameOverMenu.blocksRaycasts = false;
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits) {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs) {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        

        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }
    }

    private void Pause()
    {
        foreach (GameObject el in _objectsToPause)
        {
            el.SetActive(false);
        }
        _pauseCanva.DOFade(1, .2f).SetEase(Ease.InFlash);
        _pauseCanva.interactable = true;
        _pauseCanva.blocksRaycasts = true;
    }
    
    private void UnPause()
    {
        if (isGameRunning)
        {
            foreach (GameObject el in _objectsToPause)
            {
                el.SetActive(true);
            }
        }
        _objectsToPause[0].SetActive(true);
        _pauseCanva.interactable = false;
        _pauseCanva.blocksRaycasts = false;
        _pauseCanva.DOFade(0, .2f).SetEase(Ease.OutFlash);
    }

    public void Explode()
    {
        canNewGame = false;
        isGameRunning = false;
        blade.enabled = false;
        spawner.enabled = false;
        isLose = true;
        StartCoroutine(ExplodeSequence());
    }

    private void NeedNewGameRewarded()
    {
        levelSkipped = true;
        NeedNewGame(true);
    }

    private void NeedNewGameNonRewared() => NeedNewGame(false);

    private void NeedNewGame(bool canSkipAd)
    {
        isLose = false;
        needNewGame = true;
        if (canSkipAd)
        {
            AdManager.Instance.ShowRewardedAd();
            canSkip = true;
            SkipAdAfterRewared = true;
            return;
        }
        
        if (PlayerPrefs.GetString("LastDate") == DateTime.Today.ToString())
        {
            AdManager.Instance.ShowAd();
        }
    }
    private IEnumerator ExplodeSequence()
    {
        yield return new WaitForSeconds(0.5f);
        canNewGame = false;
        _scoreGameover.text = time.ToString();  
        _gameOverMenu.DOFade(1, 0.2f).SetEase(Ease.InFlash);
        _gameOverMenu.interactable = true;
        _gameOverMenu.blocksRaycasts = true;
        FindAnyObjectByType<Life>().Restart();
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

#if UNITY_EDITOR

    [Header("SKins")]
    [SerializeField] private int skinID;
    [Button]
    public void HeadSkinToUnloclk() => PlayerPrefs.SetInt($"Skin_{skinID}", 1);

    [Button]
    public void ResetSkins()
    {
        for (int i = 0; i < 100; i++)
        {
            PlayerPrefs.SetInt($"Skin_{i}", 0);
        }
    }

    [Button]
    public void ResetDay()
    {
        PlayerPrefs.SetInt("AdSes",0);
        PlayerPrefs.SetString("LastDate", String.Empty);
    }

#endif
}
