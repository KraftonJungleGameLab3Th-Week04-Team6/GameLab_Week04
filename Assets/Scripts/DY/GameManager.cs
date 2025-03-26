using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager
{
    public bool IsGameStart => _isGameStart;
    public bool IsPause => _isPause;
    public int CurrentDay => _currentDay;
    public int TodayCustomerCount => _todayCustomerCount;
    public int TodayCustomerMaxCount => _todayCustomerMaxCount;
        
    public float LossRate { get { return _LossRate;} set{_LossRate = value;} }
    public float MoldRate { get { return _MoldRate;} set{_MoldRate = value;} }
    
    #region 게임 흐름 관련
    private bool _isGameStart;
    private bool _isPause;
    #endregion

    #region 레스토랑 관련
    private int _currentDay;
    private int _todayCustomerCount;
    private int _todayCustomerMaxCount;
    #endregion

    #region 주방 관련
    private float _LossRate;
    private float _MoldRate;
    #endregion

    public void Init()
    {
        _currentDay = 1;
        _todayCustomerCount = 0;
        _todayCustomerMaxCount = 3;
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");
        SceneManager.LoadScene("DY_RestaurantScene");
    }

    public void SetPause()
    {
        _isPause = true;
        Time.timeScale = 0f;
    }

    public void ReleasePause()
    {
        _isPause = false;
        Time.timeScale = 1f;

    }

    public void GameExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // 유니티 에디터 종료
#else
                Application.Quit(); // 빌드된 게임 종료
#endif
        Debug.Log("게임 종료");
        Application.Quit();
    }

    public void GoKitchen()
    {
        Debug.Log("주방으로 이동");
        SceneManager.LoadScene("DY_KitchenScene");
    }
    
    public void NextDay()
    {
        _currentDay++;
        _todayCustomerCount = 0;
        _todayCustomerMaxCount = 3;
        Debug.Log("다음 날로 이동");
        SceneManager.LoadScene("DY_RestaurantScene");
    }
    
}
