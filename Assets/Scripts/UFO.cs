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
        Destroy(obj.gameObject);  // ����������� ������� ��� �������� �� ����
    }

    void OnReleaseBullet(UFOBullet obj)
    {
        obj.gameObject.SetActive(false);  // ����������� ������� ��� ����������� � ���
    }

    void OnGetBullet(UFOBullet obj)
    {
        obj.gameObject.SetActive(true);               // A�������� ������� ��� ��������� �� ����
        obj.transform.position = transform.position;  // ��������� ������� ������� ������ ������� ������
        obj.transform.rotation = transform.rotation;  // ��������� �������� ������� ������ �������� ������                                                                            
        RotateBulletTowardsPlayer(obj);               // ������������ ������ � ������� ������ ����� ��������
    }

    UFOBullet CreateBullet()
    {
        UFOBullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.SetPool(bulletPool);  // ��������� ������ �� Pool ��� ������ bullet.
        return bullet;
    }

    void Shoot()
    {
        if (shootTimer >= 0)
            shootTimer -= Time.deltaTime * shootRate;  // ���������� ������� �������� � ������ ������� ����� � ������� ��������

        if (shootTimer <= 0)
        {
            shootTimer = Random.Range(shootTimerMin, shootTimerMax);  // ����� ������� �������� �� 1 �������
            bulletPool?.Get();
        }
    }

    void RotateBulletTowardsPlayer(UFOBullet bullet)
    {
        Vector3 direction = player.position - transform.position;  // �������� ������ ����������� �� ��� � ������
        Quaternion rotation = Quaternion.FromToRotation(transform.up, direction); // ������� ���������� �� ������� ����������� ��� � ������� ����������� ������
        bullet.transform.rotation = rotation;  // ����������� ���� ���������� �������� �������
    }

    // �������� ��� �������� ����������� ������� � ������� ������� �� �������� �����
    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position; // ���������� ��������� ������� �������
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);  // ���������� ������ �� �������� ������������ �� ��������� �� ������� �������
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        gameObject.SetActive(false);  
        FindObjectOfType<GameManager>().UFODestroyed(gameObject);
    }
}