using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;                                      
    [SerializeField] float spawnRate = 2f;    // ����� ��� ������
    [SerializeField] int spawnWaveAmount = 2;
    float spawnDistance = 15.0f;              // ���������� �� ������ ������, �� ������� ���������� ���������
    float trajectoryVariance = 25.0f;         // ��� �������� ��������� ���������� ���������� ��������� � ��������
    int asteroidsCount;
    bool spawnOn = true;                      // ���������� ��� �����. ����� Invoke �������� ���� ���� ��� � Update

    void Update()
    {
        asteroidsCount = FindObjectsOfType<Asteroid>().Length;
        if (asteroidsCount <= 0 & spawnOn)
        {
            spawnOn = false;
            Invoke(nameof(SpawnWave), spawnRate);
        }
    }

    void SpawnWave()
    {
        for (int i = 0; i < spawnWaveAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;     // �������� ��������� ����������� ������ ����� � �������� spawnDistance
            Vector3 spawnPoint = transform.position + spawnDirection;                        // ��������� ����� ��������� ��������� �� ���� ����������� �� ������ ������
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);          // �������� ��������� ���������� ���������� ��������� � �������� trajectoryVariance
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);           // ������� ���������� ��� �������� �� ���� ���� ������ ��� Z
            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);           // ������� ��������� ��������� �� ������� � ����� ��������� � ���������
            asteroid.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f); // ������������� ��������� ���� �������� ������� ������ ��� Z
            asteroid.speed = asteroid.GetRandomSpeed(asteroid.minSpeed, asteroid.maxSpeed);  // ������������� ��������� �������� ��� ����������
        }
        spawnOn = true;
        spawnWaveAmount++;
    }
}