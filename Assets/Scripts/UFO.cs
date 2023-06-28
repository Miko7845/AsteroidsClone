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
        bulletPool = new ObjectPool<UFOBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 2);       // ������� ��� �������� ��� �������� � ��������� �����������.
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
        Destroy(obj.gameObject);                                                                             // ����������� ������� ��� �������� �� ����
    }

    private void OnReleaseBullet(UFOBullet obj)
    {
        obj.gameObject.SetActive(false);                                                                     // ����������� ������� ��� ����������� � ���
    }

    private void OnGetBullet(UFOBullet obj)
    {
        obj.gameObject.SetActive(true);                                                                      // A�������� ������� ��� ��������� �� ����
        obj.transform.position = transform.position;                                                         // ��������� ������� ������� ������ ������� ������
        obj.transform.rotation = transform.rotation;                                                         // ��������� �������� ������� ������ �������� ������
                                                                                                             
        RotateBulletTowardsPlayer(obj);                                                                      // ������������ ������ � ������� ������ ����� ��������
    }

    private UFOBullet CreateBullet()
    {
        UFOBullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);                // �������� ������ ������� bullet
        bullet.SetPool(bulletPool);                                                                          // ��������� ������ �� Pool ��� ������ bullet.
        return bullet;                                                                                       // ����������� ��������� ����
    }

    private void Shoot()
    {
        //Shooting rate
        if (shootTimer >= 0)
            shootTimer -= Time.deltaTime * shootRate;                                                        // ���������� ������� �������� � ������ ������� ����� � ������� ��������

        //Shoot
        if (shootTimer <= 0)
        {
            shootTimer = Random.Range(shootTimerMin, shootTimerMax);                                         // ����� ������� �������� �� 1 �������
            bulletPool?.Get();
        }
    }

    private void RotateBulletTowardsPlayer(UFOBullet bullet)
    {
        Vector3 direction = player.position - transform.position;                                           // �������� ������ ����������� �� ��� � ������
        Quaternion rotation = Quaternion.FromToRotation(transform.up, direction);                           // ������� ���������� �� ������� ����������� ��� � ������� ����������� ������
        bullet.transform.rotation = rotation;                                                               // ����������� ���� ���������� �������� �������
    }

    // �������� ��� �������� ����������� ������� � ������� ������� �� �������� �����
    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;                                                                                     // �������� ������� ������� 
        Vector2 startPosition = transform.position;                                                         // ���������� ��������� ������� �������
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);              // ���������� ������ �� �������� ������������ �� ��������� �� ������� �������
            time += Time.deltaTime;                                                                         // ����������� ������� ������� �� ��������� ����� �����
            yield return null;                                                                              // ���� ���������� �����
        }

        transform.position = targetPosition;                                                                // ������������� ������ � ������� �������
        gameObject.SetActive(false);                                                                        // ������������ ���
        FindObjectOfType<GameManager>().UFODestroyed(gameObject);                                           // ���������� GameManager �� ����������� ���
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