using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string TAG_AMINATION_MOVE_X = "x";
    private const string TAG_AMINATION_MOVE_Y = "y";

    private const string TAG_AMINATION_JUMP = "jump";
    private const string TAG_AMINATION_ISGROUND = "isGround";

    [SerializeField] private float m_speed = 5f;
    [SerializeField] private float m_jumpStrength = 3f;
    [SerializeField] private float m_gravity = -9.81f;

    private CharacterController m_controller;
    private Vector3 m_playerVelocity;
    private bool m_isGrounded;
    private float m_gravityState = -2.2f;

    private Animator m_animatorPLayer;
    private float m_animationInterpolation = 1f;

    private bool m_canMove;

    private void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_animatorPLayer = GetComponentInChildren<Animator>();
        m_canMove = true;
    }

    
    private void Update()
    {
        m_isGrounded = m_controller.isGrounded;

        m_animatorPLayer.SetBool(TAG_AMINATION_ISGROUND, m_isGrounded);
        
    }

    public void ProcessMove(Vector2 derection)
    {
        if (m_canMove)
        {
            m_animationInterpolation = Mathf.Lerp(m_animationInterpolation, 1f, Time.deltaTime * 3);
            m_animatorPLayer.SetFloat(TAG_AMINATION_MOVE_X, derection.x * m_animationInterpolation);
            m_animatorPLayer.SetFloat(TAG_AMINATION_MOVE_Y, derection.y * m_animationInterpolation);
            Vector3 moveDerection = Vector3.right * derection.x + Vector3.forward * derection.y;
            m_controller.Move(transform.TransformDirection(moveDerection) * m_speed * Time.deltaTime);
            m_playerVelocity.y += m_gravity * Time.deltaTime;
            if (m_isGrounded && m_playerVelocity.y < 0)
            {
                m_playerVelocity.y = m_gravityState * 1.5f;
            }
            m_controller.Move(m_playerVelocity * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (m_isGrounded && m_canMove)
        {
            m_animatorPLayer.SetTrigger(TAG_AMINATION_JUMP);
            m_playerVelocity.y = Mathf.Sqrt(m_jumpStrength * m_gravity * m_gravityState);
        }
    }

    public void SetMove(bool canMove)
    {
        m_canMove = canMove;
    }
}
