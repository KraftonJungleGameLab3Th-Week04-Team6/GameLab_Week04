using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    Button _button;
    public EButton Type;
    public int Index;
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        switch (Type)
        {
            case EButton.GameStart:
                Manager.Game.GameStart();
                break;

            case EButton.GameExit:
                Manager.Game.GameExit();
                break;
        }
    }

}
