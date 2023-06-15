using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 25.0f;                                                    // ��� �������� ��������� ���������� ���������� ��������� � ��������
    public float spawnDistance = 15.0f;                                                         // ���������� �� ������ ������, �� ������� ���������� ���������
    public int spawnWaveAmount = 2;
    int asteroidsCount;

    private void Update()
    {
        asteroidsCount = FindObjectsOfType<Asteroid>().Length;

        if (asteroidsCount <= 0)
        {
            SpawnWave(spawnWaveAmount);
        }
    }

    // C������� ����������
    private void SpawnWave(int spawnWaveAmount)
    {
        for (int i = 0; i < spawnWaveAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;        // �������� ��������� ����������� ������ ����� � �������� spawnDistance
            Vector3 spawnPoint = transform.position + spawnDirection;                           // ��������� ����� ��������� ��������� �� ���� ����������� �� ������ ������

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);             // �������� ��������� ���������� ���������� ��������� � �������� trajectoryVariance
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);              // ������� ���������� ��� �������� �� ���� ���� ������ ��� Z

            Instantiate(asteroidPrefab, spawnPoint, rotation);                                  // ������� ��������� ��������� �� ������� � ����� ��������� � ���������
         //   asteroid.SetTrajectory(rotation * -spawnDirection);                                 // ������������� ���������� ��������� ��� ��������������� ����������� ��������� � ������ ��������
        }
    }
}