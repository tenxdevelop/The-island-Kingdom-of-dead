using System;
using System.Collections;
using TheIslandKOD;
using UnityEngine;

public class CampFire : InteractableRaycast
{

    public event Action<bool> OnChangeContents;

    [SerializeField] private GameObject m_particle;

    private InventoryWithSlots m_contentsCampFire;
    private Coroutine m_coroutine;
    private bool m_isOpen = false;
    private bool m_isCanFire = false;
    private AudioSource m_audioSource;
    private Light m_light;
    private UICampFire m_uICampFire;

    public bool isFire = false;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_light = GetComponent<Light>();
        m_contentsCampFire = new InventoryWithSlots(5);
        m_uICampFire = UICampFire.instance;
        m_contentsCampFire.OnInventoryItemAddedEvent += AddedInventoryItems;
        m_contentsCampFire.OnInventoryItemRemovedEvent += RemoveInventoryItems;
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
        m_uICampFire.SetupContentCampFireUI(m_contentsCampFire, this);
        m_uICampFire.SetVisible(true);
        m_isOpen = true;
        
    }

    public void CloseCampFire()
    {
        m_uICampFire.UnSetupContentCampFireUI();
        m_uICampFire.SetVisible(false);
        m_isOpen = false;
        
    }


    private void ChangeIntventoryItem()
    {
        var haveItemWood = m_contentsCampFire.GetItemAmount(typeof(ItemWood));
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
            var haveItemWood = m_contentsCampFire.GetItemAmount(typeof(ItemWood));
            if (haveItemWood <= 0)
            {
                StopFire();
            }
            m_contentsCampFire.Remove(this, typeof(ItemWood), 2);
            m_uICampFire.OnContentsCampFireStateChanged(this);
            yield return new WaitForSeconds(4f);
        }
    }
}
