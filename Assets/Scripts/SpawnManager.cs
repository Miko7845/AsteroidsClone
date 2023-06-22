using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    // �������� ��� ����������
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float trajectoryVariance = 25.0f;                                  // ��� �������� ��������� ���������� ���������� ��������� � ��������
    [SerializeField] private float spawnDistance = 15.0f;                                       // ���������� �� ������ ������, �� ������� ���������� ���������
    [SerializeField] private float spawnRate = 2f;                                              // ����� ��� ������
    [SerializeField] private int spawnWaveAmount = 2;

    private int asteroidsCount;
    private bool spawnOn = true;                                                                // ���������� ��� �����. ����� Invoke �������� ���� ���� ��� � Update

    // �������� ��� ���
    [SerializeField] private UFO ufoPrefab;
    [SerializeField] private float ufoSpawnRateMin = 20f;
    [SerializeField] private float ufoSpawnRateMax = 40f;
    private int spawnSide;

    // ������� �������
    [SerializeField] private float screenHeight = 10.0f;
    [SerializeField] private float widthSides = 18.0f;

    private void Start()
    {
        InvokeRepeating("SpawnUFO", Random.Range(ufoSpawnRateMin, ufoSpawnRateMax), Random.Range(ufoSpawnRateMin, ufoSpawnRateMax));
    }

    private void Update()
    {
        asteroidsCount = FindObjectsOfType<Asteroid>().Length;

        if (asteroidsCount <= 0 & spawnOn)
        {
            spawnOn = false;
            Invoke(nameof(SpawnWave), spawnRate);
        }
    }

    // C������� ����������
    private void SpawnWave()
    {
        for (int i = 0; i < spawnWaveAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;        // �������� ��������� ����������� ������ ����� � �������� spawnDistance
            Vector3 spawnPoint = transform.position + spawnDirection;                           // ��������� ����� ��������� ��������� �� ���� ����������� �� ������ ������

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);             // �������� ��������� ���������� ���������� ��������� � �������� trajectoryVariance
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);              // ������� ���������� ��� �������� �� ���� ���� ������ ��� Z

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);              // ������� ��������� ��������� �� ������� � ����� ��������� � ���������
            asteroid.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);    // ������������� ��������� ���� �������� ������� ������ ��� Z
            asteroid.speed = asteroid.GetRandomSpeed(asteroid.minSpeed, asteroid.maxSpeed);     // ������������� ��������� �������� ��� ����������
        }

        spawnOn = true;
        spawnWaveAmount++;
    }

    private void SpawnUFO()
    {
        UFO ufo = Instantiate(ufoPrefab, RandomUFOPostion(), ufoPrefab.transform.rotation);
        ufo.speed = spawnSide == 0 ? Mathf.Abs(ufo.speed) : ufo.speed = -ufo.speed;
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