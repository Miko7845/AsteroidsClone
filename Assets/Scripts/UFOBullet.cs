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
        pool?.Release(this);  // ���������� ������ � ���
    }

    void OnEnable()
    {
        Invoke("DestroySelf", lifeTime);  // �������� ����� ����������� ������� ����� �������� ����� �����
    }

    public void SetPool(IObjectPool<UFOBullet> bulletPool)
    {
        pool = bulletPool;  // ������������� ��� �������� ��� �������
    }
}