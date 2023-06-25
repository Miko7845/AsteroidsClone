using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float respawnTime = 2.0f;
    [SerializeField] private float respawnInvulnerabilityTime = 3.0f;
    [SerializeField] private int lives = 3;
    [SerializeField] private int score = 0;

    [SerializeField] SpriteRenderer spriteRenderer;
    private Color blinkColor = new Color(0f, 0f, 0f, 255f);                            // ���� ������� ��� �������
    private Color normalColor = new Color(255f, 255f, 255f, 255f);                     // ���� ������� ��� ���������� ���������
    [SerializeField] private float blinkInterval = 0.5f;                               // �������� ������� � ��������
    private float blinkDuration;                                                       // ������������ ������� � ��������
    private bool isBlinking = false;                                                   // ����, ����������� �� ��, ��� �������� ������

    // �������� ��� ���
    [SerializeField] private UFO ufo;
    [SerializeField] private float ufoSpawnRateMin = 20f;
    [SerializeField] private float ufoSpawnRateMax = 40f;
    private int spawnSide;

    // ������� �������
    private float screenHeight = 10.0f;
    private float widthSides = 19.0f;

    private void Start()
    {
        StartCoroutine(SpawnUFO());
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();

        if (asteroid.transform.localScale.y == asteroid.maxSize)
            score = 20;
        else if (asteroid.transform.localScale.y == asteroid.midSize)
            score = 50;
        else
            score += 100;
    }

    public void PlayerDied()
    {
        explosion.transform.position = player.transform.position;
        explosion.Play();

        lives--;

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

    private void GameOver()
    {
        lives = 3;
        score = 0;

        Invoke(nameof(Respawn), respawnTime);
    }

    IEnumerator Blink()
    {
        isBlinking = true;                          
        float startTime = Time.time;                                                            // ���������� ����� ������ �������

        // ���� �� ������ ������ ����� �������
        while (Time.time - startTime < blinkDuration)
        {
            spriteRenderer.color = blinkColor;                                                  // ������ ���� ������� �� ��������
            yield return new WaitForSeconds(blinkInterval);                                     // ���� �������� �������
            spriteRenderer.color = normalColor;                                                 // ������ ���� ������� �� ����������
            yield return new WaitForSeconds(blinkInterval);                                     // ���� �������� �������
        }

        isBlinking = false;
    }

    public void UFODestroyed(GameObject ufo)
    {
        explosion.transform.position = ufo.transform.position;
        explosion.Play();
        score += 200;

        // �������� �������� SpawnUFO ����� ����������� ���
        StartCoroutine(SpawnUFO());
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