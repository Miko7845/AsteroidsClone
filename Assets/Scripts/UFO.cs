using UnityEngine;

public class UFO : MonoBehaviour
{
    public float speed = 2.0f;

    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime);
    }
}
