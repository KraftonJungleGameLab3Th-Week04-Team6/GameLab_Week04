using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;
    public TooltipController tooltipController;
    public bool isClear;
    public int endingNum;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = new Color(0, 0, 0);
        if (!isClear) { color = new Color(1, 0, 0); }

        print("하이");
        tooltipController.ShowTooltip(message, color);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("바이");
        tooltipController.HideTooltip();
    }
}
