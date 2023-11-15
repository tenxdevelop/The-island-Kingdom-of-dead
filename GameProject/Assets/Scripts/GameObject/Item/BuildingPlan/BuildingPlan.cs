using System;

namespace TheIslandKOD
{
    public class BuildingPlan : IInventoryItem
    {
        public IInventoryItemInfo info { get; }

        public IInventoryItemState state { get; }

        public Type type => GetType();

        private UIQuickSlot m_uIQuickSlot;

        private BuildingSystem m_buildingSystem;
        public BuildingPlan(IInventoryItemInfo info)
        {
            this.info = info;
            state = new InventoryItemState();
            m_uIQuickSlot = UIQuickSlot.instance;
            m_buildingSystem = new BuildingSystem();
        }

        public IInventoryItem Clone()
        {
            var clone = new BuildingPlan(info);
            clone.state.amount = state.amount;
            return clone;
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
                    m_buildingSystem.StartCoroutine();
                }
            }
            else
            {
                m_buildingSystem.StopCoroutine();
            }
        }
       
    }
}