using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropItem : MonoBehaviour, IDropHandler
{
    private UIInventory m_uIInventory;
    [SerializeField] private Transform m_playerPosition;

    private Vector3 m_positionPrefab;
    private void Awake()
    {
        m_uIInventory = GetComponentInParent<UIInventory>();
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>(); 
        var inventory = m_uIInventory.inventory;

        inventory.Remove(this, otherItemUI.item.type, otherItemUI.item.state.amount);
        m_positionPrefab = m_playerPosition.position + Vector3.forward;
        if (otherItemUI.item.info.prefab != null)
        {
            Instantiate(otherItemUI.item.info.prefab, m_positionPrefab, m_playerPosition.rotation);
        }
        otherSlotUI.Refresh();
    }
}
