using System.Collections;
using UnityEngine;

public class CuttedIngridient : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Throw());
    }

    private IEnumerator Throw()
    {
        Vector2 velocity = new Vector2(Random.Range(-7f, 7f), Random.Range(0f, 7f));
        Vector2 nowPosition = transform.position;

        for (int i = 0; i < 1500; i++)
        {
            float t = (float)i / 300;

            transform.position = new(nowPosition.x + velocity.x * t, nowPosition.y + velocity.y * t - 10f * t * t / 2); // 잘린 재료는 포물선으로 이동 후 제거

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}
