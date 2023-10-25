using System;
using System.Collections.Generic;
using System.Linq;

namespace TheIslandKOD
{
    public class InventoryWithSlots : IInventory
    {

        public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;
        public event Action<object, Type, int> OnInventoryItemRemovedEvent;

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
            foreach (var slot in m_slots.FindAll(slot => !slot.isEmpty && slot.item.isEquipped))
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
                    slot.item.amount -= amountToRemove;
                    if (slot.amount <= 0)
                    {
                        slot.Clear();
                    }
                    OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountToRemove);
                    break;
                }
                int amountRemoved = slot.amount;
                amountToRemove -= slot.amount;
                slot.Clear();
                OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountRemoved);
            }
        }

        public bool TryToAdd(object sender, IInventoryItem item)
        {
            var slotWithSameItemButNotEmpty = m_slots.Find(slot => !slot.isEmpty && !slot.isFull &&
                                                           slot.itemType == item.type);
            if (slotWithSameItemButNotEmpty != null)
            {
                return AddToSlot(sender, slotWithSameItemButNotEmpty, item);
            }
            var emptySlot = m_slots.Find(slot => slot.isEmpty);
            if (emptySlot != null)
            {
                return AddToSlot(sender, emptySlot, item);
            }

            return false;
        }
        private bool AddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
        {
            bool fits = slot.amount + item.amount <= item.maxItemsInInventorySlot;
            int amountToAdd = fits ? item.amount : item.maxItemsInInventorySlot - slot.amount;
            int amountLeft = slot.amount - amountToAdd;
            var itemClone = item.Clone();
            itemClone.amount = amountToAdd;

            if (slot.isEmpty)
            {
                slot.SetItem(itemClone);              
            }
            else
            {
                slot.item.amount += amountToAdd;           
            }
            OnInventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);

            if (amountLeft <= 0)
            {
                return true;
            }
            item.amount = amountLeft;           
            return TryToAdd(sender, item);
        }

        
    }
}
