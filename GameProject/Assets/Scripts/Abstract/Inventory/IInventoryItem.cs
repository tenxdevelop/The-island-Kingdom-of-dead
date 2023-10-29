using System;

namespace TheIslandKOD
{
    public interface IInventoryItem 
    {

        IInventoryItemInfo info { get; }
        IInventoryItemState state { get; }
        Type type { get; }
        
        IInventoryItem Clone();
        void OnEnable();
        void OnDisable();
    }
}
