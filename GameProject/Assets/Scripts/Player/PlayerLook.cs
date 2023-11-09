using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Transform itemDropPosition;
 
    [SerializeField] private float m_xSensitivity = 30f;
    [SerializeField] private float m_ySensitivity = 30f;
    [SerializeField] private float m_MaxAngleScroll = 85f;

    [SerializeField] private CinemachineVirtualCamera m_camera;

    private float m_xRotation = 0f;

    private bool m_isOpenInventory = false;

    private UIInventory m_uIInventory;

    public CinemachineVirtualCamera Camera => m_camera;
    private void Start()
    {
        Cursor.visible = false;
        m_uIInventory = UIInventory.instance;
    }

    public void ProcessLook(Vector2 mouseScoll)
    {
        if (!m_isOpenInventory)
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
        m_uIInventory.SetVisible(m_isOpenInventory);
    }

    public void ProcessLookStorage(bool visible)
    {
        m_isOpenInventory = visible;
        m_uIInventory.SetVisible(visible);
    }
}
