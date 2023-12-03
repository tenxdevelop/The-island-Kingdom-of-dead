using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Health Info")]
    [SerializeField] private float m_maxHealth;
    [SerializeField] private UIBar m_healthBar;

    [Header("TakeDamage")]
    [SerializeField] private Image m_overlayDamage;
    [SerializeField] private float m_duration;
    [SerializeField] private float m_fadeSpeed;
    [SerializeField] private AudioClip m_clipTakeDamage;

    private float m_currentAlphaDamageOverlay;

    private float m_currentHealth;

    private AudioSource m_audioSource;

    private bool m_isDead;
    public bool isDead => m_isDead;

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            damage = -damage;
        }
        UpdateHealth(-damage);
        ActiveDamageOverlayEffect();
        m_audioSource.PlayOneShot(m_clipTakeDamage, 0.8f);
        StartCoroutine(UpdateDamageOverlay());
    }
    public void HealthUp(float health)
    {
        if (health < 0)
        {
            health = -health;
        }
        UpdateHealth(health);
        DisableDamageOverlayEffect();
    }

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_currentHealth = m_maxHealth;
        m_isDead = false;
        m_currentAlphaDamageOverlay = 0;
        DisableDamageOverlayEffect();
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

        if (m_isDead)
        {
            m_currentHealth = 0;
        }

        if (m_currentHealth < 30)
        {
            m_currentAlphaDamageOverlay = 0.4f;
        }
        else
        {
            m_currentAlphaDamageOverlay = 0;
        }

    }

    private IEnumerator UpdateDamageOverlay()
    {
        yield return new WaitForSeconds(m_duration);
        while (m_overlayDamage.color.a > m_currentAlphaDamageOverlay)
        {
            float tempAlpha = m_overlayDamage.color.a;
            tempAlpha -= Time.deltaTime * m_fadeSpeed;
            m_overlayDamage.color = new Color(m_overlayDamage.color.r, m_overlayDamage.color.g, m_overlayDamage.color.b, tempAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void ActiveDamageOverlayEffect()
    {
        m_overlayDamage.color = new Color(m_overlayDamage.color.r, m_overlayDamage.color.g, m_overlayDamage.color.b, 0.8f);
    }

    private void DisableDamageOverlayEffect()
    {
        m_overlayDamage.color = new Color(m_overlayDamage.color.r, m_overlayDamage.color.g, m_overlayDamage.color.b, 0);
    }
}
