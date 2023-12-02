using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instance;

    [SerializeField] private List<AudioClip> m_backGroundClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> m_backGroundEffect = new List<AudioClip>(); 
    [SerializeField] private List<AudioList> m_rifleFires = new List<AudioList>();
    [SerializeField] private AudioClip m_clipActivateItem;

    private AudioSource m_backGroundSource;
    public List<AudioList> rifleFires => m_rifleFires;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }
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
        m_backGroundSource.PlayOneShot(m_backGroundEffect[Random.Range(0, m_backGroundEffect.Count)]);
    }
}

namespace TheIslandKOD
{
    [System.Serializable]
    public class AudioList
    {
        public string key;
        public AudioClip clip;
    }
}
