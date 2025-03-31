using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager
{
    public bool IsGameStart => _isGameStart;
    public bool IsPause => _isPause;

    public int EndingType { get { return _endingType; } set { _endingType = value; } }

    public int CurrentDay { get { return _currentDay; } set { _currentDay = value; } }
    public int TodayCustomerCount { get { return _todayCustomerCount; } set { _todayCustomerCount = value; } }

    public int TodayCustomerMaxCount => _todayCustomerMaxCount;
        
    public float LossRate { get { return _LossRate;} set{_LossRate = value;} }
    public float MoldRate { get { return _MoldRate;} set{_MoldRate = value;} }

    public int TodayGetMoney { get { return _todayGetMoney; } set { _todayGetMoney = value; } }
    public int TotalMoney { get { return _totalMoney; } set { _totalMoney = value; } }

    public int Popularity { get { return _popularity; } set { _popularity = value; } }

    public bool SafeMoldMode { get { return _safeMode; } set { _safeMode = value; } }

    #region 게임 흐름 관련
    private bool _isGameStart;
    private bool _isPause;
    #endregion

    private int _endingType;

    #region 레스토랑 관련
    private int _currentDay;
    private int _maxDay;
    private int _todayCustomerCount;
    private int _todayCustomerMaxCount;
    #endregion

    #region 주방 관련
    private float _LossRate;
    private float _MoldRate;
    private bool _safeMode;
    #endregion

    #region 돈 관련
    private int _todayGetMoney;
    private int _totalMoney;
    #endregion

    #region 인기도 관련
    private int _popularity;
    #endregion

    public void Init()
    {
        _currentDay = 1;
        _maxDay = 5;
        _todayCustomerCount = 0;
        _todayCustomerMaxCount = 3;
        TodayGetMoney = 0;
        TotalMoney = 0;
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");
        SceneManager.LoadScene("DY_RestaurantScene");
    }

    public void GameOpeningStart()
    {
        SceneManager.LoadScene("DH_OpeningScene");
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
        _todayCustomerCount++;
        Debug.Log("주방으로 이동");
        SceneManager.LoadScene("DY_KitchenScene");
    }

    public void GoResult()
    {
        Debug.Log("결과창으로 이동");
        SceneManager.LoadScene("JH_CustomerResultScene");
    }
    
    public void NextDay()
    {
        _currentDay++;
        _todayCustomerCount = 0;
        _todayCustomerMaxCount = 3;
        Debug.Log("다음 날로 이동");
        SceneManager.LoadScene("DY_RestaurantScene");
    }

    public void GoMain()
    {
        Debug.Log("메인화면으로 이동");
        Init();
        SceneManager.LoadScene("DY_MainScene");
    }

    public void GoEnding(int endingType)
    {
        _endingType = endingType;
        Debug.Log("엔딩창으로 이동");
        SceneManager.LoadScene("DH_EndingScene");
    }
}
