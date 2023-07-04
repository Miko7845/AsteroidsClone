using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float minSpeed = 1.0f;
    public float maxSpeed = 5.0f;
    [HideInInspector] public float minSize = 0.5f;
    [HideInInspector] public float midSize = 1f;
    [HideInInspector] public float maxSize = 2f;
    [HideInInspector] public float speed;

    [SerializeField] Sprite[] sprites;
    [SerializeField] int splitAmount = 2;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
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

    /// <summary> —оздает осколки атероидов </summary>
    void CreateSplit(float size)
    {
        // √енерируем случайную скорость дл€ частей астероида
        float newSpeed = GetRandomSpeed(minSpeed, speed); 
        for (int i = 0; i < splitAmount; i++)
        {
            Vector2 position = transform.position;
            position += Random.insideUnitCircle * 0.5f;
            Asteroid half = Instantiate(this, position, transform.rotation);
            half.transform.localScale = Vector3.one * size;
            half.speed = newSpeed;
            float angle = i == 0 ? 45f : -45f;
            half.transform.Rotate(0f, 0f, angle);
        }
    }

    /// <summary> ¬озвращает случайное число в заданном диапазоне </summary>
    public float GetRandomSpeed(float min, float max)
    {
        return Random.Range(min, max);  
    }
}