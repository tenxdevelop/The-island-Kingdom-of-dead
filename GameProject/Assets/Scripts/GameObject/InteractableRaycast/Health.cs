using TheIslandKOD;
using UnityEngine;

public class Health : InteractableRaycast
{
    [SerializeField] private InventoryItemInfo m_info;
    [SerializeField] private InventoryItemInfo m_info2;
    
    private PlayerInventory m_playerInventory;
    private ReferenceSystem m_referenceSystem;

    private void Start()
    {
     
        m_referenceSystem = ReferenceSystem.instance;
            
        m_playerInventory = m_referenceSystem.player.GetComponent<PlayerInventory>();     
        
    }
    protected override void Interact()
    {
        var item = new BuildingPlan(m_info);
        var item2 = new Apple(m_info2);
        item.state.amount = 1;
        item2.state.amount = 3;
        m_playerInventory.inventory.TryToAdd(this, item);
        m_playerInventory.inventory.TryToAdd(this, item2);
        Destroy(gameObject);
    }

    
}
