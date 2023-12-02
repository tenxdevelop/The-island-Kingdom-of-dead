using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private List<AudioClip> m_backGroundClips = new List<AudioClip>();
    [SerializeField] private AudioClip m_clipActivateItem;

    private AudioSource m_backGroundSource;
    private void Start()
    {
        m_backGroundSource = GetComponent<AudioSource>();

        PlayerBackGroundSound();
    }


    private void Update()
    {
        if (!m_backGroundSource.isPlaying)
        {
            PlayerBackGroundSound();
        }
    }

    private void PlayerBackGroundSound()
    {
        m_backGroundSource.clip = m_backGroundClips[Random.Range(0, m_backGroundClips.Count)];
        m_backGroundSource.Play();
    }
}
