using UnityEngine;

public class GameManager
{
    public bool IsGameStart => _isGameStart;
    public bool IsPause => _isPause;
    
    #region 게임 흐름 관련
    private bool _isGameStart;
    private bool _isPause;
    #endregion

    public void Init()
    {
        GameStart();
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");
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
        Debug.LogWarning("게임 종료");
        Application.Quit();
    }
    
}
