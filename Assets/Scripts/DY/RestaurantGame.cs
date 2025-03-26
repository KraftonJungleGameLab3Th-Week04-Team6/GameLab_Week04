using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RestaurantGame : MonoBehaviour
{
    private int _customerNum;
    private UI_RestaurantCanvas _uiRestaurantCanvas; 
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Debug.Log("RestaurantGame Init");
        // 1~10번 손님 중 랜덤
        _uiRestaurantCanvas = FindAnyObjectByType<UI_RestaurantCanvas>();
        _customerNum = Random.Range(1, 11);
        SetCustomer();
    }
    
    private void SetCustomer()
    {
        Manager.Restaurant.SendCustomer(_customerNum);
    }
    
    
    
}
