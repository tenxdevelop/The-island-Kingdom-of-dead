using TheIslandKOD;
using UnityEngine;

public class InteractableItem : InteractableRaycast
{
    private IInventoryItemState m_state;
    private IInventoryItem m_item;

    private InteractableItemState m_itemState;
    private PlayerInventory m_playerInventory;
    private ReferenceSystem m_referenceSystem;
    private Vector3 m_rotation = new Vector3(-90, 0, 0);
    private void Start()
    {
        m_itemState = GetComponent<InteractableItemState>();

        m_item = m_itemState.item;
        m_state = m_itemState.state;
        m_referenceSystem = ReferenceSystem.instance;
        m_playerInventory = m_referenceSystem.player.GetComponent<PlayerInventory>();

        transform.localRotation = Quaternion.Euler(m_rotation.x, m_rotation.y, m_rotation.z);

        gameObject.name = $"item {m_item.info.title}";
        promptMessage = m_item.info.title + " pick up";

    }

    protected override void Interact()
    {
        var item = m_item.Clone();
        Debug.Log(m_state.amount);
        item.state.amount = m_state.amount;
        m_playerInventory.inventory.TryToAdd(this, item);
        Destroy(gameObject);
    }

}
