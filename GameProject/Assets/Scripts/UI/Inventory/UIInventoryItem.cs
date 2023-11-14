using TheIslandKOD;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : UIItem
{
    [SerializeField] private Image m_imageIcon;
    [SerializeField] private Text m_textAmount;

    public IInventoryItem item { get; private set; } 

    public void Refresh(IInventorySlot slot)
    {
        if (slot.isEmpty)
        {
            Cleanup();
            return;
        }

        item = slot.item;
        m_imageIcon.sprite = item.info.spriteIcon;
        m_imageIcon.gameObject.SetActive(true);
        var textAmountEnabled = slot.amount > 1;
        m_textAmount.gameObject.SetActive(textAmountEnabled);
        if(textAmountEnabled)
            m_textAmount.text = "x" + slot.amount.ToString();
    }

    private void Cleanup()
    {
        m_textAmount.gameObject.SetActive(false);
        m_imageIcon.gameObject.SetActive(false);
    }
}
