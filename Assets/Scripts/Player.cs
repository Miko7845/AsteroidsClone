using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    private float verticalInputAcceleration = 5.0f;                                                                         // Ускорение по вертикали при нажатии клавиш.
    private float horizontalInputAcceleration = 100.0f;                                                                     // Ускорение по горизонтали при нажатии клавиш.

    private float maxSpeed = 10.0f;                                                                                         // Максимальная скорость движения.
    private float maxRotationSpeed = 100.0f;                                                                                // Максимальная скорость поворота.

    private float velocityDrag = 1.0f;                                                                                      // Коэффициент замедления скорости.
    private float rotationDrag = 1.0f;                                                                                      // Коэффициент замедления поворота.

    private Vector3 velocity;                                                                                               // Вектор скорости движения.
    private float zRotationVelocity;                                                                                        // Скорость поворота по оси Z.

    public bool mouseControlOn = false;                                                                                     // Флаг включения управления мышью.

    //private float shootRate = 3.0f;                                                         // Скорость стрельбы (количество выстрелов в секунду)
    //private float shootTimer;                                                               // Таймер для контроля скорости стрельбы

    //private IObjectPool<Bullet> bulletPool;                                                 // Пул объектов для снарядов

    private void Awake()
    {
       // bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 5);       // Создаем пул объектов для снарядов с заданными параметрами.
    }

    private void Update()
    {
        // Ввод по вертикали.
        Vector3 acceleration = Input.GetAxis("Vertical") * verticalInputAcceleration * transform.up;                       // Вычисляем вектор ускорения по вертикали в зависимости от нажатой клавиши и направления игрока.
        velocity += acceleration * Time.deltaTime;                                                                         // Прибавляем ускорение к скорости движения с учетом времени кадра.

        // Ввод по горизонтали
        float zTurnAcceleration = -1 * Input.GetAxis("Horizontal") * horizontalInputAcceleration;                          // Вычисляем ускорение поворота по оси Z в зависимости от нажатой клавиши и знака минуса (для инверсии направления).
        zRotationVelocity += zTurnAcceleration * Time.deltaTime;                                                           // Прибавляем ускорение поворота к скорости поворота с учетом времени кадра

     //   Shoot(); // Вызываем метод стрельбы
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
}