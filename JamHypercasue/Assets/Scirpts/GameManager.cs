using System;
using System.Collections;
using IIMEngine.SFX;
using DG.Tweening;
using TMPro;
using UnityEngine;
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
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI TextLevel;
    public bool isGameRunning = false;
    private bool canNewGame = true;
    private bool needNewGame = false;
    private bool isLose = false;
    [SerializeField] private GameObject[] winGameObjects;
    int level = 0;

    [Header("Pause")] 
    [SerializeField] private CanvasGroup _pauseCanva;
    [SerializeField] private Button[] _pauseButtons;
    [SerializeField] private Button[] _unpauseButtons;
    [SerializeField] private GameObject[] _objectsToPause;

    public int score { get; private set; } = 0;

    private void Awake()
    {
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

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
        
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
        print(PlayerData.GetPlayerCurrentLevel());
         SFXsManager.Instance.PlaySound("Ambiance");
        if (PlayerData.GetPlayerCurrentLevel() != 1)
        {
            for(int i = 0; i < PlayerData.GetPlayerCurrentLevel() - 1; i++)
            {
                if(PlayerData.GetPlayerCurrentLevel() % 5 == 0)
                {
                    spawner.bombChance += 0.01f;
                    spawner.luckyChance -= 0.01f;
                    spawner.comboChance += 0.01f;
                    spawner.minSpawnDelay -= 0.005f;
                    spawner.maxSpawnDelay -= 0.005f;
                    maxTime += 2f;
                    if(spawner.bombChance > 0.25f)
                        spawner.bombChance = 0.25f;
                    if (spawner.luckyChance < 0)
                        spawner.luckyChance = 0;
                    if (spawner.comboChance > 0.25f)
                        spawner.comboChance = 0.25f;
                        
                }
            }
        }
    }

    private void Update()
    {
        scoreText.text = time.ToString() + " / " + maxTime.ToString();
        if (time >= maxTime && !isLose)
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
        if(canNewGame)
        {
            if (Input.GetMouseButtonDown(0))
            {
                needNewGame = true;
                canNewGame = false;
                foreach (var go in winGameObjects)
                {
                    go.SetActive(false);
                }
                
            }
        }
        slider.value = time / maxTime;
        if(needNewGame)
        {

            NewGame();
        }
        
    }

    private void resetGame()
    {
        time = 0f;
        FindAnyObjectByType<Life>().Restart();
        isGameRunning = false;
        level++;
        
        //TextLevel.text = "Level " + PlayerData.GetPlayerCurrentLevel().ToString();
        
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
        needNewGame = false;
        resetGame();
        Time.timeScale = 1f;
        isGameRunning = true;
        blade.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        blade.enabled = true;
        spawner.enabled = true;
        isLose = false;
        

        
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
    }
    
    private void UnPause()
    {
        foreach (GameObject el in _objectsToPause)
        {
            el.SetActive(true);
        }
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

    private IEnumerator ExplodeSequence()
    {
        yield return new WaitForSeconds(0.5f);
        canNewGame = true;
        
    }

}
