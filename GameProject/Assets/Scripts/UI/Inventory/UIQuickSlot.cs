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
        var slot = m_quickSlots[m_currentQuickslotID].GetComponent<UIInventorySlot>().slot;
        currentImageSlot.sprite = m_notSelectedSprite;
        if (m_currentQuickslotID == number)
        {
            if (!slot.isEmpty)
            {
                if (!m_currentSlotActive)
                {
                    currentImageSlot.sprite = m_selectedSprite;
                    slot.item.OnEnable();
                    slot.isQuickSlot = true;
                    m_lastItemUpdate = slot.item;
                }
                else
                {
                    currentImageSlot.sprite = m_notSelectedSprite;
                    slot.item.OnDisable();
                }
                m_currentSlotActive = !m_currentSlotActive;
            }
            else if(m_lastItemUpdate != null)
            {
                m_lastItemUpdate.OnDisable();
                m_currentSlotActive = false;
            }
        }
        else 
        {
            if (!slot.isEmpty)
            {
                currentImageSlot.sprite = m_notSelectedSprite;
                slot.item.OnDisable();
            }
            m_currentQuickslotID = number;
            slot = m_quickSlots[m_currentQuickslotID].GetComponent<UIInventorySlot>().slot;
            if (!slot.isEmpty)
            {
                m_quickSlots[m_currentQuickslotID].GetComponent<Image>().sprite = m_selectedSprite;
                slot.item.OnEnable();
                slot.isQuickSlot = true;
                m_lastItemUpdate = slot.item;
                m_currentSlotActive = true;
            }

        }
        
        var inventory = m_uIInventory.inventory;
        OnQuickSlotActiveChangedEvent?.Invoke(inventory, slot, m_currentSlotActive);
    }

    public void DisableQuickSlot()
    {
        m_lastItemUpdate.OnDisable();
        m_quickSlots[m_currentQuickslotID].GetComponent<Image>().sprite = m_notSelectedSprite;
        m_currentSlotActive = false;
    }
}
