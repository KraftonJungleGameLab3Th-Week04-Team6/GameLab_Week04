using System.Collections;
using UnityEngine;

public class CuttedIngridient : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(Throwing());
    }

    private IEnumerator Throwing()
    {
        Vector2 velocity = new Vector2(Random.Range(-7f, 7f), Random.Range(5f, 7f));
        Vector2 nowPosition = transform.position;
        float t = 0f;

        for (int i = 0; i < 3000; i++)
        {
            transform.position = new(nowPosition.x + velocity.x * t, nowPosition.y + velocity.y * t - 10f * t * t / 2); // 잘린 재료는 포물선으로 이동 후 제거

            //if (_moldSpawner.MoldSet.Count == 0) t += 0.015f; // 끝난다면 빠르게 내려가기
            //else t += 0.003f;
            t += 2 * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}
