using TheIslandKOD;
using UnityEngine;

public class Health : InteractableRaycast
{
    [SerializeField] private InventoryItemInfo m_info;
    public IInventoryItemState state;
    private PlayerInventory m_playerInventory;
    private ReferenceSystem m_referenceSystem;

    private void Start()
    {
        state = GetComponent<InteractableItemState>().state;
        m_referenceSystem = ReferenceSystem.instance;
        m_playerInventory = m_referenceSystem.player.GetComponent<PlayerInventory>();
    }
    protected override void Interact()
    {
        var apple = new Apple(m_info);
        apple.state.amount = state.amount;
        m_playerInventory.inventory.TryToAdd(this, apple);
        Destroy(gameObject);
    }

    
}
