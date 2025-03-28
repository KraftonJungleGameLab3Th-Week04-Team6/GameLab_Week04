using System.Collections;
using UnityEngine;

public class CuttedIngridient : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Throw());
    }

    // Update is called once per frame
    private IEnumerator Throw()
    {
        for(int i = 0; i < 500; i++)
        {
            transform.position += Vector3.down * 0.05f;

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}
