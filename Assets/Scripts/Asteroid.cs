using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;


    private void Awake()
    {
    }

    private void Start()
    {
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);     // ������������� ��������� ���� �������� ������� ������ ��� Z
    }

    private void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * speed);                   // ������� ������
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
           
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player");
        }
    }
}
