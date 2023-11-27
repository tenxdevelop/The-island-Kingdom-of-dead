using System.Collections.Generic;
using UnityEngine;

namespace TheIslandKOD
{
    [CreateAssetMenu(fileName = "InventoryItemCraft", menuName = "Game/Items/Create New ItemCraft")]
    public class InventoryItemCraft : ScriptableObject, IInventoryItemCraft
    {
        [SerializeField] private InventoryItemInfo m_info;
        [SerializeField] private int m_amountCraft;
        [SerializeField] private string m_itemCraftType;
        [SerializeField] private CraftingCategory m_category;
        [SerializeField] private int m_timeCraft = 5;
        [SerializeField] private List<ItemCraftComponent> m_craftComponents;
        public IInventoryItemInfo info => m_info;
        public int amountCraft => m_amountCraft;
        public string itemCraftType => m_itemCraftType;
        public int timeCraft => m_timeCraft;
        public CraftingCategory category => m_category;
        public List<ItemCraftComponent> craftComponents => m_craftComponents;
    }
}