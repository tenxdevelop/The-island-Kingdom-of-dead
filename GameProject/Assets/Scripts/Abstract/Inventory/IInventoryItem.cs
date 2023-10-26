using System;

namespace TheIslandKOD
{
    public interface IInventoryItem 
    {

        IInventoryItemInfo info { get; }
        IInfentoryItemState state { get; }
        Type type { get; }
        
        IInventoryItem Clone();

    }
}
