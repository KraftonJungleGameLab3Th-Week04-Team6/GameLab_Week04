using System.Collections;
using TMPro;
using UnityEngine;

public class PlayEnding : MonoBehaviour
{
    public Canvas _canvas;
    private GameObject[] _endingObjects;
    private DisplayText _typingText;
    private WaitForSecondsRealtime _delay;

    private void Awake()
    {

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
    }

    public void StartEnding()
    {
        StartCoroutine(Co_StartEnding());
    } 

    private IEnumerator Co_StartEnding()
    {
        //_endingObjects[_endingObjects.Length - 2].SetActive(true);
        //_endingObjects[_endingObjects.Length - 2].GetComponent<TextMeshProUGUI>().text = "당신이 번 골드: " + Manager.Game.TotalMoney;

        int EndingTypeNum = Manager.Game.EndingType;

        EndingPrefs.UnlockEnding(EndingTypeNum);

        _endingObjects[EndingTypeNum].SetActive(true);

        yield return _delay;

        _endingObjects[EndingTypeNum + 6].SetActive(true);

        _typingText = _endingObjects[EndingTypeNum + 6].GetComponent<DisplayText>();

        while (!_typingText.IsDone) yield return null;

        //switch (EndingTypeNum)
        //{
        //    case 0: // 돈 많고 평점많은엔딩
        //        _endingObjects[EndingTypeNum].SetActive(true);

        //        yield return _delay;

        //        _endingObjects[EndingTypeNum + 6].SetActive(true);

        //        _typingText = _endingObjects[EndingTypeNum + 6].GetComponent<DisplayText>();

        //        while (!_typingText.IsDone) yield return null;

        //        break;
        //    case 1: // 돈 많고 평점 적은 엔딩
        //        _endingObjects[4].SetActive(true);

        //        yield return _delay;

        //        _endingObjects[11].SetActive(true);

        //        _typingText = _endingObjects[11].GetComponent<DisplayText>();

        //        while (!_typingText.IsDone) yield return null;

        //        break;
        //    case 2: // 돈 적고 평점 많은 엔딩
        //        _endingObjects[5].SetActive(true);

        //        yield return _delay;

        //        _endingObjects[12].SetActive(true);

        //        _typingText = _endingObjects[12].GetComponent<DisplayText>();

        //        while (!_typingText.IsDone) yield return null;

        //        break;
        //    case 3: // 돈 적고 평점 적은 엔딩
        //        _endingObjects[6].SetActive(true);

        //        yield return _delay;

        //        _endingObjects[13].SetActive(true);

        //        _typingText = _endingObjects[13].GetComponent<DisplayText>();

        //        while (!_typingText.IsDone) yield return null;

        //        break;
        //    case 4: // 파산엔딩
        //        _endingObjects[1].SetActive(true);

        //        yield return _delay;

        //        _endingObjects[8].SetActive(true);

        //        _typingText = _endingObjects[8].GetComponent<DisplayText>();

        //        while (!_typingText.IsDone) yield return null;

        //        break;
        //    case 5: // 식중독엔딩
        //        _endingObjects[2].SetActive(true);

        //        yield return _delay;

        //        _endingObjects[9].SetActive(true);

        //        _typingText = _endingObjects[9].GetComponent<DisplayText>();

        //        while (!_typingText.IsDone) yield return null;

        //        break;
        //    default:
        //        //unbehavior
        //        break;
        //}

        _endingObjects[_endingObjects.Length-1].SetActive(true);

        yield break;
    }
}
