using UnityEngine;
using UnityEngine.Pool;

public class UFOBullet : MonoBehaviour
{
    IObjectPool<UFOBullet> pool;
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

    internal void DestroySelf()
    {
        pool?.Release(this);  // Возвращаем объект в пул
    }

    void OnEnable()
    {
        Invoke("DestroySelf", lifeTime);  // Вызываем метод уничтожения объекта через заданное время жизни
    }

    public void SetPool(IObjectPool<UFOBullet> bulletPool)
    {
        pool = bulletPool;  // Устанавливаем пул объектов для снаряда
    }
}