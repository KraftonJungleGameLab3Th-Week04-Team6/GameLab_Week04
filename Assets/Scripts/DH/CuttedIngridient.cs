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

        for (int i = 0; i < 3000; i++)
        {
            float t = (float)i / 300;

            transform.position = new(nowPosition.x + velocity.x * t, nowPosition.y + velocity.y * t - 9.8f * t * t / 2);

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}
