using UnityEngine;

namespace TheIslandKOD
{
    public interface IInventoryItemInfo
    {
        string id { get; }
        string title { get; }
        string description { get; }

        int maxItemsInInventorySlot { get; }

        Sprite spriteIcon { get; }
    }
}
