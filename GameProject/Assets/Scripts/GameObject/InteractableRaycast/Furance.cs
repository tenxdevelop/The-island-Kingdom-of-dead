using System;
using System.Collections;
using TheIslandKOD;
using UnityEngine;

public class Furance : InteractableRaycast
{
    public event Action<bool> OnChangeContents;

    [SerializeField] private GameObject m_particle;
    [SerializeField] private InventoryItemInfo m_metalInfo;
    [SerializeField] private InventoryItemInfo m_sulfurInfo;

    private InventoryWithSlots m_contentsFurance;
    private Coroutine m_coroutine;
    private bool m_isOpen = false;
    private bool m_isCanFire = false;
    private AudioSource m_audioSource;
    private Light m_light;
    private UIFurance m_uIFurance;

    public bool isFire = false;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_light = GetComponent<Light>();
        m_contentsFurance = new InventoryWithSlots(5);
        m_uIFurance = UIFurance.instance;
        m_contentsFurance.OnInventoryItemAddedEvent += AddedInventoryItems;
        m_contentsFurance.OnInventoryItemRemovedEvent += RemoveInventoryItems;
    }

    protected override void Interact()
    {
        if (!m_isOpen)
        {
            OpenCampFire();
        }
        else
        {
            CloseCampFire();
        }
    }

    private void OpenCampFire()
    {
        m_uIFurance.SetupContentCampFireUI(m_contentsFurance, this);
        m_uIFurance.SetVisible(true);
        m_isOpen = true;

    }

    public void CloseCampFire()
    {
        m_uIFurance.UnSetupContentCampFireUI();
        m_uIFurance.SetVisible(false);
        m_isOpen = false;

    }


    private void ChangeIntventoryItem()
    {
        var haveItemWood = m_contentsFurance.GetItemAmount(typeof(ItemWood));
        m_isCanFire = haveItemWood > 0;

        OnChangeContents?.Invoke(m_isCanFire);
    }


    private void AddedInventoryItems(object obj, IInventoryItem item, int amount)
    {
        ChangeIntventoryItem();
    }

    private void RemoveInventoryItems(object obj, Type item, int amount)
    {
        ChangeIntventoryItem();
    }

    public void Fire()
    {
        m_particle.SetActive(true);
        m_light.enabled = true;
        m_audioSource.Play();
        m_coroutine = StartCoroutine(FireRoutine());
    }

    public void StopFire()
    {
        m_particle.SetActive(false);
        m_light.enabled = false;
        m_audioSource.Stop();
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
        }
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            var haveItemWood = m_contentsFurance.GetItemAmount(typeof(ItemWood));
            if (haveItemWood <= 0)
            {
                StopFire();
            }
            FurancedOre();
            m_contentsFurance.Remove(this, typeof(ItemWood), 2);
            m_uIFurance.OnContentsCampFireStateChanged(this); 
        }
    }

    private void FurancedOre()
    {
        var haveItemMetal = m_contentsFurance.GetItemAmount(typeof(ItemMetalOre));
        if (haveItemMetal >= 3)
        {
            var item = new ItemMetal(m_metalInfo);
            item.state.amount = 1;
            if (m_contentsFurance.TryToAdd(this, item))
            {
                m_contentsFurance.Remove(this, typeof(ItemMetalOre), 3);
            }
            else
            {
                StopFire();
            }
        }

        var haveItemSulfur = m_contentsFurance.GetItemAmount(typeof(ItemSulfurOre));
        if (haveItemSulfur >= 2)
        {
            var item = new ItemSulfur(m_sulfurInfo);
            item.state.amount = 1;
            if (m_contentsFurance.TryToAdd(this, item))
            {
                m_contentsFurance.Remove(this, typeof(ItemSulfurOre), 2);
            }
            else
            {
                StopFire();
            }
        }

    }
}
