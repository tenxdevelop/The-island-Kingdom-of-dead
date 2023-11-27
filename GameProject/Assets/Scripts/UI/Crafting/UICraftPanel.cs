using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;
using UnityEngine.UI;

public class UICraftPanel : MonoBehaviour
{
    public static UICraftPanel instance { get; private set; }

    [SerializeField] private List<InventoryItemCraft> m_allCraftInfo;

    [SerializeField] private Transform m_craftItemPanel;
    [SerializeField] private UICraftItem m_prefabItemCraft;

    [SerializeField] private Button m_craftButton;
    [SerializeField] private Transform m_parentInfoResources;

    [SerializeField] private Image m_iconInfoImage;
    [SerializeField] private Sprite m_defualtImage;
    [SerializeField] private Text m_nameItemInfo;
    [SerializeField] private Text m_descriptionItemInfo;

    private GameManager m_gameManager;
    
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
        m_gameManager = GameManager.instance;
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        m_gameManager.SetCursorVisible(visible);
        if (visible)
        {
            LoadCraftItemInfo("Tools");
            m_craftButton.interactable = false;
        }
        else
        {
            ClearInfoComponent();
        }
        gameObject.SetActive(visible);
        UIQuickSlot.instance.SetActiveItem(!visible);
    }

    public void LoadCraftItemInfo(string category)
    {
        for (int i = 0; i < m_craftItemPanel.childCount; i++)
        {
            Destroy(m_craftItemPanel.GetChild(i).gameObject);
        }

        foreach (InventoryItemCraft craftInfo in m_allCraftInfo)
        {
            if (craftInfo.category.ToString().ToLower() == category.ToLower())
            {
                UICraftItem currentItemCraft = Instantiate(m_prefabItemCraft, m_craftItemPanel);
                currentItemCraft.LoadUIItemCraft(craftInfo, m_iconInfoImage, m_nameItemInfo, m_descriptionItemInfo, m_parentInfoResources);
            }
        }
    }

    private void ClearInfoComponent()
    {
        for (int i = 0; i < m_parentInfoResources.childCount; i++)
        {
            Destroy(m_parentInfoResources.GetChild(i).gameObject);
        }
        m_iconInfoImage.sprite = m_defualtImage;
        m_nameItemInfo.text = "";
        m_descriptionItemInfo.text = "";
    }

}
