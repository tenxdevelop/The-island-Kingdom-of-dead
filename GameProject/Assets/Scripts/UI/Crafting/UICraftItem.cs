using System;
using TheIslandKOD;
using UnityEngine;
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
    private UIInputFeildCraft m_inputFieldCraft;
    private PlayerInventory m_playerInventory;

    private void Start()
    {
        m_inputFieldCraft = UIInputFeildCraft.instance;
        m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
    }

    private void OnDestroy()
    {
        OnDisableEvent();
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
        OnDisableEvent();
        m_iconImage.sprite = m_infoCraft.info.spriteIcon;
        m_textItemName.text = m_infoCraft.info.title;
        m_textDescription.text = m_infoCraft.info.description;

        UICraftButton.OnCraftButtonEvent += UpdateInfo;
        UICraftingQueue.OnRemoveQueueEvent += UpdateInfo;
        UIInputFeildCraft.OnChangeCountCraftEvent += UpdateInfo;

    }
    public void OnDisableEvent()
    {
        UICraftButton.OnCraftButtonEvent -= UpdateInfo;
        UICraftingQueue.OnRemoveQueueEvent -= UpdateInfo;
        UIInputFeildCraft.OnChangeCountCraftEvent -= UpdateInfo;
    }
    public void ShowInfoComponent()
    {
        UICraftButton.instance.UpdateButton(m_infoCraft, m_inputFieldCraft.countCraft);
        for (int i = 0; i < m_parentPrefab.childCount; i++)
        {
            Destroy(m_parentPrefab.GetChild(i).gameObject);
        }

        foreach (var itemComponent in m_infoCraft.craftComponents)
        {
            var haveItemAmount = m_playerInventory.inventory.GetItemAmount(Type.GetType("TheIslandKOD." + itemComponent.itemType));
            UIInfoCraftResources currentInfo = Instantiate(m_prefab, m_parentPrefab);
            var Total = itemComponent.amount * m_inputFieldCraft.countCraft;
            currentInfo.UpdateLoadInfo(itemComponent.amount.ToString(), itemComponent.itemType, Total.ToString(), haveItemAmount.ToString());
        }
    }

    private void UpdateInfo()
    {
        ShowInfoComponent();
    }
}
