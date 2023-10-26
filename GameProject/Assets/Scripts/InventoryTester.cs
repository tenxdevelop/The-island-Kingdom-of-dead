using System.Collections.Generic;
using UnityEngine;

namespace TheIslandKOD
{
    public class InventoryTester
    {

        private InventoryItemInfo m_appleInfo;
        private InventoryItemInfo m_pepperInfo;
        private UIInventorySlot[] m_uISlots;

        public InventoryWithSlots inventory { get; }

        public InventoryTester(InventoryItemInfo appleInfo, InventoryItemInfo pepperInfo, UIInventorySlot[] uISlots)
        {
            m_appleInfo = appleInfo;
            m_pepperInfo = pepperInfo;
            m_uISlots = uISlots;

            inventory = new InventoryWithSlots(28);
            inventory.OnInventoryStateChangedEvent += OnInventoryStateChanged;
        }

        public void FillSlots()
        {
            var allSlots = inventory.GetAllSlots();
            var avaibleSlots = new List<IInventorySlot>(allSlots);

            var filledSlots = 3;

            for (int i = 0; i < filledSlots; i++)
            {
                var filledSlot = AddRandomAppleIntoRandomSlots(avaibleSlots);
                avaibleSlots.Remove(filledSlot);

                filledSlot = AddRandomPepperIntoRandomSlots(avaibleSlots);
                avaibleSlots.Remove(filledSlot);

            }

            SetupInventoryUI(inventory);
        }

        private void SetupInventoryUI(InventoryWithSlots inventory)
        {
            var allSlots = inventory.GetAllSlots();
            var allSlotsCount = allSlots.Length;
            for (int i = 0; i < allSlotsCount; i++)
            {
                var slot = allSlots[i]; 
                var uISlot = m_uISlots[i];
                uISlot.SetSlot(slot);
                uISlot.Refresh();
            }
        }

        private IInventorySlot AddRandomAppleIntoRandomSlots(List<IInventorySlot> slots)
        {
            var rSlotIndex  = Random.Range(0, slots.Count);
            var rSlot =slots[rSlotIndex];
            var rCount = Random.Range(1, 8);
            var apple = new Apple(m_appleInfo);
            apple.state.amount = rCount;
            inventory.TryToAddToSlot(this, rSlot, apple);
            return rSlot;
        }

        private IInventorySlot AddRandomPepperIntoRandomSlots(List<IInventorySlot> slots)
        {
            var rSlotIndex = Random.Range(0, slots.Count);
            var rSlot = slots[rSlotIndex];
            var rCount = Random.Range(1, 4);
            var pepper = new Pepper(m_pepperInfo);
            pepper.state.amount = rCount;
            inventory.TryToAddToSlot(this, rSlot, pepper);
            return rSlot;
        }


        private void OnInventoryStateChanged(object obj)
        {
            foreach (var uISlot in m_uISlots)
            {
                uISlot.Refresh();
            }
        }
    }
    
}
