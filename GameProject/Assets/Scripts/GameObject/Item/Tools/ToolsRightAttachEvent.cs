using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsRightAttachEvent : MonoBehaviour
{
    [SerializeField] private AudioClip m_clipRightAttach;

    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Attach()
    {
        m_audioSource.PlayOneShot(m_clipRightAttach, 0.3f);
    }

}
