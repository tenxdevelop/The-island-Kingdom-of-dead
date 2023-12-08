using UnityEngine;

public class Animal : MonoBehaviour
{
    private const string TAG_ANIMATION_DEAD = "isDead";

    [SerializeField] private float m_health;
    private AnimalAttach m_animalAttach;
    private Animator m_animator;

    private bool m_isDead = false;

    private void Start()
    {
        m_animalAttach = GetComponent<AnimalAttach>();
        m_animator = GetComponent<Animator>();
        m_animalAttach.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Arrow>() != null)
        {
            UpdateHealth(-35);
        }
    }


    private void UpdateHealth(float value)
    {
        m_health += value;
        if (m_health <= 0 && !m_isDead)
        {
            m_animator.SetTrigger(TAG_ANIMATION_DEAD);
            m_isDead = true;
            m_animalAttach.enabled = true;
            m_animalAttach.OnStart();
            Destroy(GetComponent<BasicAI>());

        }
    }
}
