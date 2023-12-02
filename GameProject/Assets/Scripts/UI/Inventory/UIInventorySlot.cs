using TheIslandKOD;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventorySlot : UISlot
{

    [SerializeField] private UIInventoryItem m_uIInventoryItem;
    [SerializeField] private AudioClip m_clipMove;
    public IInventorySlot slot { get; private set; }


    private IUIInventory m_uIInventory;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_uIInventory = GetComponentInParent<IUIInventory>();
        m_audioSource = GetComponentInParent<AudioSource>();
    }
    public void SetSlot(IInventorySlot newSlot)
    {
        slot = newSlot;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        m_audioSource.PlayOneShot(m_clipMove, 0.5f);
        var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>();
        var otherSlot = otherSlotUI.slot;
        var inventory = m_uIInventory.inventory;

        inventory.TransitFromSlotToSlot(this, otherSlot, slot);

        Refresh();
        otherSlotUI.Refresh();
    }

    public void Refresh()
    {

        if (slot != null)
        {
            m_uIInventoryItem.Refresh(slot);
        }

    }
}
