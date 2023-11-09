using Cinemachine;
using TheIslandKOD;
using UnityEngine;

public class PlayerInteractRaycast : MonoBehaviour
{

    [SerializeField] private float m_distance = 3f;
    [SerializeField] private LayerMask m_layerRaycastMask;
 
    private CinemachineVirtualCamera m_camera;
    private PlayerUI m_playerUI;
    private InputManager m_inputManager;
    private void Start()
    {
        m_camera = GetComponent<PlayerLook>().Camera;
        m_playerUI = GetComponent<PlayerUI>();
        m_inputManager = GetComponent<InputManager>();
        
    }

    
    private void Update()
    {
        
        m_playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, m_distance, m_layerRaycastMask))
        {
            var interactable = hitInfo.collider.GetComponent<InteractableRaycast>();
            if (interactable != null)
            {
                m_playerUI.UpdateText(interactable.promptMessage);
                if (m_inputManager.OnFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }

        }
    }
}
