using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

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
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");
        SceneManager.LoadScene(EScenName.JH_RestaurantScene.ToString());
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
    
}
