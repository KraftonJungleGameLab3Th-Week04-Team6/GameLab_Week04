using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    Button _button;
    public EButton Type;
    public int Index; // Answer 버튼의 순서
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

            case EButton.Answer:
                SceneManager.LoadScene(EScenName.JH_KitchenScene.ToString());
                Manager.Kitchen.Order(Manager.Customer.IndexToOrderId[Index]);
                break;
        }
    }

}
