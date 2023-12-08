using System;
using UnityEngine;

namespace TheIslandKOD
{
    public class Apple : IInventoryItem
    {
        private UIQuickSlot m_uIQuickSlot;
        private PlayerMovement m_playerMovement;
        public IInventoryItemInfo info { get; }

        public IInventoryItemState state { get; }

        public Type type => GetType();
        
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
            if (isActive)
            {
                if (!slot.isEmpty)
                {
                    if (slot.itemType == type)
                    {
                        m_uIQuickSlot.DisableQuickSlot();
                        inventory.Remove(this, slot.itemType);
                        ReferenceSystem.instance.player.GetComponent<Player>().HungerUp(30);
                    }
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