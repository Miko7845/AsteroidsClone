using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float trajectoryVariance = 25.0f;                                  // Для хранения диапазона отклонения траектории астероида в градусах
    [SerializeField] private float spawnDistance = 15.0f;                                       // Расстояния от центра экрана, на котором появляются астероиды
    [SerializeField] private float spawnRate = 2f;                                              // Время для спауна
    [SerializeField] private int spawnWaveAmount = 2;

    private int asteroidsCount;
    private bool spawnOn = true;                                                                // Разрешение для спаун. Чтобы Invoke сработал лишь один раз в Update                                           

    private void Update()
    {
        asteroidsCount = FindObjectsOfType<Asteroid>().Length;

        if (asteroidsCount <= 0 & spawnOn)
        {
            spawnOn = false;
            Invoke(nameof(SpawnWave), spawnRate);
        }
    }

    // Cоздания астероидов
    private void SpawnWave()
    {
        for (int i = 0; i < spawnWaveAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;        // Выбираем случайное направление внутри круга с радиусом spawnDistance
            Vector3 spawnPoint = transform.position + spawnDirection;                           // Вычисляем точку появления астероида на этом направлении от центра экрана

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);             // Выбираем случайное отклонение траектории астероида в пределах trajectoryVariance
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);              // Создаем кватернион для поворота на этот угол вокруг оси Z

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);              // Создаем экземпляр астероида из префаба в точке появления с поворотом
            asteroid.speed = asteroid.GetRandomSpeed(asteroid.minSpeed, asteroid.maxSpeed);     // Устанавливаем рандомный скорость для астероидов
        }

        spawnOn = true;
        spawnWaveAmount++;
    }
}