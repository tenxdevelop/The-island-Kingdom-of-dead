using UnityEngine;
using UnityEngine.UI;

public class UICampFireButton : MonoBehaviour
{
    [SerializeField] private Color m_buttonOffColor;
    [SerializeField] private Color m_buttonOnColor;

    private Image m_backGroundImage;
    private Button m_buttonFireOnOff;
    private CampFire m_campFire;

    private void Start()
    {
        m_buttonFireOnOff = GetComponent<Button>();
        m_backGroundImage = GetComponent<Image>();
    }

    public void SetaupButton(CampFire campFire)
    {
        m_campFire = campFire;
        campFire.OnChangeContents += UpdateButton;
        if (m_campFire.isFire)
        { 
            m_backGroundImage.color = m_buttonOnColor;
        }
        else
        {
            m_backGroundImage.color = m_buttonOffColor;
        }
    }

    public void UnSetaupButton()
    {
        m_campFire.OnChangeContents -= UpdateButton;
        m_campFire = null;
        m_backGroundImage.color = m_buttonOffColor;
    }


    private void UpdateButton(bool canFire)
    {
        if (!canFire)
        {
            m_campFire.StopFire();
            m_backGroundImage.color = m_buttonOffColor;
        }
        m_buttonFireOnOff.interactable = canFire;
    }

    public void OnClickButton()
    {
        
        if (m_campFire != null)
        {
            m_campFire.isFire = !m_campFire.isFire;
            if (m_campFire.isFire)
            {
                m_campFire.Fire();
                m_backGroundImage.color = m_buttonOnColor;
            }
            else
            {
                m_campFire.StopFire();
                m_backGroundImage.color = m_buttonOffColor;
            }
        }
    }

}
