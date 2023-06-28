using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    public bool mouseControlOn = true;                                                                                     // Флаг включения управления мышью.

    private Vector3 velocity;                                                                                               // Вектор скорости движения.
    private IObjectPool<Bullet> bulletPool;
    private GameManager gameManager;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float verticalInputAcceleration = 10.0f;                                                               // Ускорение по вертикали при нажатии клавиш.
    [SerializeField] float horizontalInputAcceleration = 150.0f;                                                            // Ускорение по горизонтали при нажатии клавиш.
    [SerializeField] float maxSpeed = 10.0f;                                                                                // Максимальная скорость движения.
    [SerializeField] float maxRotationSpeed = 100.0f;                                                                       // Максимальная скорость поворота.
    [SerializeField] float velocityDrag = 1.0f;                                                                             // Коэффициент замедления скорости.
    [SerializeField] float rotationDrag = 1.0f;                                                                             // Коэффициент замедления поворота.
    [SerializeField] float shootRate = 3f;                                                                                  // Скорость стрельбы (количество выстрелов в секунду)
    private float zRotationVelocity;                                                                                        // Скорость поворота по оси Z.
    private float shootTimer;                                                                                               // Таймер для контроля скорости стрельбы

    
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 5);       // Создаем пул объектов для снарядов с заданными параметрами.
    }

    private void Update()
    {
        // Ввод по вертикали.
        Vector3 acceleration = Input.GetAxis("Vertical") * verticalInputAcceleration * transform.up;                        // Вычисляем вектор ускорения по вертикали в зависимости от нажатой клавиши и направления игрока.
        velocity += acceleration * Time.deltaTime;                                                                          // Прибавляем ускорение к скорости движения с учетом времени кадра.
                                                                                                                            
        // Ввод по горизонтали                                                                                              
        float zTurnAcceleration = -1 * Input.GetAxis("Horizontal") * horizontalInputAcceleration;                           // Вычисляем ускорение поворота по оси Z в зависимости от нажатой клавиши и знака минуса (для инверсии направления).
        zRotationVelocity += zTurnAcceleration * Time.deltaTime;                                                            // Прибавляем ускорение поворота к скорости поворота с учетом времени кадра

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

    private void Movement()
    {
        velocity = velocity * (1 - Time.deltaTime * velocityDrag);                                                          // Замедление скорости. Умножаем скорость на коэффициент замедления с учетом времени кадра
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);                                                              // Обрезаем вектор скорости
        transform.position += velocity * Time.deltaTime;                                                                    // (Перемещение) Прибавляем к позиции игрока вектор скорости с учетом времени кадра
    }

    private void KeyRotate()
    {
        zRotationVelocity = zRotationVelocity * (1 - Time.deltaTime * rotationDrag);                                        // Умножаем скорость поворота на коэффициент замедления с учетом времени кадра.
        zRotationVelocity = Mathf.Clamp(zRotationVelocity, -maxRotationSpeed, maxRotationSpeed);                            // Обрезаем скорость поворота по модулю до максимальной скорости поворота.
        transform.Rotate(0, 0, zRotationVelocity * Time.deltaTime);                                                         // (Поворот) Поворачиваем игрока по оси Z на угол, равный скорости поворота с учетом времени кадра.
    }

    private void MouseRotate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                        // Получаем позицию мыши в мировых координатах из экранных координат
        Vector2 direction = mousePosition - transform.position;                                                             // Вычисляем вектор направления от игрока к мыши.
        float angle = Vector2.SignedAngle(Vector2.up, direction);                                                           // Вычисляем угол между вертикальным направлением и вектором направления с учетом знака.
        Vector3 targetRotation = new Vector3(0, 0, angle);                                                                  // Создаем вектор целевого поворота из угла
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), maxRotationSpeed * Time.deltaTime); // Поворачиваем игрока к целевому повороту с заданной максимальной скоростью поворота и учетом времени кадра
    }

    private void OnDestroyBullet(Bullet obj)
    {
        Destroy(obj.gameObject);                                                                                            // Уничтожения объекта при удалении из пула
    }

    private void OnReleaseBullet(Bullet obj)
    {
        obj.gameObject.SetActive(false);                                                                                    // Деактивации объекта при возвращении в пул
    }

    private void OnGetBullet(Bullet obj)
    {
        obj.gameObject.SetActive(true);                                                                                     // Aктивации объекта при получении из пула
        obj.transform.position = transform.position;                                                                        // Установка позиции объекта равной позиции игрока
        obj.transform.rotation = transform.rotation;                                                                        // Установка поворота объекта равным повороту игрока
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);                   // Создания нового объекта bullet
        bullet.SetPool(this.bulletPool);                                                                                    // Установка ссылки на Pool для данной bullet.
        return bullet;                                                                                                      // Возвращение созданной пули
    }

    private void Shoot()
    {
        //Shooting rate
        if (shootTimer >= 0)
            shootTimer -= Time.deltaTime * shootRate;                                                                       // Уменьшение таймера стрельбы с учетом времени кадра и частоты стрельбы

        //Shoot
        if (shootTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                shootTimer = 1;                                                                                             // Сброс таймера стрельбы на 1 секунду
                bulletPool?.Get();                                                                                          // Получение объекта bullet из пула (если пул не пуст)
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("UFOBullet"))
        {
            FindObjectOfType<GameManager>().PlayerDied();
            gameObject.SetActive(false);
        }
    }
}