using System.Collections;
using UnityEngine;

public class PlayEnding : MonoBehaviour
{
    private Canvas _canvas;
    private GameObject[] _openingObjects;
    private TypingText _typingText;
    private WaitForSeconds _delay;

    private void Awake()
    {
        _canvas = FindAnyObjectByType<Canvas>();
        _openingObjects = new GameObject[_canvas.transform.childCount];
        for (int i = 0; i < _canvas.transform.childCount; i++)
        {
            _openingObjects[i] = _canvas.transform.GetChild(i).gameObject;
        }
        _delay = new WaitForSeconds(50 * Time.deltaTime);
    }

    private void Start()
    {
        foreach (GameObject openingObject in _openingObjects) openingObject.SetActive(false);

        StartCoroutine(StartEnding());
    }

    private IEnumerator StartEnding()
    {
        switch (Manager.Game.EndingType)
        {
            case 1: // 해피엔딩
                _openingObjects[0].SetActive(true);

                yield return _delay;

                _openingObjects[3].SetActive(true);

                _typingText = _openingObjects[3].GetComponent<TypingText>();

                while (!_typingText.IsDone) yield return null;

                break;
            case 2: // 파산엔딩
                _openingObjects[1].SetActive(true);

                yield return _delay;

                _openingObjects[4].SetActive(true);

                _typingText = _openingObjects[3].GetComponent<TypingText>();

                while (!_typingText.IsDone) yield return null;

                break;
            case 3: // 식중독엔딩
                _openingObjects[2].SetActive(true);

                yield return _delay;

                _openingObjects[3].SetActive(true);

                _typingText = _openingObjects[5].GetComponent<TypingText>();

                while (!_typingText.IsDone) yield return null;

                break;
            default:
                //unbehavior
                break;
        }

        Manager.Game.GoMain();

        yield break;
    }
}
