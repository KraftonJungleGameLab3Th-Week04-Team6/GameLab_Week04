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

    private JSW_CheckArea _checkArea;
    private GameObject _nowFood;
    private bool _isStart;
    private ButtonCanvas _buttonCanvas;
    private TMP_Text _playTimeText;
    private Button _endButton;
    private GameObject _enddingCavas;
    private ChoppingBoard _choppingBoard;
    
    private float _resultRemainingPercentage;
    private float _moldPercentage;
    
    public float ResultRemainingPercentage { get{return _resultRemainingPercentage;} set{_resultRemainingPercentage = value;} }
    public float MoldPercentage { get{return _moldPercentage;} set{_moldPercentage = value;} }
    
    //주문 받은 메뉴
    private OrderData _orderData;

    private void Start()
    {
        _checkArea = FindAnyObjectByType<JSW_CheckArea>();
        _buttonCanvas = FindAnyObjectByType<ButtonCanvas>();
        _playTimeText = _buttonCanvas.transform.GetComponentInChildren<PlayTimeText>().GetComponent<TMP_Text>();
        _endButton = FindAnyObjectByType<ButtonCanvas>().transform.GetComponentInChildren<EndButton>().GetComponent<Button>();
        _enddingCavas = FindAnyObjectByType<PlayEnddingCanvas>().gameObject ;
        _choppingBoard = FindAnyObjectByType<ChoppingBoard>();

        _enddingCavas.SetActive(false);
        _endButton.gameObject.SetActive(false);

        _orderData  = OrderDatabase.ObjectData[Manager.Kitchen.OrderKey];
        for (int i = 0; i < _orderData.orderIngredients.Count; i++)
        {
            Foods.Add(IngredientsDatabase.ObjectData[_orderData.orderIngredients[i]].IngredientsPrefab);
        }
        
        Manager.Kitchen.ResultRemainingPercentage = 0;
        Manager.Kitchen.MoldPercentage = 0;

        _choppingBoard.transform.DOLocalMove(Vector3.zero, 0.5f);
        StartCoroutine(EnableButton_Coroutine());
    }

    IEnumerator EnableButton_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        _buttonCanvas.GetComponent<Canvas>().enabled = true;
        _buttonCanvas.GetComponent<GraphicRaycaster>().enabled = true;
    }

    private void Update()
    {
        if (_isStart)
        {
            sliceTime -= Time.deltaTime;
            if (sliceTime <= 0)
            {
                OnEndButton();
                _endButton.onClick.Invoke();
            }
            _playTimeText.text = sliceTime.ToString("F1");
        }
    }
    
    public void OnStartButton()
    {
        OnSettingFood();
        StartCoroutine(StartPlay_Coroutine());
    }

    public void OnEndButton()
    {
        if (!_isStart) return;
        _isStart = false;
        OnSliceEndEvent?.Invoke();
        StartCoroutine("CalculateScore", 1);
    }

    public void OnSettingFood()
    {
        if (Foods.Count == 0)
        {
            return;
        }
        GameObject nowFood = Foods[0].transform.GetChild(0).gameObject;
        _nowFood = Instantiate(nowFood);
        _nowFood.transform.position = transform.position;
    }

    public void OnSumitFood()
    { 
        _checkArea.ResetCheckArea();
        Foods.RemoveAt(0);
        Destroy(_nowFood);
        sliceTime = 0;
        _playTimeText.text = sliceTime.ToString("F1");
        
        if (Foods.Count == 0)
        {
            float resultRemainingPercentage = Manager.Kitchen.ResultRemainingPercentage / 3.0f;
            float moldPercentage = Manager.Kitchen.MoldPercentage / 3.0f;
            
            _enddingCavas.SetActive(true);


            Manager.Game.LossRate = resultRemainingPercentage;
            Manager.Game.MoldRate = moldPercentage;

            Manager.Game.TodayCustomerCount += 1;
            Manager.Game.TodayGetMoney += (int)resultRemainingPercentage * 100;

            _enddingCavas.GetComponent<PlayEnddingCanvas>().Losstext.text =  "살린 재료 : " + resultRemainingPercentage.ToString("F1") + "%";
            _enddingCavas.GetComponent<PlayEnddingCanvas>().Moldtext.text = "곰팡이 비율 : " + moldPercentage.ToString("F1") + "%";
            return;
        }
    }

    public void GoLobby()
    {
        SceneManager.LoadScene("JH_CustomerResultScene");
    }

    IEnumerator StartPlay_Coroutine()
    {      
        yield return new WaitForSeconds(0.2f);
        _checkArea.ResetCheckArea();
        //Foods.RemoveAt(0);
        MoldSpawner moldSpawner = _nowFood.GetComponent<MoldSpawner>();
        moldSpawner.SettingMoldCount(230, 0.4f, 0.25f, 0.15f, 2);
        moldSpawner.StartMold();
        _checkArea.SetFoodCollider(_nowFood.GetComponent<Collider2D>());

        _isStart = true;
        sliceTime = 10;


        yield return new WaitForSeconds(0.8f);
        _endButton.gameObject.SetActive(true);
    }
    IEnumerator CalculateScore()
    {
        _checkArea.CalculateAreaPercentage();
        yield return null;
    }
}
