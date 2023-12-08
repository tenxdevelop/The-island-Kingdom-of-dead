using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;
public class AnimalAttach : InteractableAttachRaycast
{
    [SerializeField] private float health;
    [SerializeField] private InventoryItemInfo m_info;
    [SerializeField] private List<AudioClip> m_clipAttacks;

    private AudioSource m_audioSource;
    private UIQuickSlot m_quickSlot;
    private PlayerInventory m_playerInventory;
    private bool m_isDestory = false;

    public void OnStart()
    {
        m_quickSlot = UIQuickSlot.instance;
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
        m_audioSource = GetComponent<AudioSource>();
    }

    protected override void Interact()
    {
        if (m_quickSlot.ActiveSlot.itemType == typeof(StoneAxe) && !m_isDestory)
        {
            UpdateAnimal();
        }
    }

    protected override void OnEffects(Vector3 position, Quaternion rotation)
    {
        if (!m_isDestory)
        {
            m_audioSource.pitch = Random.Range(0.9f, 1.1f);
            m_audioSource.clip = m_clipAttacks[Random.Range(0, m_clipAttacks.Count)];
            m_audioSource.Play();
            base.OnEffects(position, rotation);
        }
    }

    private void UpdateAnimal()
    {

        var item = new Apple(m_info);
        item.state.amount = 2;
        m_playerInventory.inventory.TryToAdd(this, item);
        health -= Random.Range(1, 4);

        if (health <= 0)
        {
            m_isDestory = true;
            Destroy(gameObject);
        }

    }

    
}
