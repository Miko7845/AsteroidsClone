using UnityEngine;

public class WrappingEffect : MonoBehaviour
{
    // Координата всех границ экрана.
    private float screenHeight = 10.0f;
    private float screenWidth = 18.0f;

    void Update()
    {
        ScreenWrap();
    }

    public void ScreenWrap()
    {
        Vector2 newPos = transform.position;                // Создаем вектор новой позиции и инициализируем его текущей позицией объекта

        // Если объект вышел за границу экрана, перемещаем его на противоположную границу экрана.
        if (transform.position.y > screenHeight)
            newPos.y = -screenHeight;
        else if (transform.position.y < -screenHeight)
            newPos.y = screenHeight;
        else if (transform.position.x < -screenWidth)
            newPos.x = screenWidth;
        else if (transform.position.x > screenWidth)
            newPos.x = -screenWidth;

        transform.position = newPos;                        // Обновляем позицию объекта с новым вектором
    }
}
