using System;
using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;
using UnityEngine.UI;

public class UICraftButton : MonoBehaviour
{
    public static Action OnCraftButtonEvent;
    public static UICraftButton instance { get; private set; }

    private Button m_craftButton;
    private bool m_canCraft = true;
    private IInventoryItemCraft m_lastInfoCraft;
    private PlayerInventory m_playerInventory;
    private List<IInventoryItem> m_removeItem;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }

    private void Start()
    {
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
        m_removeItem = new List<IInventoryItem>();
        m_craftButton = GetComponent<Button>();
    }

    public void UpdateButton(IInventoryItemCraft infoCraft)
    {
        m_canCraft = true;
        m_lastInfoCraft = infoCraft;
        foreach (var itemComponent in infoCraft.craftComponents)
        {
            var haveItemAmount = m_playerInventory.inventory.GetItemAmount(Type.GetType("TheIslandKOD." + itemComponent.itemType));
            m_canCraft = haveItemAmount >= itemComponent.amount && m_canCraft;
        }
        m_craftButton.interactable = m_canCraft;
    }

    public void Craft()
    {
        
        foreach (var itemComponent in m_lastInfoCraft.craftComponents)
        {
            m_removeItem.Add(m_playerInventory.inventory.GetItem(Type.GetType("TheIslandKOD." + itemComponent.itemType)).Clone());
            m_playerInventory.inventory.Remove(this, Type.GetType("TheIslandKOD." + itemComponent.itemType), itemComponent.amount);
        }
        UICraftingQueue.instance.AddQueueItem(m_lastInfoCraft.info.spriteIcon, 10, 1, m_lastInfoCraft, m_removeItem);
        OnCraftButtonEvent?.Invoke();
    }
}
