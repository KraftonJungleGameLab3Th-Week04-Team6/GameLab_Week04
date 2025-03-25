using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void ClickButton(int index)
    {
        Debug.Log(index);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(EScenName.JH_RestaurantScene.ToString());
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // 유니티 에디터 종료
#else
        Application.Quit(); // 빌드된 게임 종료
#endif
    }
}

public enum EScenName
{
    JH_KitchenScene,
    JH_MainScene,
    JH_RestaurantScene,
}