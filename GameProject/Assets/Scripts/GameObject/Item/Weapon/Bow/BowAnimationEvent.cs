using System;
using UnityEngine;

public class BowAnimationEvent : MonoBehaviour
{
    public static Action OnFireEvent;

    [SerializeField] private Transform m_ArrowAnimation;

    [SerializeField] private Transform BowString;
    [SerializeField] private Transform BowStringInitPos;
    [SerializeField] private Transform BowStringInitParent;
    [SerializeField] private Transform BowStringHandPullPos;
    [SerializeField] private AudioClip m_clipPullString;

    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PullString()
    {
        BowString.transform.position = BowStringHandPullPos.position;
        BowString.transform.parent = BowStringHandPullPos;
        m_audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        m_audioSource.PlayOneShot(m_clipPullString, 0.7f);
    }

    public void ResetString()
    {
        BowString.transform.position = BowStringInitPos.position;
        BowString.transform.parent = BowStringInitParent;
    }

    public void EnableArrow()
    {
        m_ArrowAnimation.gameObject.SetActive(true);
    }

    public void DisableArrow()
    {
        m_ArrowAnimation.gameObject.SetActive(false);
    }

    public void Fire()
    {
        OnFireEvent?.Invoke();
    }
}
