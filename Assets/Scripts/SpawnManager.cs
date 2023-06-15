using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 25.0f;                                                    // Для хранения диапазона отклонения траектории астероида в градусах
    public float spawnDistance = 15.0f;                                                         // Расстояния от центра экрана, на котором появляются астероиды
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

    // Cоздания астероидов
    private void SpawnWave(int spawnWaveAmount)
    {
        for (int i = 0; i < spawnWaveAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;        // Выбираем случайное направление внутри круга с радиусом spawnDistance
            Vector3 spawnPoint = transform.position + spawnDirection;                           // Вычисляем точку появления астероида на этом направлении от центра экрана

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);             // Выбираем случайное отклонение траектории астероида в пределах trajectoryVariance
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);              // Создаем кватернион для поворота на этот угол вокруг оси Z

            Instantiate(asteroidPrefab, spawnPoint, rotation);                                  // Создаем экземпляр астероида из префаба в точке появления с поворотом
         //   asteroid.SetTrajectory(rotation * -spawnDirection);                                 // Устанавливаем траекторию астероида как противоположную направлению появления с учетом поворота
        }
    }
}