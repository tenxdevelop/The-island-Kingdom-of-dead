using System.Collections.Generic;

namespace TheIslandKOD
{
    public interface IInventoryItemCraft
    {
        IInventoryItemInfo info { get; }
        int amountCraft { get; }
        string itemCraftType { get; }
        CraftingCategory category { get; }
        List<ItemCraftComponent> craftComponents { get; }
    }
}
