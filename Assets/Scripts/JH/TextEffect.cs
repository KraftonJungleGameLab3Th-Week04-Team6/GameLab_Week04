using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
public class TextEffect : MonoBehaviour
{
    public int value;
    public Sprite[] sprites;
    public Color[] colors;

    RectTransform _rectTransform;
    public Image Image;
    TextMeshProUGUI _textMeshProUGUI;
    public void Init()
    {
        SetFX();
        Destroy(gameObject, 1.5f);
    }

    public async Task SetFX()
    {
        // 양수면 위로 이동, 음수면 아래로 이동 -> 양수 음수 모두 위로 이동
        float Ymove = 1;
        _rectTransform = GetComponent<RectTransform>();
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();

        _textMeshProUGUI.text = Mathf.Abs(value).ToString();
        // 양수인지 음수인지에 따라 스프라이트, 색 변경
        Image.sprite = sprites[value > 0 ? 0 : 1];
        _textMeshProUGUI.color = colors[value > 0 ? 0 : 1];

        // 이동방향으로 이동
        Debug.Log($"Ymove = {Ymove}");
        _rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + Ymove * 100, 1f);

        await Task.Delay(500);
        Image.DOFade(0f, 0.5f);
        _textMeshProUGUI.DOFade(0f, 0.5f);
        Debug.Log("사라지기");
    }
}
