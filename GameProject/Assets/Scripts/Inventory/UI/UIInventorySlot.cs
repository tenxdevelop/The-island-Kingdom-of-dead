using System;
using TheIslandKOD;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventorySlot : UISlot
{

    [SerializeField] private UIInventoryItem m_uIInventoryItem;

    public IInventorySlot slot { get; private set; }

    private UIInventory m_uIInventory;

    private void Awake()
    {
        m_uIInventory = GetComponentInParent<UIInventory>();
    }
    public void SetSlot(IInventorySlot newSlot)
    {
        slot = newSlot;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>();
        var otherSlot = otherSlotUI.slot;
        var inventory = m_uIInventory.inventory;

        inventory.TransitFromSlotToSlot(this, otherSlot, slot);

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
