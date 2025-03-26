using System;
using System.Resources;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }
    
    

    #region 매니저
    public static GameManager Game { get { return Instance._game; } }
    public static CustomerManager Customer { get { return Instance._customer; } }
    public static KitchenManager Kitchen { get { return Instance._kitchen; } }
    public static RestaurantManager Restaurant { get { return Instance._restaurant; } }
    
    private GameManager _game = new GameManager();
    private KitchenManager _kitchen = new KitchenManager();
    private CustomerManager _customer = new CustomerManager();
    private RestaurantManager _restaurant = new RestaurantManager();
    //private CustomerManager _customer;
    #endregion

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
    }

    // Manager 초기화
    private void Init()
    {
        Game.Init();
        Customer.Init();
    }

    private void OnDestroy()
    {
    }
}