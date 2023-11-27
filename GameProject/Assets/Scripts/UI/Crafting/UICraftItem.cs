using System;
using TheIslandKOD;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class UICraftItem : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private UIInfoCraftResources m_prefab;
    private Image m_iconImage;
    private Text m_textItemName;
    private Text m_textDescription;

    private IInventoryItemCraft m_infoCraft;

    private Transform m_parentPrefab;

    private PlayerInventory m_playerInventory;
    private void Awake()
    {
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        UICraftButton.OnCraftButtonEvent += UpdateInfo;
        UICraftingQueue.OnRemoveQueueEvent += UpdateInfo;
    }

    private void OnDestroy()
    {
        UICraftButton.OnCraftButtonEvent -= UpdateInfo;
        UICraftingQueue.OnRemoveQueueEvent -= UpdateInfo;
    }
    public void LoadUIItemCraft(IInventoryItemCraft infoCraft, Image iconImage, Text name, Text description, Transform parentPrefab)
    {  

        m_image.sprite = infoCraft.info.spriteIcon;
        m_infoCraft = infoCraft;
        m_iconImage = iconImage;
        m_textItemName = name;
        m_textDescription = description;
        m_parentPrefab = parentPrefab;
    }

    public void ShowInfoItemCraft()
    {
        m_iconImage.sprite = m_infoCraft.info.spriteIcon;
        m_textItemName.text = m_infoCraft.info.title;
        m_textDescription.text = m_infoCraft.info.description;

    }

    public void ShowInfoComponent()
    {
        UICraftButton.instance.UpdateButton(m_infoCraft);
        for (int i = 0; i < m_parentPrefab.childCount; i++)
        {
            Destroy(m_parentPrefab.GetChild(i).gameObject);
        }

        foreach (var itemComponent in m_infoCraft.craftComponents)
        {
            var haveItemAmount = m_playerInventory.inventory.GetItemAmount(Type.GetType("TheIslandKOD." + itemComponent.itemType));
            UIInfoCraftResources currentInfo = Instantiate(m_prefab, m_parentPrefab);
            currentInfo.UpdateLoadInfo(itemComponent.amount.ToString(), itemComponent.itemType, "0", haveItemAmount.ToString());
        }
    }

    private void UpdateInfo()
    {
        ShowInfoComponent();
    }
}
