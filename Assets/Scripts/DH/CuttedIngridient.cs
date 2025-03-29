using System.Collections;
using UnityEngine;

public class CuttedIngridient : MonoBehaviour
{
    private MoldSpawner _moldSpawner;

    private void Awake()
    {
        _moldSpawner = GetComponentInParent<MoldSpawner>();
    }

    public void Start()
    {
        StartCoroutine(Throwing());
    }

    private IEnumerator Throwing()
    {
        Vector2 velocity = new Vector2(Random.Range(-7f, 7f), Random.Range(0f, 7f));
        Vector2 nowPosition = transform.position;
        float t = 0f;

        for (int i = 0; i < 1000; i++)
        {
            if (_moldSpawner.MoldSet.Count == 0) t += 0.015f; // 끝난다면 빠르게 내려가기
            else t += 0.003f;

            transform.position = new(nowPosition.x + velocity.x * t, nowPosition.y + velocity.y * t - 10f * t * t / 2); // 잘린 재료는 포물선으로 이동 후 제거

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}
