using UnityEngine;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private CanvasGroup m_canvasGroup;
    private Canvas m_mainCanvas;
    private RectTransform m_rectTransform;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();

        m_mainCanvas = GetComponentInParent<Canvas>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        var slotTransform = m_rectTransform.parent;
        slotTransform.SetAsLastSibling();

        m_canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        m_rectTransform.anchoredPosition += eventData.delta / m_mainCanvas.scaleFactor;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       transform.localPosition = Vector3.zero;

        m_canvasGroup.blocksRaycasts = true;
    }
}
