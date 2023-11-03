using TheIslandKOD;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropItem : MonoBehaviour, IDropHandler
{
    private UIInventory m_uIInventory;
    private Transform m_playerPosition;
    private ReferenceSystem m_referenceSystem;
    private Vector3 m_positionPrefab;
    [SerializeField] private Vector3 m_positionOffset;
    
    private void Awake()
    {
        m_uIInventory = UIInventory.instance;
        
    }
    private void Start()
    {
    
        m_referenceSystem = ReferenceSystem.instance;
        m_playerPosition = m_referenceSystem.player.GetComponent<PlayerLook>().itemDropPosition.transform;   
       
    }
    public void OnDrop(PointerEventData eventData)
    {
        var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>();
        var inventory = m_uIInventory.inventory;

        m_positionPrefab = m_playerPosition.position + m_positionOffset;
        if (otherItemUI.item.info.prefab != null)
        {
            var prefab = Instantiate(otherItemUI.item.info.prefab, m_positionPrefab, m_playerPosition.rotation);
            prefab.GetComponent<InteractableItemState>().state = otherItemUI.item.state.Clone();
            prefab.GetComponent<InteractableItemState>().item = otherItemUI.item.Clone();
        }

        inventory.Remove(this, otherItemUI.item.type, otherItemUI.item.state.amount);
        
        otherSlotUI.Refresh();
    }
}
