using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] UFO ufo;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    private Color blinkColor;                                                          // ���� ������� ��� �������
    private Color normalColor;                                                         // ���� ������� ��� ���������� ���������   
    private SpriteRenderer playerRenderer;
    private ParticleSystem explosion;
    [SerializeField] float ufoSpawnRateMin = 20f;
    [SerializeField] float ufoSpawnRateMax = 40f;   
    private float blinkInterval = 0.5f;                                                // �������� ������� � ��������
    private float blinkDuration;                                                       // ������������ ������� � ��������
    private float screenHeight = 10.0f;
    private float widthSides = 19.0f;
    private float respawnTime = 2.0f;
    private float respawnInvulnerabilityTime = 3.0f;
    private int spawnSide;
    private int lives = 3;
    private int score = 0;
    private bool isBlinking = false;                                                   // ����, ����������� �� ��, ��� �������� ������
    private bool isGameActive;

    private void Awake()
    {
        explosion = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
        playerRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();

        blinkColor = new Color(0f, 0f, 0f, 255f);
        normalColor = new Color(255f, 255f, 255f, 255f);
    }

    private void Start()
    {
        StartCoroutine(SpawnUFO());
        StartGame();
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

    private void Respawn()
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

    private void TurnOnCollisions()
    {
        player.gameObject.tag = "Player";
    }

    private void StartGame()
    {
        isGameActive = true;
        lives = 3;
        score = 0;

        UpdateLives(0);
        UpdateScore(0);
    }

    private void GameOver()
    {
        isGameActive = false;

        StartGame();
        Invoke(nameof(Respawn), respawnTime);
    }

    public void UFODestroyed(GameObject ufo)
    {
        explosion.transform.position = ufo.transform.position;
        explosion.Play();
        UpdateScore(200);

        // �������� �������� SpawnUFO ����� ����������� ���
        StartCoroutine(SpawnUFO());
    }

    private void UpdateScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score.ToString();
    }

    private void UpdateLives(int value)
    {
        lives -= value;
        livesText.text = "Lives: " + lives.ToString();
    }

    IEnumerator Blink()
    {
        isBlinking = true;                          
        float startTime = Time.time;                                                            // ���������� ����� ������ �������

        // ���� �� ������ ������ ����� �������
        while (Time.time - startTime < blinkDuration)
        {
            playerRenderer.color = blinkColor;                                                  // ������ ���� ������� �� ��������
            yield return new WaitForSeconds(blinkInterval);                                     // ���� �������� �������
            playerRenderer.color = normalColor;                                                 // ������ ���� ������� �� ����������
            yield return new WaitForSeconds(blinkInterval);                                     // ���� �������� �������
        }

        isBlinking = false;
    }

    private IEnumerator SpawnUFO()
    {
        yield return new WaitForSeconds(Random.Range(ufoSpawnRateMin, ufoSpawnRateMax));        // ���� ��������� ���������� ������ ����� ufoSpawnRateMin � ufoSpawnRateMax

        // ���������, ��� �� ���
        if (ufo.gameObject.activeSelf)
        {
            StopCoroutine(SpawnUFO());                                                          // ���� ���, ������������� ��������
        }
        else
        {
            // ���� �� ���, ���������� ���
            ufo.gameObject.SetActive(true);
            ufo.transform.position = RandomUFOPostion();
            ufo.positionToMove = spawnSide == 0 ?
                new Vector2((Mathf.Abs(widthSides) + 0.5f), ufo.transform.position.y) :
                new Vector2((-widthSides - 0.5f), ufo.transform.position.y);
        }
    }

    private Vector2 RandomUFOPostion()
    {
        spawnSide = Random.Range(0, 2);
        widthSides = spawnSide == 0 ? -widthSides : Mathf.Abs(widthSides);

        float height20 = screenHeight - (screenHeight * 0.2f);
        float randomHeight = Random.Range(-height20, height20);

        return new Vector2(widthSides, randomHeight);
    }
}