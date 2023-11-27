using System;
using System.Collections;
using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem instance { get; private set; }

    [SerializeField] private List<InventoryItemInfo> m_ItemInfos = new List<InventoryItemInfo>();

    private bool m_isNext = true;

    private Coroutine m_craftCoroutine;
    private int m_timer;
    private CraftInfoCoroutine m_lastRemoveCraftInfo;
    private CraftInfoCoroutine m_currentCraftInfo;
    private List<CraftInfoCoroutine> m_allCrafting;

    private PlayerInventory m_playerInventory;
    private List<IInventoryItem> m_inventoryItems = new List<IInventoryItem>();
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
        m_allCrafting = new List<CraftInfoCoroutine>();
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
        InitItemList();
    }

    private void Update()
    {
        if (m_isNext && m_allCrafting.Count > 0)
        {
            UpdateCraftList();
        }
    }

    public void AddCraftingItem(CraftInfoCoroutine info)
    {
        m_allCrafting.Add(info);
    }

    public void RemoveCratingItem(CraftInfoCoroutine info)
    {
        m_allCrafting.Remove(info);
        m_lastRemoveCraftInfo = info;

        if (!m_currentCraftInfo.Equals(info))
        {
            Destroy(info.queueObject);
        }
    }

    private void UpdateCraftList()
    {
        foreach (var craftInfo in m_allCrafting)
        {
            CreateCoroutine(craftInfo);
        }
    }

    public void CreateCoroutine(CraftInfoCoroutine info)
    {
        if (m_craftCoroutine != null)
            return;
        m_craftCoroutine = StartCoroutine(CraftingMethod(info));
    }

    private void RemoveCoroutine(CraftInfoCoroutine info)
    {
        if (m_craftCoroutine != null)
        {
            StopCoroutine(m_craftCoroutine);
        }
        m_craftCoroutine = null;
        Destroy(info.queueObject);
        RemoveCratingItem(info);
        m_isNext = true;
    }

    private IEnumerator CraftingMethod(CraftInfoCoroutine info)
    {
        m_timer = info.timeCraft;
        m_isNext = false;
        m_currentCraftInfo = info;
        while (true)
        {
            if (m_lastRemoveCraftInfo.Equals(info))
            {
                RemoveCoroutine(info);
            }
            yield return new WaitForSeconds(1);            
            m_timer--;
            info.updateText.text = m_timer.ToString();
            if (m_timer <= 0)
            {
                var item = m_inventoryItems.Find(i => i.type == info.itemCraft).Clone();
                item.state.amount = info.amount;
                m_playerInventory.inventory.TryToAdd(this, item);
                RemoveCoroutine(info);
            }
        }
    }

    private void InitItemList()
    {
        var itemInfo = m_ItemInfos.Find(i => i.id == "stoneAxe");
        var item = new StoneAxe(itemInfo);
        m_inventoryItems.Add(item);
        
    }
}
