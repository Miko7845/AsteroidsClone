using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isGameActive;

    [SerializeField] GameObject menu;
    [SerializeField] Player player;
    [SerializeField] UFO ufo;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] Button resume;
    [SerializeField] TextMeshProUGUI controlsText;
    [SerializeField] float ufoSpawnRateMin = 20f;
    [SerializeField] float ufoSpawnRateMax = 40f;
    Color blinkColor;
    Color normalColor; 
    SpriteRenderer playerRenderer;
    ParticleSystem explosion;
    float blinkInterval = 0.5f;
    float blinkDuration;
    float screenHeight = 10.0f;
    float widthSides = 19.0f;
    float respawnTime = 2.0f;
    float respawnInvulnerabilityTime = 3.0f;
    int spawnSide;  // UFO spawn side
    int lives = 3;
    int score = 0;
    bool isBlinking = false;

    void Awake()
    {
        explosion = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
        playerRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        blinkColor = new Color(0f, 0f, 0f, 255f);
        normalColor = new Color(255f, 255f, 255f, 255f);
    }

    void Start()
    {
        StartCoroutine(SpawnUFO());
        StartGame();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            isGameActive = false;
            menu.SetActive(true);
            resume.gameObject.SetActive(true);
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            isGameActive = true;
            menu.SetActive(false);
        }
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();

        if (asteroid.transform.localScale.y == asteroid.maxSize)
            UpdateScore(20);
        else if (asteroid.transform.localScale.y == asteroid.midSize)
            UpdateScore(50);
        else
            UpdateScore(100);
    }

    public void PlayerDied()
    {
        explosion.transform.position = player.transform.position;
        explosion.Play();
        UpdateLives(1);

        if (lives <= 0)
            GameOver();
        else
            Invoke(nameof(Respawn), respawnTime);
    }

    public void UFODestroyed(GameObject ufo)
    {
        explosion.transform.position = ufo.transform.position;
        explosion.Play();
        UpdateScore(200);

        // Вызываем корутину SpawnUFO после уничтожения НЛО
        StartCoroutine(SpawnUFO());
    }

    void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.tag = "Invulnerability";
        player.gameObject.SetActive(true);

        if (!isBlinking)
        {
            blinkDuration = respawnInvulnerabilityTime;
            StartCoroutine(Blink());
        }
        Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
    }

    void StartGame()
    {
        isGameActive = true;
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
        lives = 3;
        score = 0;
        UpdateLives(0);
        UpdateScore(0);
    }

    void GameOver()
    {
        PauseGame();
        isGameActive = false;
        resume.gameObject.SetActive(false);
    }

    void UpdateScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score.ToString();
    }

    void UpdateLives(int value)
    {
        lives -= value;
        livesText.text = "Lives: " + lives.ToString();
    }

    void TurnOnCollisions()
    {
        player.gameObject.tag = "Player";
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PauseGame();
    }

    public void Resume()
    {
        PauseGame();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ChangeControls()
    {
        if (player.mouseControlOn)
        {
            player.mouseControlOn = false;
            controlsText.text = "Keyboard";
        }
        else
        {
            player.mouseControlOn = true;
            controlsText.text = "Mouse";
        }
    }

    IEnumerator Blink()
    {
        isBlinking = true;                          
        float startTime = Time.time;  // Запоминаем время начала мигания
                                      // 
        while (Time.time - startTime < blinkDuration)
        {
            // Меняем цвет спрайта на мигающий и Ждем интервал мигания
            playerRenderer.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);
            playerRenderer.color = normalColor;
            yield return new WaitForSeconds(blinkInterval);
        }
        isBlinking = false;
    }

    IEnumerator SpawnUFO()
    {
        // Ждем случайное количество секунд между ufoSpawnRateMin и ufoSpawnRateMax
        yield return new WaitForSeconds(Random.Range(ufoSpawnRateMin, ufoSpawnRateMax));

        // Проверяем, жив ли НЛО
        if (ufo.gameObject.activeSelf)
        {
            // Если жив, останавливаем корутину
            StopCoroutine(SpawnUFO());
        }
        else
        {
            // Если не жив, возрождаем НЛО
            ufo.gameObject.SetActive(true);
            ufo.transform.position = RandomUFOPostion();
            ufo.positionToMove = spawnSide == 0 ?
                new Vector2((Mathf.Abs(widthSides) + 0.5f), ufo.transform.position.y) :
                new Vector2((-widthSides - 0.5f), ufo.transform.position.y);
        }
    }

    Vector2 RandomUFOPostion()
    {
        spawnSide = Random.Range(0, 2);
        widthSides = spawnSide == 0 ? -widthSides : Mathf.Abs(widthSides);
        float height20 = screenHeight - (screenHeight * 0.2f);
        float randomHeight = Random.Range(-height20, height20);
        return new Vector2(widthSides, randomHeight);
    }
}