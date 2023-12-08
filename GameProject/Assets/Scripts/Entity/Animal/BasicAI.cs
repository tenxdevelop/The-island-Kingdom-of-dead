using System.Collections;
using UnityEngine;


public class BasicAI : MonoBehaviour
{
    private const string TAG_ANIMATION_WALK = "isWalking";

    [SerializeField] private AudioClip m_clipAttack;
    [SerializeField] private float m_moveSpeed = 0.2f;
    [SerializeField] private float m_runSpeed = 1f;
    [SerializeField] private Vector2 m_walkRange;

    [SerializeField] private Vector2 m_waitRange;
    [SerializeField] private bool m_isWalking;
    [SerializeField] private float m_gravity;

    [SerializeField] private float m_distanceAttack;
    [SerializeField] private float m_distanceVisible;

    private AudioSource m_audioSource;
    private Animator m_animator;
    private CharacterController m_characterController;
    private Player m_player;
    private Coroutine m_routine;

    private float m_waitTime;
    private float m_walkTime;
    private int m_walkDirection;

    private bool m_isGrounded;
    private float m_gravityState = -2.2f;
    private Vector3 m_velocity;

    private float m_walkCounter;
    private float m_waitCounter;

    public bool canWalk = true;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_characterController = GetComponent<CharacterController>();
        
        m_walkTime = Random.Range(m_walkRange.x, m_walkRange.y);
        m_waitTime = Random.Range(m_waitRange.x, m_waitRange.y);

        m_walkCounter = m_walkTime;
        m_waitCounter = m_waitTime;

        ChooseDirection();
    }

    private void Update()
    {
        UpdateMove();
        SetDistancePlayer();
    }

    private void ChooseDirection()
    {
        m_walkDirection = Random.Range(0, 4);

        m_isWalking = true;
        m_walkCounter = m_walkTime;
    }

    private void UpdateMove()
    {
        if (canWalk)
        {
            if (m_isWalking)
            {
                m_animator.SetBool(TAG_ANIMATION_WALK, true);

                m_walkCounter -= Time.deltaTime;

                switch (m_walkDirection)
                {
                    case 0:
                        Move(0);
                        break;
                    case 1:
                        Move(90);
                        break;
                    case 2:
                        Move(-90);
                        break;
                    case 3:
                        Move(180);
                        break;
                }

                if (m_walkCounter <= 0)
                {
                    m_isWalking = false;

                    m_animator.SetBool(TAG_ANIMATION_WALK, false);

                    m_waitCounter = m_waitTime;
                }
            }
            else
            {

                m_waitCounter -= Time.deltaTime;

                if (m_waitCounter <= 0)
                {
                    ChooseDirection();
                }
            }
        }

        m_isGrounded = m_characterController.isGrounded;
        m_velocity.y += m_gravity * Time.deltaTime;
        if (m_isGrounded && m_velocity.y < 0)
        {
            m_velocity.y = m_gravityState * 1.5f;
        }
        m_characterController.Move(m_velocity * Time.deltaTime);

    }

    private void Move(float rotate)
    {
        transform.localRotation = Quaternion.Euler(0f, rotate, 0f);
        m_characterController.Move(transform.forward * m_moveSpeed * Time.deltaTime);
    }

    private void SetDistancePlayer()
    {
        if (m_player != null)
        {
            var distance = Vector3.Distance(transform.position, m_player.transform.position);

            if (m_distanceVisible >= distance)
            {
                m_animator.SetBool(TAG_ANIMATION_WALK, true);
                var direction = m_player.transform.position - transform.position;
                direction.Normalize();
                transform.rotation = Quaternion.LookRotation(direction);
                m_characterController.Move(direction * m_runSpeed * Time.deltaTime);
                canWalk = false;
            }
            else
            {
                m_animator.SetBool(TAG_ANIMATION_WALK, false);
                canWalk = true;
            }
            if (m_distanceAttack >= distance)
            {
                if (m_routine == null)
                {
                    m_routine = StartCoroutine(AttackUpdate());
                }
            }
        }
        else
        {
            if (ReferenceSystem.instance.player != null)
            {
                 m_player = ReferenceSystem.instance.player.GetComponent<Player>();
            }
        }

    }


    private IEnumerator AttackUpdate()
    {
        m_audioSource.PlayOneShot(m_clipAttack, 0.7f);
        m_player.TakeDamage(10);
        yield return new WaitForSeconds(3);
        m_routine = null;

    }
}
