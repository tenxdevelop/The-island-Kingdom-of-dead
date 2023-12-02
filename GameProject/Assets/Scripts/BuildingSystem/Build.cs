using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    [SerializeField] private AudioClip m_buildClip;

    private AudioSource m_audioSource; 


    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.pitch = Random.Range(0.9f, 1.1f);
        m_audioSource.clip = m_buildClip;
        m_audioSource.Play();
    }
}
