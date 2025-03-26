using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{

    public Action OnSliceEndEvent;

    public List<GameObject> Foods = new List<GameObject>();
    public float sliceTime;

    private JSW_CheckArea _checkArea;
    private GameObject _nowFood;
    private bool _isStart;

    private void Start()
    {
        _checkArea = FindAnyObjectByType<JSW_CheckArea>();
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnEndButton();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnStartButton();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnSettingFood();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnSumitFood();
        }

        if (_isStart)
        {
            sliceTime -= Time.deltaTime;
            if (sliceTime <= 0)
            {
                OnEndButton();
            }
        }
    }
    
    public void OnStartButton() 
    {
        _checkArea.ResetCheckArea();
        if (Foods.Count != 0)
        {
            Foods.RemoveAt(0);
            MoldSpawner moldSpawner = _nowFood.GetComponent<MoldSpawner>();
            moldSpawner.SettingMoldCount(230, 0.1f, 0.25f, 0.15f, 2);
            moldSpawner.StartMold();
            _checkArea.SetFoodCollider(_nowFood.GetComponent<Collider2D>());
        }
        _isStart = true;
        sliceTime = 10;
    }

    public void OnEndButton()
    {
        _isStart = false;
        OnSliceEndEvent?.Invoke();
        StartCoroutine("CalculateScore", 2);
    }

    public void OnSettingFood()
    {
        GameObject nowFood = Foods[0].transform.GetChild(0).gameObject;
        _nowFood = Instantiate(nowFood);
        _nowFood.transform.position = transform.position;
    }

    public void OnSumitFood()
    {
        _checkArea.ResetCheckArea();
        Destroy(_nowFood);
    }

    IEnumerator CalculateScore()
    {
        _checkArea.CalculateAreaPercentage();
        yield return null;
    }
}
