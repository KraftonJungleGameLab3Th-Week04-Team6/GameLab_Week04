using UnityEngine;
using UnityEngine.UI;

public class PicClickCanvas : MonoBehaviour
{
    private Button _backButton;
    private GameObject _background;
    private GameObject _dialogue;

    private void Start()
    {
        _backButton = transform.GetChild(transform.childCount - 1).GetComponent<Button>();
        _backButton.onClick.AddListener(OnClickPicBack);
    }

    public void OnClickPic(int num)
    {
        _background = transform.GetChild(num).gameObject;
        _dialogue = transform.GetChild(num + 5).gameObject;
        _background.SetActive(true);
        _dialogue.SetActive(true);
        _backButton.gameObject.SetActive(true);
    }

    public void OnClickPicBack()
    {
        _background.SetActive(false);
        _dialogue.SetActive(false);
        _backButton.gameObject.SetActive(false);
    }
}
