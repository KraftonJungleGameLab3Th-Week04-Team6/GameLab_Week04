using System.Collections;
using UnityEngine;


public class PlayOpening : MonoBehaviour
{
    private Canvas _canvas;
    private GameObject[] _openingObjects;
    private DisplayText _typingText;
    private WaitForSecondsRealtime _delay;

    private void Awake()
    {
        _canvas = FindAnyObjectByType<Canvas>();
        _openingObjects = new GameObject[_canvas.transform.childCount];
        for(int i=0;i< _canvas.transform.childCount; i++)
        {
            _openingObjects[i] = _canvas.transform.GetChild(i).gameObject;
        }
        _delay = new WaitForSecondsRealtime(1f);
    }

    private void Start()
    {
        foreach (GameObject openingObject in _openingObjects) openingObject.SetActive(false);

        StartCoroutine(StartOpening());
    }

    private IEnumerator StartOpening()
    {
        _openingObjects[0].SetActive(true);

        yield return _delay;

        _typingText = _openingObjects[0].GetComponent<DisplayText>();

        while (!_typingText.IsDone) yield return null;

        _openingObjects[0].SetActive(false);
        _openingObjects[1].SetActive(true);

        yield return _delay;

        _typingText = _openingObjects[1].GetComponent<DisplayText>();

        while (!_typingText.IsDone) yield return null;

        Manager.Game.GameStart();
    }
}
