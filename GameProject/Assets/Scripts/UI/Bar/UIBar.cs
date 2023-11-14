using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField] private float m_chipSpeed = 2f;
    [SerializeField] private Image m_frontImage;
    [SerializeField] private Image m_backImage;
    private float m_amount = 1f;
    private float m_lerptimer;
    public void SetValueBar(float value)
    {
        m_amount = value;
        m_lerptimer = 0;
    }

    private void Update()
    {
        UpdateBarUI(m_amount);
    }
    private void UpdateBarUI(float value)
    {
        float fillF = m_frontImage.fillAmount;
        float fillB = m_backImage.fillAmount;
        if (fillB > value)
        {
            m_frontImage.fillAmount = value;
            m_backImage.color = new Color(220, 0, 0, 150);
            m_lerptimer += Time.deltaTime;
            float percentComplete = m_lerptimer / m_chipSpeed;
            m_backImage.fillAmount = Mathf.Lerp(fillB, value, percentComplete);
        }
        if (value > fillF)
        {
            m_backImage.color = new Color(0, 150, 0, 150);
            m_backImage.fillAmount = value;
            m_lerptimer += Time.deltaTime;
            float percentComplete = m_lerptimer / m_chipSpeed;
            m_frontImage.fillAmount = Mathf.Lerp(fillF, value, percentComplete);
        }
    }
}
