using System;
using System.Collections;
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
    [SerializeField] private Image fadeImage;
    [SerializeField] public float time = 0f;
    [SerializeField] private float maxTime = 30f;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI TextLevel;
    public bool isGameRunning = false;
    int level = 0;

    public int score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        scoreText.text = time.ToString() + " / " + maxTime.ToString();
        if (time >= maxTime)
        {
            time = maxTime;
            NewGame();
        }
        slider.value = time / maxTime;
        
    }

    private void NewGame()
    {
        time = 0f;
        FindAnyObjectByType<Life>().Restart();
        isGameRunning = false;
        level++;
        TextLevel.text = "Level " + level.ToString();
        Time.timeScale = 1f;

        ClearScene();
        isGameRunning = true;

        if (level == 1)
            maxTime = 10;
        else if(level == 2)
            maxTime = 15;
        
        if(level % 5 == 0)
        {
            spawner.bombChance += 0.01f;
            spawner.minSpawnDelay -= 0.05f;
            spawner.maxSpawnDelay -= 0.05f;
            maxTime += 5f;
            if(spawner.bombChance > 0.25f)
                spawner.bombChance = 0.25f;
        }

      
        blade.enabled = true;
        spawner.enabled = true;

        
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

    public void Explode()
    {
        isGameRunning = false;
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;
        fadeImage.enabled = true;
        // Fade to white
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        fadeImage.enabled = false;
        yield return new WaitForSecondsRealtime(1f);

        NewGame();

        elapsed = 0f;

        // Fade back in
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }

}
