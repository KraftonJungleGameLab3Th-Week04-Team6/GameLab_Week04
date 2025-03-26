using System.Collections;
using UnityEngine;


public class PlayOpening : MonoBehaviour
{
    private Canvas canvas;
    private GameObject[] openingObjects;

    private void Awake()
    {
        canvas = FindAnyObjectByType<Canvas>();

        openingObjects = new GameObject[canvas.transform.childCount];
        for(int i=0;i< canvas.transform.childCount; i++)
        {
            openingObjects[i] = canvas.transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        foreach (GameObject openingObject in openingObjects) openingObject.SetActive(false);

        StartCoroutine(StartOpening());
    }

    private IEnumerator StartOpening()
    {
        openingObjects[0].SetActive(true);

        yield return new WaitForSeconds(1f);

        openingObjects[2].SetActive(true);

        yield return new WaitForSeconds(6.5f);

        openingObjects[0].SetActive(false);
        openingObjects[2].SetActive(false);
        openingObjects[1].SetActive(true);

        yield return new WaitForSeconds(1f);

        openingObjects[3].SetActive(true);

        yield return new WaitForSeconds(11f);

        Manager.Game.GameStart();
    }
}
