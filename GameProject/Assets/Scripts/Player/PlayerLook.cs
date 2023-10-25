using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    [SerializeField] private float m_xSensitivity = 30f;
    [SerializeField] private float m_ySensitivity = 30f;
    [SerializeField] private float m_MaxAngleScroll = 85f;

    private float m_xRotation = 0f;

    public void ProcessLook(Vector2 mouseScoll)
    {
        float mouseX = mouseScoll.x;
        float mouseY = mouseScoll.y;

        m_xRotation -= (mouseY * Time.deltaTime) * m_ySensitivity;
        m_xRotation = Mathf.Clamp(m_xRotation, -m_MaxAngleScroll, m_MaxAngleScroll);

        m_camera.transform.localRotation = Quaternion.Euler(m_xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * m_xSensitivity);
    }
}
