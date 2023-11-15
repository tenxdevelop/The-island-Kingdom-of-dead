using System;
using System.Collections;
using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickSlot : MonoBehaviour
{
    public static UIQuickSlot instance;

    public event Action<InventoryWithSlots, IInventorySlot, bool> OnQuickSlotActiveChangedEvent;

    [SerializeField] private List<GameObject> m_quickSlots = new List<GameObject>(); 
    [SerializeField] private Sprite m_notSelectedSprite;
    [SerializeField] private Sprite m_selectedSprite;
    private int m_currentQuickslotID = 0;
    private bool m_currentSlotActive = false;

    private UIInventory m_uIInventory;
    private IInventoryItem m_lastItemUpdate;

    private InventoryWithSlots m_lastInventory;
    private IInventorySlot m_lastSlot;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;

        m_uIInventory = GetComponentInParent<UIInventory>();
    }
    public void QuickSlotInputAction(int number)
    {
        var currentImageSlot = m_quickSlots[m_currentQuickslotID].GetComponent<Image>();
        m_lastSlot = m_quickSlots[m_currentQuickslotID].GetComponent<UIInventorySlot>().slot;
        m_lastInventory = m_uIInventory.inventory;
        currentImageSlot.sprite = m_notSelectedSprite;
        if (m_currentQuickslotID == number)
        {
            if (!m_lastSlot.isEmpty)
            {
                if (!m_currentSlotActive)
                {
                    currentImageSlot.sprite = m_selectedSprite;
                    m_lastSlot.item.OnEnable();
                    m_currentSlotActive = !m_currentSlotActive;
                    m_lastSlot.isQuickSlot = true;
                    m_lastItemUpdate = m_lastSlot.item;
                }
                else
                {
                    currentImageSlot.sprite = m_notSelectedSprite;
                    DisableItem(m_lastInventory, m_lastSlot);
                }
                
            }
            else if(m_lastItemUpdate != null)
            {
                DisableItem(m_lastInventory, m_lastSlot);
            }
        }
        else 
        {
            if (!m_lastSlot.isEmpty)
            {
                currentImageSlot.sprite = m_notSelectedSprite;
                DisableItem(m_lastInventory, m_lastSlot);
            }
            m_currentQuickslotID = number;
            m_lastSlot = m_quickSlots[m_currentQuickslotID].GetComponent<UIInventorySlot>().slot;
            if (!m_lastSlot.isEmpty)
            {
                m_quickSlots[m_currentQuickslotID].GetComponent<Image>().sprite = m_selectedSprite;
                m_lastSlot.item.OnEnable();
                m_lastSlot.isQuickSlot = true;
                m_lastItemUpdate = m_lastSlot.item;
                m_currentSlotActive = true;
            }

        }
        
        OnQuickSlotActiveChangedEvent?.Invoke(m_lastInventory, m_lastSlot, m_currentSlotActive);
    }

    public void DisableQuickSlot()
    {
        if (m_lastSlot != null && m_lastInventory != null)
        {
            DisableItem(m_lastInventory, m_lastSlot);
            m_quickSlots[m_currentQuickslotID].GetComponent<Image>().sprite = m_notSelectedSprite;       
        }
    }

    private void DisableItem(InventoryWithSlots inventory, IInventorySlot slot)
    {
        if (m_lastItemUpdate != null)
        {
            m_currentSlotActive = false;
            OnQuickSlotActiveChangedEvent?.Invoke(inventory, slot, m_currentSlotActive);
            m_lastItemUpdate.OnDisable();
        }
    }
}
