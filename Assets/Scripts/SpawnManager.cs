using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float trajectoryVariance = 25.0f;                                  // ��� �������� ��������� ���������� ���������� ��������� � ��������
    [SerializeField] private float spawnDistance = 15.0f;                                       // ���������� �� ������ ������, �� ������� ���������� ���������
    [SerializeField] private float spawnRate = 2f;                                              // ����� ��� ������
    [SerializeField] private int spawnWaveAmount = 2;

    private int asteroidsCount;
    private bool spawnOn = true;                                                                // ���������� ��� �����.                                           

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

            Instantiate(asteroidPrefab, spawnPoint, rotation);                                  // ������� ��������� ��������� �� ������� � ����� ��������� � ���������

         //   asteroid.trajectory = rotation * -spawnDirection;
         //   asteroid.SetTrajectory(rotation * -spawnDirection);                               // ������������� ���������� ��������� ��� ��������������� ����������� ��������� � ������ ��������
        }

        spawnOn = true;
        spawnWaveAmount++;
    }
}