using TheIslandKOD;
using UnityEngine;

public class Tree : InteractableAttachRaycast
{
    
    [SerializeField] private float health;
    [SerializeField] private InventoryItemInfo m_info;
    
    private UIQuickSlot m_quickSlot;
    private PlayerInventory m_playerInventory;
    private Player m_player;
    private void Start()
    {
        m_quickSlot = UIQuickSlot.instance;
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
        m_player = ReferenceSystem.instance.player.GetComponent<Player>();
    }
    protected override void Interact()
    {
        if (m_quickSlot.ActiveSlot.itemType == typeof(StoneAxe))
        {
            UpdateTree();
        }
    }
    protected override void OnEffects(Vector3 position, Quaternion rotation)
    {
        base.OnEffects(position, rotation);
    }

    private void UpdateTree()
    {
        var item = new Apple(m_info);
        item.state.amount = 2;
        m_playerInventory.inventory.TryToAdd(this, item);
        m_player.TakeDamage(7.5f);
        health -= 1;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
      
    }

}
