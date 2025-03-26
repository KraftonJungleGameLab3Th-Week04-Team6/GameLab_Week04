using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour
{

    public Action OnSliceEndEvent;

    public List<GameObject> Foods = new List<GameObject>();
    public float sliceTime;

    private JSW_CheckArea _checkArea;
    private GameObject _nowFood;
    private bool _isStart;
    private ButtonCanvas _buttonCanvas;
    private TMP_Text _playTimeText;
    private Button _endButton;
    private GameObject _enddingCavas;

    private void Start()
    {
        _checkArea = FindAnyObjectByType<JSW_CheckArea>();
        _buttonCanvas = FindAnyObjectByType<ButtonCanvas>();
        _playTimeText = _buttonCanvas.transform.GetComponentInChildren<PlayTimeText>().GetComponent<TMP_Text>();
        _endButton = FindAnyObjectByType<ButtonCanvas>().transform.GetComponentInChildren<EndButton>().GetComponent<Button>();
        _enddingCavas = FindAnyObjectByType<PlayEnddingCanvas>().gameObject ;
        _enddingCavas.SetActive(false);
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
        if (Foods.Count == 0)
        {
            _enddingCavas.SetActive(true);
            return;
        }
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
    }
    IEnumerator CalculateScore()
    {
        _checkArea.CalculateAreaPercentage();
        yield return null;
    }
}
