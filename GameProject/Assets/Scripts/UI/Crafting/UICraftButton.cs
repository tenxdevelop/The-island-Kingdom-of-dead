using System;
using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;
using UnityEngine.UI;

public class UICraftButton : MonoBehaviour
{
    public static event Action OnCraftButtonEvent;
    public static UICraftButton instance { get; private set; }

    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioClipCraft;

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
        ReferenceSystem.OnFindedObjecs += OnInitPlayer;
        m_removeItem = new List<IInventoryItem>();
        m_craftButton = GetComponent<Button>();
    }

    private void OnInitPlayer()
    {
        ReferenceSystem.OnFindedObjecs -= OnInitPlayer;
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
    }

    public void UpdateButton(IInventoryItemCraft infoCraft, int countCraft)
    {
        m_canCraft = true;
        m_lastInfoCraft = infoCraft;
        foreach (var itemComponent in infoCraft.craftComponents)
        {
            var haveItemAmount = m_playerInventory.inventory.GetItemAmount(Type.GetType("TheIslandKOD." + itemComponent.itemType));
            m_canCraft = haveItemAmount >= itemComponent.amount * countCraft && m_canCraft;
        }
        m_craftButton.interactable = m_canCraft;
    }

    public void Craft()
    {
        m_audioSource.PlayOneShot(m_audioClipCraft, 0.7f);
        var countCraft = UIInputFeildCraft.instance.countCraft;
        foreach (var itemComponent in m_lastInfoCraft.craftComponents)
        {
            m_removeItem.Add(m_playerInventory.inventory.GetItem(Type.GetType("TheIslandKOD." + itemComponent.itemType)).Clone());
            m_playerInventory.inventory.Remove(this, Type.GetType("TheIslandKOD." + itemComponent.itemType), itemComponent.amount * countCraft);
        }
        UICraftingQueue.instance.AddQueueItem(m_lastInfoCraft.info.spriteIcon, m_lastInfoCraft.timeCraft * countCraft, countCraft, countCraft * m_lastInfoCraft.amountCraft, m_lastInfoCraft, m_removeItem);
        OnCraftButtonEvent?.Invoke();
    }
}
