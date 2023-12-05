using UnityEngine;
using UnityEngine.UI;

public class UIFuranceButton : MonoBehaviour
{
    [SerializeField] private Color m_buttonOffColor;
    [SerializeField] private Color m_buttonOnColor;

    private Image m_backGroundImage;
    private Button m_buttonFireOnOff;
    private Furance m_furance;

    private void Start()
    {
        m_buttonFireOnOff = GetComponent<Button>();
        m_backGroundImage = GetComponent<Image>();
    }

    public void SetaupButton(Furance furance)
    {
        m_furance = furance;
        furance.OnChangeContents += UpdateButton;
        if (m_furance.isFire)
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
        m_furance.OnChangeContents -= UpdateButton;
        m_furance = null;
        m_backGroundImage.color = m_buttonOffColor;
    }


    private void UpdateButton(bool canFire)
    {
        if (!canFire)
        {
            m_furance.StopFire();
            m_backGroundImage.color = m_buttonOffColor;
        }
        m_buttonFireOnOff.interactable = canFire;
    }

    public void OnClickButton()
    {

        if (m_furance != null)
        {
            m_furance.isFire = !m_furance.isFire;
            if (m_furance.isFire)
            {
                m_furance.Fire();
                m_backGroundImage.color = m_buttonOnColor;
            }
            else
            {
                m_furance.StopFire();
                m_backGroundImage.color = m_buttonOffColor;
            }
        }
    }
}
