using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_StartCanvas : MonoBehaviour
{
    private Button _startButton;
    private Button _settingsButton;
    private Button _exitButton;


    private void Start()
    {
        _startButton = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        _settingsButton = transform.GetChild(1).GetChild(1).GetComponent<Button>();
        _exitButton = transform.GetChild(1).GetChild(2).GetComponent<Button>();
        
        _startButton.onClick.AddListener(OnStartClick);
        _settingsButton.onClick.AddListener(OnTutorialClick);
        _exitButton.onClick.AddListener(OnEndClick);
    }
    
    private void OnStartClick()
    {
        Debug.Log("Start");
        Manager.Game.GameOpeningStart();
    }
    
    private void OnTutorialClick()
    {
        Debug.Log("Tutorial");
        SceneManager.LoadScene("JH_Tutorial");

    }
    
    private void OnEndClick()
    {
        Debug.Log("End");
        Manager.Game.GameExit();
    }
}
