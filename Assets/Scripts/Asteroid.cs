using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    Collider2D hit;

    private void Start()
    {
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
                Destroy(gameObject);
                Destroy(hit.gameObject);
            }

            if (hit.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
                Destroy(hit.gameObject);
            }
        }
    }
}
