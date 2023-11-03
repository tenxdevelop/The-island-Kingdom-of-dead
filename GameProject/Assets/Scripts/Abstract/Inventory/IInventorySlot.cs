
using System;

namespace TheIslandKOD
{
    public interface IInventorySlot
    {
        bool isFull { get; }
        bool isEmpty { get; }
        bool isQuickSlot { get; set; }
        IInventoryItem item { get; }
        Type itemType { get; }
        int amount { get; }
        int capacity { get; }

        void SetItem(IInventoryItem item);
        void Clear();
    }
}
