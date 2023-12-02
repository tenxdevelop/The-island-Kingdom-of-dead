using System;
using UnityEngine;

public class RifleAnimationEvent : MonoBehaviour
{
    public static RifleAnimationEvent instance;

    public event Action OnFireEvent;
    public AudioClip m_clipFire;

    [SerializeField] private AudioClip m_clipRifleDropClip;
    [SerializeField] private AudioClip m_clipRifleSetClip;
    [SerializeField] private AudioClip m_clipRifleReloadClip;
    [SerializeField] private AudioClip m_clipReloadRifle;

    private AudioSource m_audioSource;

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
        m_audioSource = GetComponent<AudioSource>();
    }


    public void RifleFire()
    {
        m_audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        m_audioSource.PlayOneShot(m_clipFire, 0.7f);

        OnFireEvent?.Invoke();
    }

    public void RifleDropClip()
    {
        m_audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        m_audioSource.PlayOneShot(m_clipRifleDropClip, 0.7f);
    }

    public void RifleSetClip()
    {
        m_audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        m_audioSource.PlayOneShot(m_clipRifleSetClip, 0.7f);
    }

    public void RifleReloadClip()
    {
        m_audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        m_audioSource.PlayOneShot(m_clipRifleReloadClip, 0.7f);
    }

    public void RifleReload()
    {
        m_audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        m_audioSource.PlayOneShot(m_clipReloadRifle, 0.7f);
    }
}
