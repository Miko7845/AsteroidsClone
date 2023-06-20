using System.Drawing;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private Collider2D hit;
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);     // Устанавливаем случайный угол поворота объекта вокруг оси Z
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);                   // Двигаем объект

        CustomTrigger();
    }

    private void CustomTrigger()
    {
        hit = Physics2D.OverlapCircle(transform.position, 1.19f);

        if (hit != null)
        {

            if (hit.gameObject.CompareTag("Bullet"))
            {
                FindObjectOfType<GameManager>().AsteroidDestroyed(this);

                if (transform.localScale.y == maxSize)
                    CreateSplit(midSize);
                else if (transform.localScale.y == midSize)
                    CreateSplit(minSize);

                Destroy(gameObject);
                Destroy(hit.gameObject);
            }

            if (hit.gameObject.CompareTag("Player"))
            {
                FindObjectOfType<GameManager>().AsteroidDestroyed(this);
                FindObjectOfType<GameManager>().PlayerDied();
                Destroy(gameObject);
                hit.gameObject.SetActive(false);
            }
        }
    }

    private void CreateSplit(float size)
    {
        float newSpeed = GetRandomSpeed(minSpeed, speed);
        for(int i = 0; i < splitAmount; i++)
        {
            Vector2 position = transform.position;
            position += Random.insideUnitCircle * 0.5f;

            Asteroid half = Instantiate(this, position, transform.rotation);
            half.transform.localScale = Vector3.one * size;
            half.speed = newSpeed;                                                                  // Скорость новых астероидов: 1. Одинаковая. 2. Значение случайное. 3. Не больше, чем скорость уничтоженного астероида.
        }
    }

    public float GetRandomSpeed(float min, float max)
    {
        return Random.Range(min, max);
    }
}
