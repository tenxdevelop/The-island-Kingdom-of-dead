using TheIslandKOD;
using UnityEngine;


public class UIInventory : MonoBehaviour
{
    [SerializeField] private int m_capacity;

    private UIInventorySlot[] m_uISlots;
    private PlayerInventory m_playerInventory;

    public InventoryWithSlots inventory => m_playerInventory.inventory;

    private void Start()
    {
        m_uISlots = GetComponentsInChildren<UIInventorySlot>();
        m_playerInventory = new PlayerInventory(m_capacity);
        inventory.OnInventoryStateChangedEvent += OnInventoryStateChanged;

        SetupInventoryUI(inventory);
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void SetupInventoryUI(InventoryWithSlots inventory)
    {
        var allSlots = inventory.GetAllSlots();
        var allSlotsCount = allSlots.Length;
        for (int i = 0; i < allSlotsCount; i++)
        {
            var slot = allSlots[i];
            var uISlot = m_uISlots[i];
            uISlot.SetSlot(slot);
            uISlot.Refresh();
        }
    }

    private void OnInventoryStateChanged(object obj)
    {
        foreach (var uISlot in m_uISlots)
        {
            uISlot.Refresh();
        }
    }
}
