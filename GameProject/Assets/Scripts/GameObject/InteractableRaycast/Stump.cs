using UnityEngine;
using TheIslandKOD;
public class Stump : InteractableRaycast
{
    [SerializeField] private InventoryItemInfo m_info;
    [SerializeField] private AudioClip m_clipPickUp;

    private PlayerInventory m_playerInventory;

    private void Start()
    {
        ReferenceSystem.OnFindedObjecs += OnInitPlayer;
    }

    private void OnInitPlayer()
    {
        ReferenceSystem.OnFindedObjecs -= OnInitPlayer;
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
    }

    protected override void Interact()
    {
        var item = new ItemWood(m_info);
        item.state.amount = 50;
        m_playerInventory.inventory.TryToAdd(this, item);

        SoundSystem.instance.backGroundSource.PlayOneShot(m_clipPickUp, 0.7f);
        Destroy(gameObject);
    }

}
