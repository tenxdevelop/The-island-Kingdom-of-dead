using UnityEngine;

public class HeadTarget : MonoBehaviour
{
    [SerializeField] private float m_distance;
    [SerializeField] private Transform m_camera;

    private void Update()
    {
        Ray desiredTargetRay = m_camera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Vector3 targetPosition = desiredTargetRay.origin + desiredTargetRay.direction * m_distance;
        transform.position = targetPosition;
    }
}
