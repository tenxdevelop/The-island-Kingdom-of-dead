using TheIslandKOD;
using UnityEngine;

public class UIStorage : MonoBehaviour, IUIInventory
{
    public static UIStorage instance { get; private set; }

    [SerializeField] private GameObject m_gridInventory;

    private UIInventorySlot[] m_uISlots;
    private InventoryWithSlots m_storage;
    private PlayerLook m_playerLook;
    private PlayerMovement m_playerMovement;
    public InventoryWithSlots inventory => m_storage;
    public void SetVisible(bool visible)
    {
        m_gridInventory.SetActive(visible);
        m_playerLook.ProcessLookStorage(visible);
        m_playerMovement.SetMove(!visible);
    }

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
        m_playerLook = ReferenceSystem.instance.player.GetComponent<PlayerLook>();
        m_playerMovement = ReferenceSystem.instance.player.GetComponent<PlayerMovement>();
        m_gridInventory.SetActive(false);
    }
    public void SetupStorageUI(InventoryWithSlots storage)
    {
        m_storage = storage;
        m_storage.OnInventoryStateChangedEvent += OnStorageStateChanged;
        var allSlots = storage.GetAllSlots();
        var allSlotsCount = allSlots.Length;
        for (int i = 0; i < allSlotsCount; i++)
        {
            var slot = allSlots[i];
            var uISlot = m_uISlots[i];
            uISlot.SetSlot(slot);
            uISlot.Refresh();
        }
    }

    public void UnSetupStorageUI()
    {
        if (m_storage != null)
        {
            m_storage.OnInventoryStateChangedEvent -= OnStorageStateChanged;
            m_storage = null;
        }
    }

    private void OnStorageStateChanged(object obj)
    {
        foreach (var uISlot in m_uISlots)
        {
            uISlot.Refresh();
        }
    }
}
