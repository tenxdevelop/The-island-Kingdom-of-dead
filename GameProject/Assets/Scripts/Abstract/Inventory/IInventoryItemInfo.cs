using System.Collections.Generic;
using UnityEngine;

namespace TheIslandKOD
{
    public enum ItemType
    {
        None,
        Default,
        Build,
        Food,
        Weapon,
        Tools,
        Bow
    }

    public interface IInventoryItemInfo
    {
        string id { get; }
        string title { get; }
        string description { get; }

        int maxItemsInInventorySlot { get; }

        ItemType itemType { get; }

        Sprite spriteIcon { get; }
        GameObject DropPrefab { get; }

        List<BuildObjectType> buildObjects { get; }
    }
}
