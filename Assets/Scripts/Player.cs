using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool mouseControlOn = true;          // ���� ��������� ���������� �����.

    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float verticalInputAcceleration = 10.0f;     // ��������� �� ��������� ��� ������� ������.
    [SerializeField] float horizontalInputAcceleration = 150.0f;  // ��������� �� ����������� ��� ������� ������.
    [SerializeField] float maxSpeed = 10.0f;                      // ������������ �������� ��������.
    [SerializeField] float maxRotationSpeed = 100.0f;             // ������������ �������� ��������.
    [SerializeField] float velocityDrag = 1.0f;                   // ����������� ���������� ��������.
    [SerializeField] float rotationDrag = 1.0f;                   // ����������� ���������� ��������.
    [SerializeField] float shootRate = 3f;                        // �������� �������� (���������� ��������� � �������)
    Vector3 velocity;                                             // ������ �������� ��������.
    IObjectPool<Bullet> bulletPool;
    GameManager gameManager;
    float zRotationVelocity;                                      // �������� �������� �� ��� Z.
    float shootTimer;

    
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 5);       // ������� ��� �������� ��� �������� � ��������� �����������.
    }

    void Update()
    {
        // ���� �� ���������.
        Vector3 acceleration = Input.GetAxis("Vertical") * verticalInputAcceleration * transform.up;  // ��������� ������ ��������� �� ��������� � ����������� �� ������� ������� � ����������� ������.
        velocity += acceleration * Time.deltaTime;     // ���������� ��������� � �������� �������� � ������ ������� �����.
                                                                                                      
        // ���� �� �����������                                                                        
        float zTurnAcceleration = -1 * Input.GetAxis("Horizontal") * horizontalInputAcceleration;     // ��������� ��������� �������� �� ��� Z � ����������� �� ������� ������� � ����� ������ (��� �������� �����������).
        zRotationVelocity += zTurnAcceleration * Time.deltaTime;   // ���������� ��������� �������� � �������� �������� � ������ ������� �����
        Shoot();
    }

    private void FixedUpdate()
    {
        if(gameManager.isGameActive)
        {
            Movement();

            if (mouseControlOn)
                MouseRotate();
            else
                KeyRotate();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("UFOBullet"))
        {
            FindObjectOfType<GameManager>().PlayerDied();
            gameObject.SetActive(false);
        }
    }

    void Movement()
    {
        velocity = velocity * (1 - Time.deltaTime * velocityDrag);  // ���������� ��������. �������� �������� �� ����������� ���������� � ������ ������� �����
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);      // �������� ������ ��������
        transform.position += velocity * Time.deltaTime;            // (�����������) ���������� � ������� ������ ������ �������� � ������ ������� �����
    }

    void KeyRotate()
    {
        zRotationVelocity = zRotationVelocity * (1 - Time.deltaTime * rotationDrag);             // �������� �������� �������� �� ����������� ���������� � ������ ������� �����.
        zRotationVelocity = Mathf.Clamp(zRotationVelocity, -maxRotationSpeed, maxRotationSpeed); // �������� �������� �������� �� ������ �� ������������ �������� ��������.
        transform.Rotate(0, 0, zRotationVelocity * Time.deltaTime);                              // (�������) ������������ ������ �� ��� Z �� ����, ������ �������� �������� � ������ ������� �����.
    }

    void MouseRotate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;  // ��������� ������ ����������� �� ������ � ����.
        float angle = Vector2.SignedAngle(Vector2.up, direction);  // ��������� ���� ����� ������������ ������������ � �������� ����������� � ������ �����.
        Vector3 targetRotation = new Vector3(0, 0, angle);  // ������� ������ �������� �������� �� ����
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), maxRotationSpeed * Time.deltaTime);  // ������������ ������ � �������� �������� � �������� ������������ ��������� �������� � ������ ������� �����
    }

    void OnDestroyBullet(Bullet obj)
    {
        Destroy(obj.gameObject);    // ����������� ������� ��� �������� �� ����
    }

    void OnReleaseBullet(Bullet obj)
    {
        obj.gameObject.SetActive(false);    // ����������� ������� ��� ����������� � ���
    }

    void OnGetBullet(Bullet obj)
    {
        obj.gameObject.SetActive(true);  // A�������� ������� ��� ��������� �� ����
        obj.transform.position = transform.position;  // ��������� ������� ������� ������ ������� ������
        obj.transform.rotation = transform.rotation;  // ��������� �������� ������� ������ �������� ������
    }

    Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.SetPool(this.bulletPool);  // ��������� ������ �� Pool ��� ������ bullet.
        return bullet;
    }

    void Shoot()
    {
        if (shootTimer >= 0)
            shootTimer -= Time.deltaTime * shootRate;

        if (shootTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                shootTimer = 1;     // ����� ������� �������� �� 1 �������
                bulletPool?.Get();  // ��������� ������� bullet �� ���� (���� ��� �� ����)
            }
        }
    }
}