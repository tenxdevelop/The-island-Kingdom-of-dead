using TheIslandKOD;
using UnityEngine;


public class UIInventory : MonoBehaviour
{
    [SerializeField] private InventoryItemInfo m_appleInfo;
    [SerializeField] private InventoryItemInfo m_pepperInfo;
    
    public InventoryWithSlots inventory => m_inventoryTester.inventory;

    private InventoryTester m_inventoryTester;
    private void Start()
    {
        var uISlots = GetComponentsInChildren<UIInventorySlot>();
        m_inventoryTester = new InventoryTester(m_appleInfo, m_pepperInfo, uISlots);
        m_inventoryTester.FillSlots();
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
