using UnityEngine;

public class WrappingEffect : MonoBehaviour
{
    // Координата всех границ экрана.
    private float screenTop = 10.0f;
    private float screenBottom = -10.0f;
    private float screenLeft = -18.0f;
    private float screenRight = 18.0f;

    void Update()
    {
        ScreenWrap();
    }

    public void ScreenWrap()
    {
        Vector2 newPos = transform.position;                // Создаем вектор новой позиции и инициализируем его текущей позицией объекта

        // Если объект вышел за границу экрана, перемещаем его на противоположную границу экрана.
        if (transform.position.y > screenTop)
            newPos.y = screenBottom;
        else if (transform.position.y < screenBottom)
            newPos.y = screenTop;
        else if (transform.position.x < screenLeft)
            newPos.x = screenRight;
        else if (transform.position.x > screenRight)
            newPos.x = screenLeft;

        transform.position = newPos;                        // Обновляем позицию объекта с новым вектором
    }
}
