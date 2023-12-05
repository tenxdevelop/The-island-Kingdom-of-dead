using System;
using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;
using UnityEngine.UI;

public class UICraftingQueueItem : MonoBehaviour
{
    [SerializeField] private Text m_timerText;
    [SerializeField] private Text m_amountText;
    [SerializeField] private Image m_image;

    private CraftingSystem m_craftingSystem;

    private CraftInfoCoroutine m_craftingInfoCoroutine;
    private PlayerInventory m_playerInventory;

    private List<IInventoryItem> m_removeItem;
    private IInventoryItemCraft m_infoCraft;

    private int m_countCraft;
    private bool m_isRemove = false;
    private void Awake()
    {
        m_craftingSystem = CraftingSystem.instance;
    }

    private void Start()
    {
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
    }

    public void LoadInfo(Sprite sprite, int time,int countCraft, int amount, IInventoryItemCraft infoCraft, List<IInventoryItem> removeItem)
    {
        transform.SetAsFirstSibling();
        m_timerText.text = time.ToString();
        m_amountText.text = amount.ToString();
        m_countCraft = countCraft;
        m_image.sprite = sprite;
        m_removeItem = removeItem;
        m_infoCraft = infoCraft;
        m_craftingInfoCoroutine = new CraftInfoCoroutine(time, m_timerText, Type.GetType("TheIslandKOD." + infoCraft.itemCraftType), amount, gameObject);
        m_craftingSystem.AddCraftingItem(m_craftingInfoCoroutine);
    }

    public void RemoveCraft()
    {
        if (!m_isRemove)
        {
            foreach (var itemComponent in m_infoCraft.craftComponents)
            {
                var item = m_removeItem.Find(i => i.type == Type.GetType("TheIslandKOD." + itemComponent.itemType));
                item.state.amount = itemComponent.amount * m_countCraft;
                m_playerInventory.inventory.TryToAdd(this, item);
            }
            m_craftingSystem.RemoveCratingItem(m_craftingInfoCoroutine);
            UICraftingQueue.OnRemoveQueueEvent?.Invoke();
            m_isRemove = true;
        }
    }

}
