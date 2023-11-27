using System;
using UnityEngine;

namespace TheIslandKOD
{

    public enum CraftingCategory
    {
        Common,
        Tools,
        Weapon,
        Build,
        Resources
    }

    [Serializable]
    public class ItemCraftComponent
    {
        public int amount;
        public string itemType;
        public InventoryItemInfo info;
    }
}