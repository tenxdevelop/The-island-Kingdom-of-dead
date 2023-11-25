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


    public void PullString()
    {
        BowString.transform.position = BowStringHandPullPos.position;
        BowString.transform.parent = BowStringHandPullPos;
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
