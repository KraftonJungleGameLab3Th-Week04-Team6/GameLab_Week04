using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class TextEffect : MonoBehaviour
{
    public EDirection Direction;
    RectTransform _rectTransform;
    Image _image;
    TextMeshProUGUI _textMeshProUGUI;
    private void Start()
    {
        float Ymove = Direction == EDirection.Up ? 1 : -1;
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponentInChildren<Image>();
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();

        _rectTransform.DOAnchorPosY(Ymove * 50, 1f);
        _image.DOFade(0f, 1f);
        //_textMeshProUGUI.color
    }
}
