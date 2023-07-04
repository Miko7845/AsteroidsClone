using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    IObjectPool<Bullet> pool;                               // Пул объектов для снарядов
    float speed = 20.0f;
    float lifeTime = 2.0f;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        DestroySelf();
    }

    void OnEnable()
    {
        Invoke("DestroySelf", lifeTime); // Вызываем метод уничтожения объекта через заданное время жизни
    }

    internal void DestroySelf()
    {
        pool?.Release(this);    // Возвращаем объект в пул
    }

    public void SetPool(IObjectPool<Bullet> bulletPool)
    {
        pool = bulletPool;  // Устанавливаем пул объектов для снаряда                                     
    }
}