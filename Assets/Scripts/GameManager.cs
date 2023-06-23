using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 2.0f;
    public float respawnInvulnerabilityTime = 3.0f;
    public int lives = 3;
    public int score = 0;

    // Свойства для НЛО
    [SerializeField] private UFO ufo;
    [SerializeField] private float ufoSpawnRateMin = 20f;
    [SerializeField] private float ufoSpawnRateMax = 40f;
    private int spawnSide;

    // Игровая область
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
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        player.gameObject.SetActive(true);

        Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        lives = 3;
        score = 0;

        Invoke(nameof(Respawn), respawnTime);
    }

    public void UFODestroyed(GameObject ufo)
    {
        explosion.transform.position = ufo.transform.position;
        explosion.Play();

        // Вызываем корутину SpawnUFO после уничтожения НЛО
        StartCoroutine(SpawnUFO());
    }

    private IEnumerator SpawnUFO()
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

    private Vector2 RandomUFOPostion()
    {
        spawnSide = Random.Range(0, 2);
        widthSides = spawnSide == 0 ? -widthSides : Mathf.Abs(widthSides);

        float height20 = screenHeight - (screenHeight * 0.2f);
        float randomHeight = Random.Range(-height20, height20);

        return new Vector2(widthSides, randomHeight);
    }
}