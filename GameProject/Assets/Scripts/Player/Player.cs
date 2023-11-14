using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_maxHealth;
    [SerializeField] private UIBar m_healthBar;

    private float m_currentHealth;

    private bool m_isDead;
    public bool isDead => m_isDead;

    public void TakeDamage(float damage)
    {
        UpdateHealth(-damage);  
    }
    public void HealthUp(float health)
    {
        UpdateHealth(health);
    }

    private void Start()
    {
        m_currentHealth = m_maxHealth;
        m_isDead = false;
    }

    private void UpdateHealth(float health)
    {
        m_currentHealth += health;

        if (m_currentHealth > m_maxHealth)
        {
            m_currentHealth = m_maxHealth;
        }
        m_healthBar.SetValueBar(m_currentHealth / m_maxHealth);

        m_isDead = m_currentHealth <= 0;  
    }
}
