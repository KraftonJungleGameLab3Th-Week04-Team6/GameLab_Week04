using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;
    public TooltipController tooltipController;

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("하이");
        tooltipController.ShowTooltip(message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("바이");
        tooltipController.HideTooltip();
    }
}
