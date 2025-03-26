using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RestaurantGame : MonoBehaviour
{
    private int _customerNum;
    private UI_RestaurantCanvas _uiRestaurantCanvas; 
    
    private void Start()
    {
        if (Manager.Game.TodayCustomerCount < Manager.Game.TodayCustomerMaxCount)
        {
            SetCustomerUI();
        }
        else
        {
            SetResultUI();
        }
    }

    private void SetCustomerUI()
    {
        Debug.Log("RestaurantGame Init");
        // 1~10번 손님 중 랜덤
        _uiRestaurantCanvas = FindAnyObjectByType<UI_RestaurantCanvas>();
        _customerNum = Random.Range(1, 11);
        Manager.Restaurant.CurrentCustomNum = _customerNum;
        SetCustomer();
    }
    
    private void SetCustomer()
    {
        Manager.Restaurant.SendCustomer(_customerNum);
    }

    private void SetResultUI()
    {
        Debug.Log("Today's result");
        // 일일 정산 UI
    }
}
