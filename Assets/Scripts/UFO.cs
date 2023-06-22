using System.Collections;
using UnityEngine;

public class UFO : MonoBehaviour
{
    internal Vector2 positionToMove;

    void Start()
    {
        StartCoroutine(LerpPosition(positionToMove, 10));
    }

    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
