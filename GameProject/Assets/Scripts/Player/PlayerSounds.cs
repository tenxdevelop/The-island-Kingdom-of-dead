using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private List<AudioClip> m_clipSteps;

    private AudioSource m_audioSource;


    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }


    public void FootStep()
    {
        int random = Random.Range(0, m_clipSteps.Count);
        m_audioSource.pitch = Random.Range(0.9f, 1.1f);
        m_audioSource.PlayOneShot(m_clipSteps[random]);
    }

}
