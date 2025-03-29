using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class TextEffect : MonoBehaviour
{
    public int value;
    public Sprite[] sprites;
    public Color[] colors;

    RectTransform _rectTransform;
    Image _image;
    TextMeshProUGUI _textMeshProUGUI;
    public void Init()
    {
        SetFX();
        Destroy(gameObject, 1.5f);
    }

    public void SetFX()
    {
        float Ymove = value > 0 ? 1 : -1;
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponentInChildren<Image>();
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();

        _image.sprite = sprites[value > 0 ? 0 : 1];
        _textMeshProUGUI.text = Mathf.Abs(value).ToString();
        _textMeshProUGUI.color = colors[value > 0 ? 0 : 1];

        _rectTransform.DOAnchorPosY(Ymove * 50, 1f);
        _image.DOFade(0f, 1.5f);
        _textMeshProUGUI.DOFade(0f, 1.5f);
    }
}
