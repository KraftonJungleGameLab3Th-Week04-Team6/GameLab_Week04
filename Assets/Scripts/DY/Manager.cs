using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }
    
    

    #region 매니저
    public static GameManager Game { get { return Instance._game; } }
    public static UIManager UI {  get { return Instance._ui; } }
    
    private GameManager _game = new GameManager();
    private UIManager _ui = new UIManager();
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
        //UI.Init();
        //Game.Init();
    }

    private void OnDestroy()
    {
    }
}
