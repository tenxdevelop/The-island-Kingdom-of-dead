using TheIslandKOD;
using UnityEngine;

public class Health : InteractableRaycast
{
    [SerializeField] private InventoryItemInfo m_info;
    [SerializeField] private InventoryItemInfo m_info2;
    [SerializeField] private InventoryItemInfo m_info3;
    
    private PlayerInventory m_playerInventory;
    private ReferenceSystem m_referenceSystem;
    private Player m_player;
    private void Start()
    {
     
        m_referenceSystem = ReferenceSystem.instance;
            
        m_playerInventory = m_referenceSystem.player.GetComponent<PlayerInventory>();
        m_player = m_referenceSystem.player.GetComponent<Player>();
    }
    protected override void Interact()
    {
        var item = new StoneAxe(m_info);
        var item2 = new ItemBow(m_info2);
        var item3 = new ItemStorage(m_info3);
        item.state.amount = 1;
        item2.state.amount = 1;
        item3.state.amount = 1;
        m_playerInventory.inventory.TryToAdd(this, item);
        m_playerInventory.inventory.TryToAdd(this, item2);
        m_playerInventory.inventory.TryToAdd(this, item3);
        m_player.TakeDamage(20);
        Destroy(gameObject);
    }

    
}
