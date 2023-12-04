using TheIslandKOD;
using UnityEngine;

public class StoneOre : InteractableAttachRaycast
{
    [SerializeField] private float health;
    [SerializeField] private InventoryItemInfo m_info;
    [SerializeField] private AudioClip m_clipAttack;
    [SerializeField] private AudioClip m_clipDestroy;
    [SerializeField] private OreType m_typeOre;

    private AudioSource m_audioSource;
    private UIQuickSlot m_quickSlot;
    private PlayerInventory m_playerInventory;
    private bool m_isDestory = false;


    private void Start()
    {
        m_quickSlot = UIQuickSlot.instance;
        ReferenceSystem.OnFindedObjecs += OnInitPlayer;
        m_audioSource = GetComponent<AudioSource>();

    }

    private void OnInitPlayer()
    {
        ReferenceSystem.OnFindedObjecs -= OnInitPlayer;
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
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

        var item = GetItemType();

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


    private IInventoryItem GetItemType()
    {
        IInventoryItem item = null;

        switch (m_typeOre)
        {
            case OreType.Metal:
                item = new ItemMetalOre(m_info);
                item.state.amount = 70;
                break;

            case OreType.Stone:
                item = new ItemStone(m_info);
                item.state.amount = 120;
                break;

            case OreType.Sulfure:
                item = new ItemSulfurOre(m_info);
                item.state.amount = 50;
                break;

        }

        return item;
    }

}


namespace TheIslandKOD
{

    [System.Serializable]
    public enum OreType
    {
        None = 0,
        Stone,
        Metal,
        Sulfure
    }

}