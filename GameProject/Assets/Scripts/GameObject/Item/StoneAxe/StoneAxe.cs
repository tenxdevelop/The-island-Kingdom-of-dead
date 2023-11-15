using System;
using System.Linq;
using UnityEngine;

namespace TheIslandKOD
{
    public class StoneAxe : IInventoryItem
    {
        private const string TAG_ITEM_ARM = "StoneAxe";
        private UIQuickSlot m_uIQuickSlot;
        private PlayerMovement m_playerMovement;
        private PlayerItemArm m_playerItemArm;
        public IInventoryItemInfo info { get; }

        public IInventoryItemState state { get; }

        public Type type => GetType();

        public StoneAxe(IInventoryItemInfo info)
        {
            this.info = info;
            state = new InventoryItemState();
            m_uIQuickSlot = UIQuickSlot.instance;
            m_playerMovement = ReferenceSystem.instance.player.GetComponent<PlayerMovement>();
            m_playerItemArm = ReferenceSystem.instance.player.GetComponent<PlayerItemArm>();
        }
        public IInventoryItem Clone()
        {
            var cloneStoneAxe = new StoneAxe(info);
            cloneStoneAxe.state.amount = state.amount;
            return cloneStoneAxe;

        }

        public void OnDisable()
        {    
            m_uIQuickSlot.OnQuickSlotActiveChangedEvent -= OnQuickSlotChangedEvent;
        }

        public void OnEnable()
        {
            m_uIQuickSlot.OnQuickSlotActiveChangedEvent += OnQuickSlotChangedEvent;
        }

        private void OnQuickSlotChangedEvent(InventoryWithSlots inventory, IInventorySlot slot, bool isActive)
        {

            if (isActive)
            {
                if (slot.itemType == type)
                {
                    m_playerMovement.SetItemState(true);
                    m_playerItemArm.ItemArm.Find(i => i.key == TAG_ITEM_ARM).item.SetActive(true);
                }

            }
            else
            {
                m_playerMovement.SetItemState(false);
                m_playerItemArm.ItemArm.Find(i => i.key == TAG_ITEM_ARM).item.SetActive(false);
            }
           
        }

    }
}
