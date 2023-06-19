using System.Drawing;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float minSpeed = 2.0f;
    [SerializeField] private float maxSpeed = 6.0f;

    [SerializeField] private int splitAmount = 2;
    [SerializeField] private float minSize = 0.5f;
    [SerializeField] private float midSize = 1f;
    [SerializeField] private float maxSize = 2f;


    private float speed;
    private Collider2D hit;
    private SpriteRenderer spriteRenderer;
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);     // Устанавливаем случайный угол поворота объекта вокруг оси Z

        speed = Random.Range(minSpeed, maxSpeed);
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
        for(int i = 0; i < splitAmount; i++)
        {
            Vector2 position = transform.position;
            position += Random.insideUnitCircle * 0.5f;

            Asteroid half = Instantiate(this, position, transform.rotation);
            half.transform.localScale = Vector3.one * size;
        }
    }
}
