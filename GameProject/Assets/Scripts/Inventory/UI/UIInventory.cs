using TheIslandKOD;
using UnityEngine;


public class UIInventory : MonoBehaviour
{
    public static UIInventory instance;


    [SerializeField] private GameObject m_gridInventory;

    private UIInventorySlot[] m_uISlots;
    private PlayerInventory m_playerInventory;

    public InventoryWithSlots inventory => m_playerInventory.inventory;

    private ReferenceSystem m_referenceSystem;
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
        m_uISlots = GetComponentsInChildren<UIInventorySlot>();
        m_referenceSystem = ReferenceSystem.instance;
        m_playerInventory = m_referenceSystem.player.GetComponent<PlayerInventory>();
        inventory.OnInventoryStateChangedEvent += OnInventoryStateChanged;

        SetupInventoryUI(inventory);
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        m_gridInventory.SetActive(visible);
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
