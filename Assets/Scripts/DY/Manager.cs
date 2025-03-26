using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }
    
    

    #region 매니저
    public static GameManager Game { get { return Instance._game; } }
    public static UIManager UI {  get { return Instance._ui; } }
    
    private GameManager _game;
    private UIManager _ui;
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
        UI.Init();
    }

    private void OnDestroy()
    {
    }
}
