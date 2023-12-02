using TheIslandKOD;
using UnityEngine;

public class StoneOre : InteractableAttachRaycast
{
    [SerializeField] private float health;
    [SerializeField] private InventoryItemInfo m_info;
    [SerializeField] private AudioClip m_clipAttack;
    [SerializeField] private AudioClip m_clipDestroy;

    private AudioSource m_audioSource;
    private UIQuickSlot m_quickSlot;
    private PlayerInventory m_playerInventory;
    private bool m_isDestory = false;


    private void Start()
    {
        m_quickSlot = UIQuickSlot.instance;
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
        m_audioSource = GetComponent<AudioSource>();

    }


    protected override void Interact()
    {
        if (m_quickSlot.ActiveSlot.itemType == typeof(StonePickAxe) && !m_isDestory)
        {
            UpdateOre();
        }
    }

    protected override void OnEffects(Vector3 position, Quaternion rotation)
    {
        if (!m_isDestory)
        {
            m_audioSource.pitch = Random.Range(0.9f, 1.1f);
            m_audioSource.clip = m_clipAttack;
            m_audioSource.Play();
            base.OnEffects(position, rotation);
        }
    }

    private void UpdateOre()
    {
        var item = new ItemStone(m_info);
        item.state.amount = 120;
        m_playerInventory.inventory.TryToAdd(this, item);
        health -= 1;

        if (health <= 0)
        {
            m_audioSource.pitch = 1;
            m_audioSource.clip = m_clipDestroy;
            m_audioSource.Play();
            m_isDestory = true;
            Destroy(gameObject);
        }

    }

}
