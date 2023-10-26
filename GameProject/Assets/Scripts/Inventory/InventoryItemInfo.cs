using UnityEngine;

namespace TheIslandKOD
{

    [CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Game/Items/Create New ItemInfo")]
    public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
    {

        [SerializeField] private string m_id;
        [SerializeField] private string m_title;
        [SerializeField] private string m_desctription;
        [SerializeField] private int m_maxItemsInInventorySlot;
        [SerializeField] private Sprite m_sprite;


        public string id => m_id;

        public string title => m_title;

        public string description => m_desctription;

        public int maxItemsInInventorySlot => m_maxItemsInInventorySlot;

        public Sprite spriteIcon => m_sprite;
    }

}
