using Cinemachine;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerLook : MonoBehaviour
{
    public Transform itemDropPosition;
 
    [SerializeField] private float m_xSensitivity = 30f;
    [SerializeField] private float m_ySensitivity = 30f;
    [SerializeField] private float m_MaxAngleScroll = 85f;

    [SerializeField] private CinemachineVirtualCamera m_camera;

    private float m_xRotation = 0f;

    private bool m_isOpenInventory = false;
    private bool m_isOpenCraftingPanel = false;
    private bool m_isOpenMenu = false;
    private bool m_isOpenInteractableItem = false;

    private UIInventory m_uIInventory;
    private UICraftPanel m_uICraftingPanel;
    private UIMenu m_uIMenu;
    public CinemachineVirtualCamera Camera => m_camera;
    private void Start()
    {
        Cursor.visible = false;
        m_uIInventory = UIInventory.instance;
        m_uIMenu = UIMenu.instance;
        m_uICraftingPanel = UICraftPanel.instance;
    }

    public void ProcessLook(Vector2 mouseScoll)
    {
        if (!m_isOpenInventory && !m_isOpenCraftingPanel && !m_isOpenMenu)
        {
            float mouseX = mouseScoll.x;
            float mouseY = mouseScoll.y;

            m_xRotation -= (mouseY * Time.deltaTime) * m_ySensitivity;
            m_xRotation = Mathf.Clamp(m_xRotation, -m_MaxAngleScroll, m_MaxAngleScroll);

            m_camera.transform.localRotation = Quaternion.Euler(m_xRotation, 0, 0);

            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * m_xSensitivity);
        }
    }

    public void ProcessLookInventory()
    {
        m_isOpenInventory = !m_isOpenInventory;
        if (!m_isOpenCraftingPanel && !m_isOpenInteractableItem)
        {
            m_uIInventory.SetVisible(m_isOpenInventory);
        }
       
    }

    public void ProcessLookCraftingPanel()
    {
        if (m_isOpenInventory)
        {
            ProcessLookInventory();
        }
        m_isOpenCraftingPanel = !m_isOpenCraftingPanel;
        m_uICraftingPanel.SetVisible(m_isOpenCraftingPanel);

        if (!m_isOpenInventory)
        {
            ProcessLookInventory();
        }
    }

    public void ProcessLookStorage(bool visible)
    {
        m_isOpenInteractableItem = visible;
        m_isOpenInventory = visible;
        m_uIInventory.SetVisible(visible);
    }

    public void ProcessLookCampFire(bool visible)
    {
        m_isOpenInteractableItem = visible;
        m_isOpenInventory = visible;
        m_uIInventory.SetVisible(visible);
    }

    public void ProcessLookMenu()
    {
        m_isOpenMenu = !m_isOpenMenu;
        m_uIMenu.SetVisible(m_isOpenMenu);
    }

}
