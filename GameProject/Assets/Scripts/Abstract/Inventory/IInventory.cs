
using System;

namespace TheIslandKOD
{

    public interface IInventory
    {
        int capacity { get; set; }
        bool isFull { get; }

        IInventoryItem GetItem(Type itemType);
        IInventoryItem[] GetAllItems();
        IInventoryItem[] GetAllItems(Type itemType);
        IInventoryItem[] GetEquippedItems();
        
        int GetItemAmount(Type itemType);

        bool Add(object sender, IInventoryItem item);
        void Remove(object sender, Type itemType, int amount = 1);
        bool HasItem(Type itemType, out IInventoryItem item);

    }
}
