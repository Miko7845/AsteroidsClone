using System.Drawing;
using Unity.VisualScripting;
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
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);                   // Двигаем объект
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);

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
            Destroy(other.gameObject);
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
            half.speed = newSpeed;

            // Задаем угол поворота в 45 или -45 градусов по оси y
            float angle = i == 0 ? 45f : -45f;
            half.transform.Rotate(0f, 0f, angle);
        }
    }

    public float GetRandomSpeed(float min, float max)
    {
        return Random.Range(min, max);
    }
}
