using System.Collections;
using UnityEngine;

public class PlayEnding : MonoBehaviour
{
    private Canvas _canvas;
    private GameObject[] _endingObjects;
    private DisplayText _typingText;
    private WaitForSecondsRealtime _delay;

    private void Awake()
    {
        _canvas = FindAnyObjectByType<Canvas>();
        _endingObjects = new GameObject[_canvas.transform.childCount];
        for (int i = 0; i < _canvas.transform.childCount; i++)
        {
            _endingObjects[i] = _canvas.transform.GetChild(i).gameObject;
        }
        _delay = new WaitForSecondsRealtime(1f);
    }

    private void Start()
    {
        foreach (GameObject endingObject in _endingObjects) endingObject.SetActive(false);

        StartCoroutine(StartEnding());
    }

    private IEnumerator StartEnding()
    {
        switch (Manager.Game.EndingType)
        {
            case 1: // 해피엔딩
                _endingObjects[0].SetActive(true);

                yield return _delay;

                _endingObjects[3].SetActive(true);

                _typingText = _endingObjects[3].GetComponent<DisplayText>();

                while (!_typingText.IsDone) yield return null;

                break;
            case 2: // 파산엔딩
                _endingObjects[1].SetActive(true);

                yield return _delay;

                _endingObjects[4].SetActive(true);

                _typingText = _endingObjects[4].GetComponent<DisplayText>();

                while (!_typingText.IsDone) yield return null;

                break;
            case 3: // 식중독엔딩
                _endingObjects[2].SetActive(true);

                yield return _delay;

                _endingObjects[5].SetActive(true);

                _typingText = _endingObjects[5].GetComponent<DisplayText>();

                while (!_typingText.IsDone) yield return null;

                break;
            default:
                //unbehavior
                break;
        }

        _endingObjects[6].SetActive(true);

        yield break;
    }
}
