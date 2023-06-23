using UnityEngine;
using UnityEngine.Pool;

public class UFOBullet : MonoBehaviour
{
    private float speed = 20.0f;
    private float lifeTime = 2.0f;

    private IObjectPool<UFOBullet> pool;                               // Пул объектов для снарядов

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);   // Двигаем объект вперед с заданной скоростью и учетом времени кадра
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DestroySelf();
    }

    internal void DestroySelf()
    {
        pool?.Release(this);                                        // Возвращаем объект в пул
    }

    private void OnEnable()
    {
        Invoke("DestroySelf", lifeTime);                            // Вызываем метод уничтожения объекта через заданное время жизни
    }

    public void SetPool(IObjectPool<UFOBullet> bulletPool)
    {
        pool = bulletPool;                                          // Устанавливаем пул объектов для снаряда
    }
}
