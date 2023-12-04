using TheIslandKOD;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventorySlot : UISlot
{

    [SerializeField] private UIInventoryItem m_uIInventoryItem;
    [SerializeField] private AudioClip m_clipMove;
    public IInventorySlot slot { get; private set; }


    private IUIInventory m_uIInventory;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_uIInventory = GetComponentInParent<IUIInventory>();
        m_audioSource = GetComponentInParent<AudioSource>();

    }
    public void SetSlot(IInventorySlot newSlot)
    {
        slot = newSlot;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        var inventory = m_uIInventory.inventory;
        m_audioSource.PlayOneShot(m_clipMove, 0.5f);
        var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>();
        var otherSlot = otherSlotUI.slot;
        if (otherSlotUI.m_uIInventory.inventory != inventory)
        {
            var item = otherSlot.item.Clone();
            item.state.amount = otherSlot.amount;
            if (inventory.TryToAdd(this, item))
            {
                otherSlotUI.m_uIInventory.inventory.Remove(this, otherSlot.itemType, otherSlot.amount);
            }    
                
        }
        else
        {
            inventory.TransitFromSlotToSlot(this, otherSlot, slot);
        }
        Refresh();
        otherSlotUI.Refresh();
    }

    public void Refresh()
    {

        if (slot != null)
        {
            m_uIInventoryItem.Refresh(slot);
        }

    }
}
