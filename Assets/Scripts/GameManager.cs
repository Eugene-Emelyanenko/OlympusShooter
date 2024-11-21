using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Background")]
    [SerializeField] private Image bgImage;

    [Header("Movement")]
    [SerializeField] private float xMinLimit = -5f;
    [SerializeField] private float xMaxLimit = 5f;
    public float moveSpeed = 3f;

    [Header("Timer")]
    public int gameTimer = 60;
    public int superGameTimer = 90;
    [SerializeField] private Slider timerSlider;

    [Header("Lighting")]
    [SerializeField] private Transform lightingSpawnPoint;
    public float lightingSpeed = 2f;
    [SerializeField] private GameObject lightingPrefab;
    public float shotInterval = 1f;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winFrame;
    [SerializeField] private TextMeshProUGUI collectedCoinsText;
    [SerializeField] private GameObject loseFrame;

    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;

    private int selectedLevel = 1;
    private int selectedBg = 1;
    private int selectedCharacter = 1;

    private int xInput = 0;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private int timerTime;
    private float currentTime;
    private float lastShotTime = 0f;
    private bool isGameOver = false;
    public int collectedCoins { get; set; }

    private void Awake()
    {
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        selectedBg = PlayerPrefs.GetInt("Background", 1);
        selectedCharacter = PlayerPrefs.GetInt("Character", 1);

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if(selectedLevel == 1)
            timerTime = gameTimer;
        else
            timerTime = superGameTimer;
    }

    private void Start()
    {
        UnPause();
        gameOverPanel.SetActive(false);
        collectedCoins = 0;

        bgImage.sprite = Resources.Load<Sprite>($"Background/{selectedBg}");
        spriteRenderer.sprite = Resources.Load<Sprite>($"Character/{selectedCharacter}");
        gameOverPanel.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Background/{selectedBg}");
        pausePanel.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Background/{selectedBg}");

        currentTime = timerTime;
        timerSlider.maxValue = timerTime;
        timerSlider.value = currentTime;
    }
    private void Update()
    {
        if(!isGameOver)
        {
            MoveCharacter();
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            // Уменьшаем текущее время
            currentTime -= Time.deltaTime;
            // Обновляем слайдер
            timerSlider.value = currentTime;

            // Проверяем, если время истекло
            if (currentTime <= 0)
            {
                GameOver(true); // Время истекло, игрок победил
            }
        }
    }

    private void MoveCharacter()
    {
        float newXPosition = transform.position.x + xInput * moveSpeed * Time.deltaTime;

        newXPosition = Mathf.Clamp(newXPosition, xMinLimit, xMaxLimit);

        transform.position = new Vector2(newXPosition, transform.position.y);
    }

    public void Move(bool isLeft)
    {
        xInput = isLeft ? -1 : 1;
    }

    public void Shot()
    {
        if (Time.time >= lastShotTime + shotInterval && !isGameOver)
        {
            GameObject newLighting = Instantiate(lightingPrefab, lightingSpawnPoint.position, Quaternion.identity);
            Lighting lighting = newLighting.GetComponent<Lighting>();
            if (lighting != null)
            {
                lighting.SetSpeed(lightingSpeed);
            }

            SoundManager.Instance.PlayClip("Shot");

            lastShotTime = Time.time;
        }
        else
        {
            Debug.Log("Cannot shoot yet, waiting for cooldown.");
        }
    }

    public void Stop()
    {
        xInput = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            GameOver(false);
        }
    }

    private void GameOver(bool isWin)
    {
        if (isGameOver)
            return;

        enemySpawner.StopSpawn();
        isGameOver = true;
        gameOverPanel.SetActive(true);

        if(isWin)
        {
            SoundManager.Instance.PlayClip("Win");

            winFrame.SetActive(true);
            loseFrame.SetActive(false);

            collectedCoinsText.text = collectedCoins.ToString();
        }
        else
        {
            SoundManager.Instance.PlayClip("Lose");

            winFrame.SetActive(false);
            loseFrame.SetActive(true);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);       
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
