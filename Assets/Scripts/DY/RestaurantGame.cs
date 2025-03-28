using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RestaurantGame : MonoBehaviour
{
    private int _customerNum;
    private UI_RestaurantCanvas _uiRestaurantCanvas; 
    private int _customerIndex;
    
    private void Start()
    {
        if (Manager.Game.TodayCustomerCount < Manager.Game.TodayCustomerMaxCount)
        {
            if (Manager.Game.CurrentDay == 1 && Manager.Game.TodayCustomerCount == 0)
            {
                Manager.Restaurant.Init();

            }
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
        _uiRestaurantCanvas = FindAnyObjectByType<UI_RestaurantCanvas>();

        // 전체 15명의 손님 중 몇번째 손님인지 계산
        _customerIndex = (Manager.Game.CurrentDay - 1) * 3 + Manager.Game.TodayCustomerCount;
        // 0번째 손님 불러오기
        _customerNum = Manager.Restaurant.CustomerList[_customerIndex];
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
