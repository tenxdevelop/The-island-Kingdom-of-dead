using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerDeadEvent;
    [Header("Health Info")]
    [SerializeField] private float m_maxHealth;
    private UIBar m_healthBar;

    [Header("Hunger info")]
    [SerializeField] private float m_maxHunger;
    private UIBar m_hungerBar;

    [Header("TakeDamage")]
    private Image m_overlayDamage;
    [SerializeField] private float m_duration;
    [SerializeField] private float m_fadeSpeed;
    [SerializeField] private AudioClip m_clipTakeDamage;

    private float m_currentAlphaDamageOverlay;

    private float m_currentHealth;
    private float m_currentHunger;
    private Coroutine m_routine;

    private AudioSource m_audioSource;

    private bool m_isDead;
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

    public void HungerUp(float amountHunger)
    {
        if (amountHunger < 0)
            return;

        if (amountHunger + m_currentHunger > m_maxHunger)
        {
            m_currentHunger = m_maxHunger;
        }
        else
        {
            m_currentHunger += amountHunger;
        }
        m_hungerBar.SetValueBar(m_currentHunger / m_maxHunger);
    }

    private void Start()
    {
        m_healthBar = ReferenceSystem.instance.healthBar;
        m_hungerBar = ReferenceSystem.instance.hungerBar;
        m_overlayDamage = ReferenceSystem.instance.damageOverlay;
        m_audioSource = GetComponent<AudioSource>();
        m_currentHealth = m_maxHealth;
        m_currentHunger = m_maxHunger;
        m_isDead = false;
        m_currentAlphaDamageOverlay = 0;
        DisableDamageOverlayEffect();
        m_routine = StartCoroutine(UpdateHungerState());
    }

    private void OnDestroy()
    {
        StopCoroutine(m_routine);
    }

    private void UpdateHealth(float health)
    {
        m_currentHealth += health;

        if (m_currentHealth > m_maxHealth)
        {
            m_currentHealth = m_maxHealth;
        }
        
        m_healthBar.SetValueBar(m_currentHealth / m_maxHealth);

        if (!m_isDead && m_currentHealth <= 0)
        {
            m_currentHealth = 0;
            OnPlayerDeadEvent?.Invoke();
            m_isDead = true;
            Destroy(gameObject);
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

    private IEnumerator UpdateHungerState()
    {
        while (true)
        {
            m_currentHunger -= 2;
            if (m_currentHunger <= 0)
            {
                m_currentHunger = 0;
                TakeDamage(2);
            }
            m_hungerBar.SetValueBar(m_currentHunger / m_maxHunger);
            yield return new WaitForSeconds(1f);
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
