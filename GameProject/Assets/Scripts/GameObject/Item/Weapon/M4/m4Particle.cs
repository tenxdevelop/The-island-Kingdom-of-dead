using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m4Particle : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_particle;
    [SerializeField] private Light m_light;
    private Coroutine m_coroutine;
    public void Activate()
    {
        if (m_coroutine == null)
        {
            m_particle.Play();
            m_light.enabled = true;
            m_coroutine = StartCoroutine(WaitForSecond());
        }
    }

    private IEnumerator WaitForSecond()
    {
        yield return new WaitForSeconds(0.1f);
        m_particle.Stop();
        m_light.enabled = false;
        m_coroutine = null;
    }

}
