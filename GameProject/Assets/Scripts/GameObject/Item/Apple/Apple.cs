using System;
using UnityEngine;

namespace TheIslandKOD
{
    public class Apple : IInventoryItem
    {
        public IInventoryItemInfo info { get; }

        public IInventoryItemState state { get; }

        public Type type => GetType();

        private UIQuickSlot m_uIQuickSlot;
        public Apple(IInventoryItemInfo info)
        {
            this.info = info;
            state = new InventoryItemState();
            m_uIQuickSlot = UIQuickSlot.instance;
            
        }

        public void OnEnable()
        {
            m_uIQuickSlot.OnQuickSlotActiveChangedEvent += OnQuickSlotChangedEvent;
        }

        public void OnDisable()
        {
            m_uIQuickSlot.OnQuickSlotActiveChangedEvent -= OnQuickSlotChangedEvent;
        }

        private void OnQuickSlotChangedEvent(InventoryWithSlots inventory, IInventorySlot slot, bool isActive)
        {
            
            if (!slot.isEmpty)
            {
                if (slot.itemType == type)
                {
                    m_uIQuickSlot.DisableQuickSlot();
                    inventory.Remove(this, slot.itemType);
                }
            }
           
        }
        public IInventoryItem Clone()
        {
            var cloneApple = new Apple(info);
            cloneApple.state.amount = state.amount;
            return cloneApple;
        }
    }
}