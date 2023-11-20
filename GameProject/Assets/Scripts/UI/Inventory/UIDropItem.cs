using TheIslandKOD;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropItem : MonoBehaviour, IDropHandler
{
    
    private Transform m_playerPosition;
    private ReferenceSystem m_referenceSystem;
    private Vector3 m_positionPrefab;
    [SerializeField] private Vector3 m_positionOffset;
    
    
    private void Start()
    {
    
        m_referenceSystem = ReferenceSystem.instance;
        m_playerPosition = m_referenceSystem.player.GetComponent<PlayerLook>().itemDropPosition.transform;   
       
    }
    public void OnDrop(PointerEventData eventData)
    {
        var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>();
        
        m_positionPrefab = m_playerPosition.position + m_positionOffset;
        if (otherItemUI.item.info.DropPrefab != null)
        {

            var prefab = Instantiate(otherItemUI.item.info.DropPrefab, m_positionPrefab, m_playerPosition.rotation);
            prefab.GetComponent<InteractableItemState>().state = otherItemUI.item.state.Clone();
            prefab.GetComponent<InteractableItemState>().item = otherItemUI.item.Clone();
        }

        DeleteInvenotyItem(otherSlotUI, otherItemUI);


        otherSlotUI.Refresh();
    }

    private void DeleteInvenotyItem(UIInventorySlot otherSlot, UIInventoryItem otherItem)
    {
        var uIStorage = otherSlot.GetComponentInParent<UIStorage>();
        var uIInventory = otherSlot.GetComponentInParent<UIInventory>();
        var inventory = uIStorage?.inventory;
        if (inventory != null && inventory.HasItem(otherItem.item.type, out var item))
        {
            inventory.Remove(this, otherItem.item.type, otherItem.item.state.amount);
        }
        else
        {
            inventory = uIInventory.inventory;
            inventory.Remove(this, otherItem.item.type, otherItem.item.state.amount);
        }
    }
}
