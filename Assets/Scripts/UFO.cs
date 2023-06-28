using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class UFO : MonoBehaviour
{
    internal Vector2 positionToMove;
    private Transform player;
    private IObjectPool<UFOBullet> bulletPool;
    [SerializeField] private UFOBullet bulletPrefab;
    [SerializeField] private float shootRate = 1f;
    [SerializeField] private float shootTimerMin = 2f;
    [SerializeField] private float shootTimerMax = 5f;
    private float shootTimer;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        bulletPool = new ObjectPool<UFOBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 2);       // Создаем пул объектов для снарядов с заданными параметрами.
    }

    private void OnBecameVisible()
    {
        StartCoroutine(LerpPosition(positionToMove, 10));
    }

    private void Update()
    {
        Shoot();
    }

    private void OnDestroyBullet(UFOBullet obj)
    {
        Destroy(obj.gameObject);                                                                             // Уничтожения объекта при удалении из пула
    }

    private void OnReleaseBullet(UFOBullet obj)
    {
        obj.gameObject.SetActive(false);                                                                     // Деактивации объекта при возвращении в пул
    }

    private void OnGetBullet(UFOBullet obj)
    {
        obj.gameObject.SetActive(true);                                                                      // Aктивации объекта при получении из пула
        obj.transform.position = transform.position;                                                         // Установка позиции объекта равной позиции игрока
        obj.transform.rotation = transform.rotation;                                                         // Установка поворота объекта равным повороту игрока
                                                                                                             
        RotateBulletTowardsPlayer(obj);                                                                      // Поворачиваем снаряд в сторону игрока после создания
    }

    private UFOBullet CreateBullet()
    {
        UFOBullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);                // Создания нового объекта bullet
        bullet.SetPool(bulletPool);                                                                          // Установка ссылки на Pool для данной bullet.
        return bullet;                                                                                       // Возвращение созданной пули
    }

    private void Shoot()
    {
        //Shooting rate
        if (shootTimer >= 0)
            shootTimer -= Time.deltaTime * shootRate;                                                        // Уменьшение таймера стрельбы с учетом времени кадра и частоты стрельбы

        //Shoot
        if (shootTimer <= 0)
        {
            shootTimer = Random.Range(shootTimerMin, shootTimerMax);                                         // Сброс таймера стрельбы на 1 секунду
            bulletPool?.Get();
        }
    }

    private void RotateBulletTowardsPlayer(UFOBullet bullet)
    {
        Vector3 direction = player.position - transform.position;                                           // Получаем вектор направления от НЛО к игроку
        Quaternion rotation = Quaternion.FromToRotation(transform.up, direction);                           // Создаем кватернион из вектора направления НЛО к вектору направления игрока
        bullet.transform.rotation = rotation;                                                               // Присваиваем этот кватернион повороту снаряда
    }

    // Корутина для плавного перемещения объекта к целевой позиции за заданное время
    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;                                                                                     // Обнуляем счетчик времени 
        Vector2 startPosition = transform.position;                                                         // Запоминаем начальную позицию объекта
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);              // Перемещаем объект по линейной интерполяции от начальной до целевой позиции
            time += Time.deltaTime;                                                                         // Увеличиваем счетчик времени на прошедшее время кадра
            yield return null;                                                                              // Ждем следующего кадра
        }

        transform.position = targetPosition;                                                                // Устанавливаем объект в целевую позицию
        gameObject.SetActive(false);                                                                        // Деактивируем НЛО
        FindObjectOfType<GameManager>().UFODestroyed(gameObject);                                           // Уведомляем GameManager об уничтожении НЛО
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            FindObjectOfType<GameManager>().UFODestroyed(this.gameObject);
            gameObject.SetActive(false);

            Destroy(other.gameObject);
        }
    }
}