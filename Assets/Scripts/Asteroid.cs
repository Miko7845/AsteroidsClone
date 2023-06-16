using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    Collider2D hit;

    private void Start()
    {
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);     // ������������� ��������� ���� �������� ������� ������ ��� Z
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);                   // ������� ������

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
