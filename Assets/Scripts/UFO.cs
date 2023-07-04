using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class UFO : MonoBehaviour
{
    [HideInInspector] public Vector2 positionToMove;

    [SerializeField] UFOBullet bulletPrefab;
    [SerializeField] float shootRate = 1f;
    [SerializeField] float shootTimerMin = 2f;
    [SerializeField] float shootTimerMax = 5f;
    Transform player;
    IObjectPool<UFOBullet> bulletPool;
    float shootTimer;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        bulletPool = new ObjectPool<UFOBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 2);
    }
    void Update()
    {
        Shoot();
    }

    void OnBecameVisible()
    {
        StartCoroutine(LerpPosition(positionToMove, 10));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            FindObjectOfType<GameManager>().UFODestroyed(gameObject);
            gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }

    void OnDestroyBullet(UFOBullet obj)
    {
        Destroy(obj.gameObject);  // Уничтожения объекта при удалении из пула
    }

    void OnReleaseBullet(UFOBullet obj)
    {
        obj.gameObject.SetActive(false);  // Деактивации объекта при возвращении в пул
    }

    void OnGetBullet(UFOBullet obj)
    {
        obj.gameObject.SetActive(true);               // Aктивации объекта при получении из пула
        obj.transform.position = transform.position;  // Установка позиции объекта равной позиции игрока
        obj.transform.rotation = transform.rotation;  // Установка поворота объекта равным повороту игрока                                                                            
        RotateBulletTowardsPlayer(obj);               // Поворачиваем снаряд в сторону игрока после создания
    }

    UFOBullet CreateBullet()
    {
        UFOBullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.SetPool(bulletPool);  // Установка ссылки на Pool для данной bullet.
        return bullet;
    }

    void Shoot()
    {
        if (shootTimer >= 0)
            shootTimer -= Time.deltaTime * shootRate;  // Уменьшение таймера стрельбы с учетом времени кадра и частоты стрельбы

        if (shootTimer <= 0)
        {
            shootTimer = Random.Range(shootTimerMin, shootTimerMax);  // Сброс таймера стрельбы на 1 секунду
            bulletPool?.Get();
        }
    }

    void RotateBulletTowardsPlayer(UFOBullet bullet)
    {
        Vector3 direction = player.position - transform.position;  // Получаем вектор направления от НЛО к игроку
        Quaternion rotation = Quaternion.FromToRotation(transform.up, direction); // Создаем кватернион из вектора направления НЛО к вектору направления игрока
        bullet.transform.rotation = rotation;  // Присваиваем этот кватернион повороту снаряда
    }

    // Корутина для плавного перемещения объекта к целевой позиции за заданное время
    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position; // Запоминаем начальную позицию объекта
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);  // Перемещаем объект по линейной интерполяции от начальной до целевой позиции
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        gameObject.SetActive(false);  
        FindObjectOfType<GameManager>().UFODestroyed(gameObject);
    }
}