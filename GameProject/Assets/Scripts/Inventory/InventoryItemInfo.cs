using UnityEngine;

namespace TheIslandKOD
{
    public enum ItemType
    {
        None,
        Default,
        Food,
        Weapon,
        Tools
    }
    [CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Game/Items/Create New ItemInfo")]
    public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
    {

        [SerializeField] private string m_id;
        [SerializeField] private string m_title;
        [SerializeField] private string m_desctription;
        [SerializeField] private int m_maxItemsInInventorySlot;
        [SerializeField] private Sprite m_sprite;
        [SerializeField] private GameObject m_prefab;
        [SerializeField] private ItemType m_itemType;

        public string id => m_id;

        public string title => m_title;

        public string description => m_desctription;

        public int maxItemsInInventorySlot => m_maxItemsInInventorySlot;

        public ItemType itemType => m_itemType;

        public Sprite spriteIcon => m_sprite;
        public GameObject prefab => m_prefab;
    }

}
