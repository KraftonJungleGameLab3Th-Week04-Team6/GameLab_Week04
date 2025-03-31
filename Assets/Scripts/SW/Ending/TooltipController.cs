using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    public GameObject tooltipObject; // Tooltip Panel
    public TMP_Text tooltipText;

    void Update()
    {
        // 마우스 따라다니기
        if (tooltipObject.activeSelf)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipObject.transform.parent as RectTransform,
                Input.mousePosition, null, out position);
            tooltipObject.GetComponent<RectTransform>().anchoredPosition = position + new Vector2(15, -15);
        }
    }

    public void ShowTooltip(string message, Color color)
    {
        tooltipText.text = message;
        tooltipText.color = color;
        tooltipObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
