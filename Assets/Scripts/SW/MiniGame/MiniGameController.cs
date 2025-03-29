using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour
{

    public Action OnSliceEndEvent;

    public List<GameObject> Foods;
    public float sliceTime;
    public bool isStart;

    private JSW_CheckArea _checkArea;
    private GameObject _nowFood;
    private ButtonCanvas _buttonCanvas;
    private TMP_Text _playTimeText;
    private GameObject _enddingCavas;
    private ChoppingBoard _choppingBoard;
    private JSW_DrawLine _drawLine;

    private float _resultRemainingPercentage;
    private float _moldPercentage;
    private GameObject _previewPanel;
    private Image _resultFood;
    private TMP_Text _resultFoodText;
public float ResultRemainingPercentage { get { return _resultRemainingPercentage; } set { _resultRemainingPercentage = value; } }
    public float MoldPercentage { get { return _moldPercentage; } set { _moldPercentage = value; } }

    //주문 받은 메뉴
    private MenuData _menuData;
    private int _totalFoodNum;
    private Toggle _safeModeToggle;
    private String _menuName;


    private void Start()
    {
        _checkArea = FindAnyObjectByType<JSW_CheckArea>();
        _buttonCanvas = FindAnyObjectByType<ButtonCanvas>();
        _enddingCavas = FindAnyObjectByType<PlayEnddingCanvas>().gameObject;
        _choppingBoard = FindAnyObjectByType<ChoppingBoard>();
        _previewPanel = FindAnyObjectByType<PreviewPanel>().gameObject;
        _resultFood = FindAnyObjectByType<ResultFood>().GetComponent<Image>();
        _drawLine = FindAnyObjectByType<JSW_DrawLine>();
        _safeModeToggle = FindAnyObjectByType<Toggle>();
        _resultFoodText = FindAnyObjectByType<ResultFoodText>().GetComponent<TMP_Text>();

        _playTimeText = _buttonCanvas.transform.GetComponentInChildren<PlayTimeText>().GetComponent<TMP_Text>();
        _enddingCavas.SetActive(false);
        _safeModeToggle.isOn = Manager.Game.SafeMoldMode;
        _safeModeToggle.onValueChanged.AddListener(OnSafeModeToggleChanged);

        _menuData = MenuDatabase.ObjectData[Manager.Kitchen.MenuKey];
        for (int i = 0; i < _menuData.menuIngredients.Count; i++)
        {
            Foods.Add(IngredientsDatabase.ObjectData[_menuData.menuIngredients[i]].IngredientsPrefab);
        }

        _menuName = _menuData.menuName;

        _totalFoodNum = Foods.Count;

        Manager.Kitchen.ResultRemainingPercentage = 0;
        Manager.Kitchen.MoldPercentage = 0;

        _choppingBoard.transform.DOLocalMove(Vector3.zero, 0.5f);

        // 프리뷰에 음식 등록
        for (int i = 0; i < Foods.Count; i++)
        {
            _previewPanel.transform.GetChild(i).gameObject.SetActive(true);
            _previewPanel.transform.GetChild(i).GetComponent<Image>().sprite = Foods[i].transform.GetComponent<SpriteRenderer>().sprite;
        }

        OnStartButton();
    }



    private void Update()
    {
        if (isStart)
        {
            sliceTime -= Time.deltaTime;
            if (sliceTime <= 0)
            {
                OnEndButton();
            }
            _playTimeText.text = sliceTime.ToString("F1");
        }
    }

    public void OnStartButton()
    {
        OnSettingFood();
    }

    public void OnEndButton()
    {
        if (!isStart) return;
        isStart = false;
        OnSliceEndEvent?.Invoke();
        StartCoroutine("CalculateScore", 1);
    }


    public void OnSettingFood()
    {
        if (Foods.Count == 0)
        {
            return;
        }
        StartCoroutine(Co_OnSettingFood());
    }

    IEnumerator Co_OnSettingFood()
    {
        yield return new WaitForSeconds(0.5f);
        _choppingBoard.transform.DOLocalMove(Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StartPlay_Coroutine());

        GameObject nowFood = Foods[0].transform.gameObject;
        nowFood.transform.position = transform.position;
        _nowFood = Instantiate(nowFood);
        _drawLine.NowIngredient = _nowFood;
    }

    public void OnSumitFood()
    {
        _choppingBoard.transform.DOLocalMove(new Vector3(2000, 0, 0), 0.5f);
        _checkArea.ResetCheckArea();

        // 오른쪽 위 알파 값 조절
        Image img = _previewPanel.transform.GetChild(_totalFoodNum - Foods.Count).GetComponent<Image>();
        Color c = img.color;
        c.a = 60f / 255f;
        img.color = c;

        Foods.RemoveAt(0);
        Destroy(_nowFood);
        sliceTime = 0;
        _playTimeText.text = sliceTime.ToString("F1");

        if (Foods.Count == 0)
        {
            float resultRemainingPercentage = Manager.Kitchen.ResultRemainingPercentage / _totalFoodNum;
            float moldPercentage = Manager.Kitchen.MoldPercentage / _totalFoodNum;

            Manager.Game.TodayGetMoney += (int)resultRemainingPercentage * _menuData.menuPrice / 100;

            //_enddingCavas.GetComponent<PlayEnddingCanvas>().Losstext.text = "남은 재료 비율 : " + resultRemainingPercentage.ToString("F1") + "%";
            //_enddingCavas.GetComponent<PlayEnddingCanvas>().Moldtext.text = "곰팡이 비율 : " + moldPercentage.ToString("F1") + "%";
            //_enddingCavas.GetComponent<PlayEnddingCanvas>().Moneytext.text = "수익 : " + (int)resultRemainingPercentage * _menuData.menuPrice / 100 + "원";

            Manager.Kitchen.ResultRemainingPercentage = resultRemainingPercentage;
            Manager.Kitchen.MoldPercentage = moldPercentage;
            

            StartCoroutine(EndMiniGame_Co());
            return;
        }
        OnStartButton();
    }

    IEnumerator EndMiniGame_Co()
    {
        yield return new WaitForSeconds(1f);
        _previewPanel.SetActive(false);
        _enddingCavas.SetActive(true);
        _playTimeText.gameObject.SetActive(false);
        _choppingBoard.transform.DOLocalMove(Vector3.zero, 0.8f);
  
        _resultFood.transform.DOLocalMove(Vector3.zero, 0.8f);
        // 이미지 주기
        _resultFood.sprite = _menuData.menuImage;
        yield return new WaitForSeconds(1.1f);

        _resultFoodText.text = _menuName;
        _resultFoodText.DOFade(1f, 1f);
        _resultFood.transform.DOScale(Vector3.one * 1.2f, 1);
    }

    public void GoLobby()
    {
        SceneManager.LoadScene("JH_CustomerResultScene");
    }

    IEnumerator StartPlay_Coroutine()
    {
        _buttonCanvas.GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(0.7f);
        _checkArea.ResetCheckArea();
        MoldSpawner moldSpawner = _nowFood.GetComponent<MoldSpawner>();
        moldSpawner.SettingMoldCount(230, 0.5f - ((Manager.Game.CurrentDay - 1) / 2) * 0.05f, 0.35f, 0.25f, 1 + Manager.Game.CurrentDay / 2, 1.6f);
        moldSpawner.StartMold();
        _checkArea.SetFoodCollider(_nowFood.GetComponent<Collider2D>());

        isStart = true;
        sliceTime = 10;
    }
    IEnumerator CalculateScore()
    {
        _checkArea.CalculateAreaPercentage();
        yield return new WaitForSeconds(0.5f);
        OnSumitFood();
    }

    void OnSafeModeToggleChanged(bool value)
    {
        Manager.Game.SafeMoldMode = value;
    }
}
