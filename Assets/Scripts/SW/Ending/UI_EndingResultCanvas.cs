using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndingResultCanvas : MonoBehaviour
{
    public GameObject background;
    public GameObject totalText;
    public Image imageMoney;
    public Image imagePopularity;
    public TMP_Text textMoney;
    public TMP_Text textPopularity;
    public GameObject pleaseClick;
    private float _nowtotalMoney=0;
    private float _nowtotalPopularity=0;
    private float _totalMoney;
    private float _totalPopularity;
    private PlayEnding _playerEnding;

    private void Start()
    {
        _totalMoney = Manager.Game.TotalMoney;
        _totalPopularity = Manager.Game.Popularity;
        _playerEnding = FindAnyObjectByType<PlayEnding>();

        StartCoroutine(Co_StartEnding());
        print("Ending");
    }

    IEnumerator Co_StartEnding()
    {

        print("Endding2");
        background.SetActive(true);
        totalText.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        imageMoney.gameObject.SetActive(true);
        textMoney.gameObject.SetActive(true);

        while (true)
        {
            _nowtotalMoney = Mathf.Lerp(_nowtotalMoney, _totalMoney, Time.deltaTime * 10);
            textMoney.text = ((int)_nowtotalMoney).ToString() + "원";
            if ((_totalMoney - _nowtotalMoney <= 1) ||Input.GetMouseButtonDown(0))
            {
                _nowtotalMoney = _totalMoney;
                textMoney.text = ((int)_nowtotalMoney).ToString() + "원";
                break;
            }
            yield return null;
        }

        imagePopularity.gameObject.SetActive(true);
        textPopularity.gameObject.SetActive(true);

        while (true)
        {
            _nowtotalPopularity = Mathf.Lerp(_nowtotalPopularity, _totalPopularity, Time.deltaTime * 10);
            textPopularity.text = ((int)_nowtotalPopularity).ToString();
            if ((_totalPopularity - _nowtotalPopularity <= 1) || Input.GetMouseButtonDown(0))
            {
                _nowtotalPopularity = _totalPopularity;
                textPopularity.text = ((int)_nowtotalPopularity).ToString();
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        pleaseClick.SetActive(true);

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _playerEnding.StartEnding();
                gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
        print("Endding3");

    }
}
