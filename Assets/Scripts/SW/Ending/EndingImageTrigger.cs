using UnityEngine;

using UnityEngine.EventSystems;
public class EndingImageTrigger : MonoBehaviour, IPointerClickHandler
{
    private PicClickCanvas _pickClickCanvas;
    private TooltipTrigger _tooltipTrigger;

    private void Start()
    {
        _pickClickCanvas = FindAnyObjectByType<PicClickCanvas>();
        _tooltipTrigger = GetComponent<TooltipTrigger>();
    }

    // 클릭 시 호출됨
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("이미지 클릭됨!");

        if (_pickClickCanvas != null && _tooltipTrigger.isClear)
        {
            _pickClickCanvas.OnClickPic(_tooltipTrigger.endingNum);
        }
    }
}