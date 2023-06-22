using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    // Свойства для Астероидов
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float trajectoryVariance = 25.0f;                                  // Для хранения диапазона отклонения траектории астероида в градусах
    [SerializeField] private float spawnDistance = 15.0f;                                       // Расстояния от центра экрана, на котором появляются астероиды
    [SerializeField] private float spawnRate = 2f;                                              // Время для спауна
    [SerializeField] private int spawnWaveAmount = 2;

    private int asteroidsCount;
    private bool spawnOn = true;                                                                // Разрешение для спаун. Чтобы Invoke сработал лишь один раз в Update

    // Свойства для НЛО
    [SerializeField] private UFO ufoPrefab;
    [SerializeField] private float ufoSpawnRateMin = 20f;
    [SerializeField] private float ufoSpawnRateMax = 40f;
    private int spawnSide;

    // Игровая область
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
            asteroid.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);    // Устанавливаем случайный угол поворота объекта вокруг оси Z
            asteroid.speed = asteroid.GetRandomSpeed(asteroid.minSpeed, asteroid.maxSpeed);     // Устанавливаем рандомный скорость для астероидов
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