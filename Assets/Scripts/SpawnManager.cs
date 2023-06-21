using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float trajectoryVariance = 25.0f;                                  // ��� �������� ��������� ���������� ���������� ��������� � ��������
    [SerializeField] private float spawnDistance = 15.0f;                                       // ���������� �� ������ ������, �� ������� ���������� ���������
    [SerializeField] private float spawnRate = 2f;                                              // ����� ��� ������
    [SerializeField] private int spawnWaveAmount = 2;

    [SerializeField] private UFO ufoPrefab;
    [SerializeField] private float ufoSpawnRateMin = 20f;
    [SerializeField] private float ufoSpawnRateMax = 40f;


    private int asteroidsCount;
    private bool spawnOn = true;                                                                // ���������� ��� �����. ����� Invoke �������� ���� ���� ��� � Update

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
        float variance = Random.Range(-trajectoryVariance, trajectoryVariance);             
        Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

        UFO ufo = Instantiate(ufoPrefab, new Vector3(1,1,1), rotation);
    }
}