using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    IObjectPool<Bullet> pool;                               // ��� �������� ��� ��������
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
        Invoke("DestroySelf", lifeTime); // �������� ����� ����������� ������� ����� �������� ����� �����
    }

    internal void DestroySelf()
    {
        pool?.Release(this);    // ���������� ������ � ���
    }

    public void SetPool(IObjectPool<Bullet> bulletPool)
    {
        pool = bulletPool;  // ������������� ��� �������� ��� �������                                     
    }
}