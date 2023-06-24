using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    
    public float minSpeed = 1.0f;
    public float maxSpeed = 5.0f;
    internal float speed;

    [SerializeField] private int splitAmount = 2;
    [SerializeField] private float minSize = 0.5f;
    [SerializeField] private float midSize = 1f;
    [SerializeField] private float maxSize = 2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];       // ¬ыбираем случайный спрайт из массива
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);               // ƒвигаем астероид вверх с заданной скоростью
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);            // ”ведомл€ем GameManager об уничтожении астероида

            // ≈сли астероид большой или средний, то раздел€ем его на части
            if (transform.localScale.y == maxSize)                              
                CreateSplit(midSize);
            else if (transform.localScale.y == midSize)
                CreateSplit(minSize);

            Destroy(gameObject);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            // ”ведомл€ем GameManager об уничтожении астероида и игрока
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            FindObjectOfType<GameManager>().PlayerDied();
            Destroy(gameObject);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("UFO"))
        {
            FindObjectOfType<GameManager>().UFODestroyed(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    private void CreateSplit(float size)
    {
        float newSpeed = GetRandomSpeed(minSpeed, speed);                       // √енерируем случайную скорость дл€ частей астероида
        for (int i = 0; i < splitAmount; i++)
        {
            // —оздаем случайную позицию дл€ части астероида
            Vector2 position = transform.position;
            position += Random.insideUnitCircle * 0.5f;

            Asteroid half = Instantiate(this, position, transform.rotation);    // —оздаем копию текущего астероида с новой позицией и поворотом

            half.transform.localScale = Vector3.one * size;                     // «адаем размер части астероида
            half.speed = newSpeed;                                              // «адаем скорость части астероида

            // «адаем угол поворота в 45 или -45 градусов по оси y
            float angle = i == 0 ? 45f : -45f;
            half.transform.Rotate(0f, 0f, angle);
        }
    }

    public float GetRandomSpeed(float min, float max)
    {
        return Random.Range(min, max);                                          // ¬озвращает случайное число в заданном диапазоне
    }
}
