using UnityEngine;
using TheIslandKOD;

public class UIFurance : MonoBehaviour, IUIInventory
{
    public static UIFurance instance { get; private set; }

    [SerializeField] private UIFuranceButton m_uIBbuttonFireOnOff;
    [SerializeField] private Transform m_frame;

    private UIInventorySlot[] m_uISlots;
    private InventoryWithSlots m_contentsCampFire;
    private PlayerLook m_playerLook;
    private PlayerMovement m_playerMovement;
    public InventoryWithSlots inventory => m_contentsCampFire;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;

        m_uISlots = GetComponentsInChildren<UIInventorySlot>();
        ReferenceSystem.OnFindedObjecs += OnInitPlayer;
        GenerateMap.OnInitMapedEvent += OnInitMap;
    }

    private void OnInitPlayer()
    {
        ReferenceSystem.OnFindedObjecs -= OnInitPlayer;
        m_playerLook = ReferenceSystem.instance.player.GetComponent<PlayerLook>();
        m_playerMovement = ReferenceSystem.instance.player.GetComponent<PlayerMovement>();
    }

    public void SetVisible(bool visible)
    {
        m_frame.gameObject.SetActive(visible);
        m_playerLook.ProcessLookCampFire(visible);
        m_playerMovement.SetMove(!visible);
    }

    public void SetupContentCampFireUI(InventoryWithSlots contents, Furance furance)
    {
        m_uIBbuttonFireOnOff.SetaupButton(furance);
        m_contentsCampFire = contents;
        m_contentsCampFire.OnInventoryStateChangedEvent += OnContentsCampFireStateChanged;
        var allSlots = contents.GetAllSlots();
        var allSlotsCount = allSlots.Length;
        for (int i = 0; i < allSlotsCount; i++)
        {
            var slot = allSlots[i];
            var uISlot = m_uISlots[i];
            uISlot.SetSlot(slot);
            uISlot.Refresh();
        }
    }

    public void UnSetupContentCampFireUI()
    {
        if (m_contentsCampFire != null)
        {
            m_contentsCampFire.OnInventoryStateChangedEvent -= OnContentsCampFireStateChanged;
            m_uIBbuttonFireOnOff.UnSetaupButton();
            m_contentsCampFire = null;
        }
    }

    public void OnContentsCampFireStateChanged(object obj)
    {
        foreach (var uISlot in m_uISlots)
        {
            uISlot.Refresh();
        }
    }

    private void OnInitMap()
    {
        GenerateMap.OnInitMapedEvent -= OnInitMap;
        m_frame.gameObject.SetActive(false);
    }

}
