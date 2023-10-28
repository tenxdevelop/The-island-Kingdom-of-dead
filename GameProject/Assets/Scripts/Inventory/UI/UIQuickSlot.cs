using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickSlot : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_quickSlots = new List<GameObject>(); 
    [SerializeField] private Sprite m_notSelectedSprite;
    [SerializeField] private Sprite m_selectedSprite;
    private int m_currentQuickslotID = 0;
    private bool m_currentSlotActive = false;
    public void QuickSlotInputAction(int number)
    {
        var currentImageSlot = m_quickSlots[m_currentQuickslotID].GetComponent<Image>();
        currentImageSlot.sprite = m_notSelectedSprite;
        if (m_currentQuickslotID == number)
        {
            if (!m_currentSlotActive)
            {                
                currentImageSlot.sprite = m_selectedSprite;
            }
            else
            {                
                currentImageSlot.sprite = m_notSelectedSprite;
            }
            m_currentSlotActive = !m_currentSlotActive;
        }
        else
        {
            currentImageSlot.sprite = m_notSelectedSprite;
            m_currentQuickslotID = number;
            m_currentSlotActive = true;
            m_quickSlots[m_currentQuickslotID].GetComponent<Image>().sprite = m_selectedSprite;
        }
    }
}
