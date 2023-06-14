using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private float verticalInputAcceleration = 10.0f;                                                       // ��������� �� ��������� ��� ������� ������.
    [SerializeField] private float horizontalInputAcceleration = 150.0f;                                                    // ��������� �� ����������� ��� ������� ������.

    [SerializeField] private float maxSpeed = 10.0f;                                                                        // ������������ �������� ��������.
    [SerializeField] private float maxRotationSpeed = 100.0f;                                                               // ������������ �������� ��������.

    [SerializeField] private float velocityDrag = 1.0f;                                                                     // ����������� ���������� ��������.
    [SerializeField] private float rotationDrag = 1.0f;                                                                     // ����������� ���������� ��������.

    private Vector3 velocity;                                                                                               // ������ �������� ��������.
    private float zRotationVelocity;                                                                                        // �������� �������� �� ��� Z.

    public bool mouseControlOn = false;                                                                                     // ���� ��������� ���������� �����.

    [SerializeField] private float shootRate = 3f;                                                                          // �������� �������� (���������� ��������� � �������)
    private float shootTimer;                                                                                               // ������ ��� �������� �������� ��������

    private IObjectPool<Bullet> bulletPool;                                                                                 // ��� �������� ��� ��������

    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 5);       // ������� ��� �������� ��� �������� � ��������� �����������.
    }

    private void Update()
    {
        // ���� �� ���������.
        Vector3 acceleration = Input.GetAxis("Vertical") * verticalInputAcceleration * transform.up;                        // ��������� ������ ��������� �� ��������� � ����������� �� ������� ������� � ����������� ������.
        velocity += acceleration * Time.deltaTime;                                                                          // ���������� ��������� � �������� �������� � ������ ������� �����.
                                                                                                                            
        // ���� �� �����������                                                                                              
        float zTurnAcceleration = -1 * Input.GetAxis("Horizontal") * horizontalInputAcceleration;                           // ��������� ��������� �������� �� ��� Z � ����������� �� ������� ������� � ����� ������ (��� �������� �����������).
        zRotationVelocity += zTurnAcceleration * Time.deltaTime;                                                            // ���������� ��������� �������� � �������� �������� � ������ ������� �����

        Shoot();
    }

    private void FixedUpdate()
    {
        Movement();

        if (mouseControlOn)
            MouseRotate();
        else
            KeyRotate();
    }

    private void Movement()
    {
        velocity = velocity * (1 - Time.deltaTime * velocityDrag);                                                          // ���������� ��������. �������� �������� �� ����������� ���������� � ������ ������� �����
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);                                                              // �������� ������ ��������
        transform.position += velocity * Time.deltaTime;                                                                    // (�����������) ���������� � ������� ������ ������ �������� � ������ ������� �����
    }

    private void KeyRotate()
    {
        zRotationVelocity = zRotationVelocity * (1 - Time.deltaTime * rotationDrag);                                        // �������� �������� �������� �� ����������� ���������� � ������ ������� �����.
        zRotationVelocity = Mathf.Clamp(zRotationVelocity, -maxRotationSpeed, maxRotationSpeed);                            // �������� �������� �������� �� ������ �� ������������ �������� ��������.
        transform.Rotate(0, 0, zRotationVelocity * Time.deltaTime);                                                         // (�������) ������������ ������ �� ��� Z �� ����, ������ �������� �������� � ������ ������� �����.
    }

    private void MouseRotate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                        // �������� ������� ���� � ������� ����������� �� �������� ���������
        Vector2 direction = mousePosition - transform.position;                                                             // ��������� ������ ����������� �� ������ � ����.
        float angle = Vector2.SignedAngle(Vector2.up, direction);                                                           // ��������� ���� ����� ������������ ������������ � �������� ����������� � ������ �����.
        Vector3 targetRotation = new Vector3(0, 0, angle);                                                                  // ������� ������ �������� �������� �� ����
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), maxRotationSpeed * Time.deltaTime); // ������������ ������ � �������� �������� � �������� ������������ ��������� �������� � ������ ������� �����
    }

    private void OnDestroyBullet(Bullet obj)
    {
        Destroy(obj.gameObject);                                                                                            // ����������� ������� ��� �������� �� ����
    }

    private void OnReleaseBullet(Bullet obj)
    {
        obj.gameObject.SetActive(false);                                                                                    // ����������� ������� ��� ����������� � ���
    }

    private void OnGetBullet(Bullet obj)
    {
        obj.gameObject.SetActive(true);                                                                                     // A�������� ������� ��� ��������� �� ����
        obj.transform.position = transform.position;                                                                        // ��������� ������� ������� ������ ������� ������
        obj.transform.rotation = transform.rotation;                                                                        // ��������� �������� ������� ������ �������� ������
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);                   // �������� ������ ������� bullet
        bullet.SetPool(this.bulletPool);                                                                                    // ��������� ������ �� Pool ��� ������ bullet.
        return bullet;                                                                                                      // ����������� ��������� ����
    }

    private void Shoot()
    {
        //Shooting rate
        if (shootTimer >= 0)
            shootTimer -= Time.deltaTime * shootRate;                                                                       // ���������� ������� �������� � ������ ������� ����� � ������� ��������

        //Shoot
        if (shootTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                shootTimer = 1;                                                                                             // ����� ������� �������� �� 1 �������
                bulletPool?.Get();                                                                                          // ��������� ������� bullet �� ���� (���� ��� �� ����)
            }
        }
    }
}