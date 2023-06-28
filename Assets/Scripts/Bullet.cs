using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private IObjectPool<Bullet> pool;                               // ��� �������� ��� ��������
    private float speed = 20.0f;
    private float lifeTime = 2.0f;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);   // ������� ������ ������ � �������� ��������� � ������ ������� �����
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DestroySelf();
    }

    internal void DestroySelf()
    {
        pool?.Release(this);                                        // ���������� ������ � ���
    }

    private void OnEnable()
    {
        Invoke("DestroySelf", lifeTime);                            // �������� ����� ����������� ������� ����� �������� ����� �����
    }

    public void SetPool(IObjectPool<Bullet> bulletPool)
    {
        pool = bulletPool;                                          // ������������� ��� �������� ��� �������
    }
}
