using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float speed = 5.0f;
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
}
