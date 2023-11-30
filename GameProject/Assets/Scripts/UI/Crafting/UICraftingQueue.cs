using System;
using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class UICraftingQueue : MonoBehaviour
{
    public static Action OnRemoveQueueEvent;
    public static UICraftingQueue instance { get; private set; }
    [SerializeField] private UICraftingQueueItem m_prefabItemQueue;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public void AddQueueItem(Sprite sprite, int time,int countCraft, int amount, IInventoryItemCraft infoCraft, List<IInventoryItem> removeItem)
    {
        UICraftingQueueItem currentQueueItem = Instantiate(m_prefabItemQueue, transform);
        currentQueueItem.LoadInfo(sprite, time, countCraft, amount, infoCraft, removeItem);
    }


}
