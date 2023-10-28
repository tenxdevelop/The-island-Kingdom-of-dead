using TheIslandKOD;
using UnityEngine;

public class Health : InteractableRaycast
{
    [SerializeField] private InventoryItemInfo m_info;
    [SerializeField] private UIInventory m_uIInventory;
    
    protected override void Interact()
    {
        Debug.Log("apple Up");
        Destroy(gameObject);
        IInventoryItem apple = new Apple(m_info);
        apple.state.amount = 3;
        m_uIInventory.inventory.TryToAdd(this, apple);
    }
}
