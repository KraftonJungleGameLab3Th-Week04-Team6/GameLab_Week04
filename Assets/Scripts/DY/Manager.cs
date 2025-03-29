using System;
using System.Resources;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }
    
    

    #region 매니저
    public static GameManager Game { get { return Instance._game; } }
    public static KitchenManager Kitchen { get { return Instance._kitchen; } }
    public static RestaurantManager Restaurant { get { return Instance._restaurant; } }
    public static PopularityManager Popularity { get { return Instance._popularityManager; } }

    private GameManager _game = new GameManager();
    private RestaurantManager _restaurant = new RestaurantManager();
    private KitchenManager _kitchen = new KitchenManager();
    private PopularityManager _popularityManager = new PopularityManager();
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
        // UI.Init();
        Game.Init();
        Popularity.Init();
    }

    private void OnDestroy()
    {
    }
}
