using System;
using System.Collections.Generic;
using System.Linq;

namespace TheIslandKOD
{
    public class InventoryWithSlots : IInventory
    {

        public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;
        public event Action<object, Type, int> OnInventoryItemRemovedEvent;
        public event Action<object> OnInventoryStateChangedEvent;
        public int capacity { get; set; }

        public bool isFull => m_slots.All(slot => slot.isFull);

        private List<IInventorySlot> m_slots;

        public InventoryWithSlots(int capacity)
        {
            this.capacity = capacity;

            m_slots = new List<IInventorySlot>(capacity);
            for(int i = 0; i < capacity; i++)
                m_slots.Add(new InventorySlot());
        }

        public IInventoryItem[] GetAllItems()
        {
            var allItem = new List<IInventoryItem>();
            foreach (var slot in m_slots.FindAll(slot => !slot.isEmpty))
            {
                allItem.Add(slot.item);
            }
            return allItem.ToArray();
        }

        public IInventoryItem[] GetAllItems(Type itemType)
        {
            var allItem = new List<IInventoryItem>();
            foreach (var slot in m_slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType))
            {
                allItem.Add(slot.item);
            }
            return allItem.ToArray();
        }

        public IInventoryItem[] GetEquippedItems()
        {
            var equippedItems = new List<IInventoryItem>();
            foreach (var slot in m_slots.FindAll(slot => !slot.isEmpty && slot.item.state.isEquipped))
            {
                equippedItems.Add(slot.item);
            }
            return equippedItems.ToArray();
        }

        public IInventoryItem GetItem(Type itemType)
        {
            return m_slots.Find(slot => slot.itemType == itemType).item;
        }

        public int GetItemAmount(Type itemType)
        {
            int allAmount = 0;
            foreach (var slot in m_slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType))
            {
                allAmount += slot.amount;
            }
            return allAmount;
        }
        public IInventorySlot[] GetAllSlots(Type itemType)
        {
            return m_slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType).ToArray();
        }

        public IInventorySlot[] GetAllSlots()
        {
            return m_slots.ToArray();
        }
        public bool HasItem(Type itemType, out IInventoryItem item)
        {
            item = GetItem(itemType);
            return item != null;
        }

        public void Remove(object sender, Type itemType, int amount = 1)
        {
            var slotWithItemType = GetAllSlots(itemType);
            if (slotWithItemType.Length == 0)
                return;
            var amountToRemove = amount;

            for (int i = slotWithItemType.Length - 1; i >= 0; i--)
            {
                var slot = slotWithItemType[i];
                if (slot.amount >= amountToRemove)
                {
                    slot.item.state.amount -= amountToRemove;
                    if (slot.amount <= 0)
                    {
                        ClearSlot(slot);
                    }
                    OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountToRemove);
                    OnInventoryStateChangedEvent?.Invoke(sender);
                    break;
                }
                int amountRemoved = slot.amount;
                amountToRemove -= slot.amount;
                ClearSlot(slot);
                OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountRemoved);
                OnInventoryStateChangedEvent?.Invoke(sender);
            }
        }

        public bool TryToAdd(object sender, IInventoryItem item)
        {
            var slotWithSameItemButNotEmpty = m_slots.Find(slot => !slot.isEmpty && !slot.isFull &&
                                                           slot.itemType == item.type);
            if (slotWithSameItemButNotEmpty != null)
            {
                return TryToAddToSlot(sender, slotWithSameItemButNotEmpty, item);
            }
            var emptySlot = m_slots.Find(slot => slot.isEmpty);
            if (emptySlot != null)
            {
                return TryToAddToSlot(sender, emptySlot, item);
            }

            return false;
        }

        public void TransitFromSlotToSlot(object sender, IInventorySlot fromSlot, IInventorySlot toSlot)
        {
            if (fromSlot.isEmpty)
                return;

            if (fromSlot == toSlot)
                return;

            if (toSlot.isFull)
            {
                SwapSlotInventory(fromSlot, toSlot);
                OnInventoryStateChangedEvent?.Invoke(sender);
                return;
            }


            if (!toSlot.isEmpty && fromSlot.itemType != toSlot.itemType)
            {
                SwapSlotInventory(fromSlot, toSlot);
                OnInventoryStateChangedEvent?.Invoke(sender);
                return;
            }
            
            int slotCapacity = fromSlot.capacity;
            bool fits = fromSlot.amount + toSlot.amount <= slotCapacity;
            int amountToAdd = (fits) ? fromSlot.amount : slotCapacity - toSlot.amount;
            int amountLeft = fromSlot.amount - amountToAdd;

            if (toSlot.isEmpty)
            {
                toSlot.SetItem(fromSlot.item);
                ClearSlot(fromSlot);
                OnInventoryStateChangedEvent?.Invoke(sender);
            }

            toSlot.item.state.amount += amountToAdd;
            if (fits)
            {
                ClearSlot(fromSlot);
            }
            else
            {
                fromSlot.item.state.amount = amountLeft;
            }
            OnInventoryStateChangedEvent?.Invoke(sender);
        }


        public bool TryToAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
        {
            bool fits = slot.amount + item.state.amount <= item.info.maxItemsInInventorySlot;
            int amountToAdd = fits ? item.state.amount : item.info.maxItemsInInventorySlot - slot.amount;
            int amountLeft = fits ? 0 : item.state.amount - amountToAdd;
            var itemClone = item.Clone();
            itemClone.state.amount = amountToAdd;
            
            if (slot.isEmpty)
            {
                slot.SetItem(itemClone);
                
            }
            else
            {
                slot.item.state.amount += amountToAdd;           
            }
            OnInventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);
            OnInventoryStateChangedEvent?.Invoke(sender);

            if (amountLeft <= 0)
            {
                return true;
            }
            item.state.amount = amountLeft;           
            return TryToAdd(sender, item);
        }

        private void SwapSlotInventory(IInventorySlot fromSlot, IInventorySlot toSlot)
        {
            
            IInventoryItem fromItem = toSlot.item.Clone();
            IInventoryItem toItem = fromSlot.item.Clone();
            ClearSlot(toSlot);
            toSlot.SetItem(toItem);
            ClearSlot(fromSlot);
            fromSlot.SetItem(fromItem);
        }

        private void ClearSlot(IInventorySlot slot)
        {
            if (slot.isQuickSlot)
            {
                UIQuickSlot.instance.DisableQuickSlot();
            }
            slot.Clear();
        }
    }
}
