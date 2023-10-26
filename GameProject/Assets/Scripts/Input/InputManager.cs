using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private PlayerInput m_playerInput;
    private PlayerInput.OnFootActions m_onFoot;

    private PlayerMovement m_playerMovement;
    private PlayerLook m_playerLook;

    public PlayerInput.OnFootActions OnFoot => m_onFoot;
    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_onFoot = m_playerInput.OnFoot;
        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerLook = GetComponent<PlayerLook>();
    }
    
    private void FixedUpdate()
    {
        m_playerMovement.ProcessMove(m_onFoot.Movement.ReadValue<Vector2>());
        m_onFoot.Jump.performed += ctx => m_playerMovement.Jump();
        m_onFoot.LookInventory.performed += ctx => m_playerLook.ProcessLookInventory();

    }

    private void LateUpdate()
    {
        m_playerLook.ProcessLook(m_onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        m_onFoot.Enable();
    }

    private void OnDisable()
    {
        m_onFoot.Disable();
    }
}
