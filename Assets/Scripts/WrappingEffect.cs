using UnityEngine;

public class WrappingEffect : MonoBehaviour
{
    // ���������� ���� ������ ������.
    private float screenHeight = 10.0f;
    private float screenWidth = 18.0f;

    void Update()
    {
        ScreenWrap();
    }

    public void ScreenWrap()
    {
        Vector2 newPos = transform.position;                // ������� ������ ����� ������� � �������������� ��� ������� �������� �������

        // ���� ������ ����� �� ������� ������, ���������� ��� �� ��������������� ������� ������.
        if (transform.position.y > screenHeight)
            newPos.y = -screenHeight;
        else if (transform.position.y < -screenHeight)
            newPos.y = screenHeight;
        else if (transform.position.x < -screenWidth)
            newPos.x = screenWidth;
        else if (transform.position.x > screenWidth)
            newPos.x = -screenWidth;

        transform.position = newPos;                        // ��������� ������� ������� � ����� ��������
    }
}
