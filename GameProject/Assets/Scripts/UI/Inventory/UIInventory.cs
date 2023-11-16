using TheIslandKOD;
using UnityEngine;


public class UIInventory : MonoBehaviour, IUIInventory
{
    public static UIInventory instance;

    [SerializeField] private GameObject m_gridInventory;

    private UIInventorySlot[] m_uISlots;
    private InventoryWithSlots m_inventory;
    

    public InventoryWithSlots inventory => m_inventory;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
        m_uISlots = GetComponentsInChildren<UIInventorySlot>();
    }
    private void Start()
    {
        SetVisible(false);
    }
    public void SetVisible(bool visible)
    {
        SetCursor(visible);
        UIQuickSlot.instance.SetActiveItem(!visible);
        m_gridInventory.SetActive(visible);
    }

    public void SetupInventoryUI(InventoryWithSlots inventory)
    {
        m_inventory = inventory;
        inventory.OnInventoryStateChangedEvent += OnInventoryStateChanged;
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

    private void SetCursor(bool state)
    {
        Cursor.visible = state;
        if (!state)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
