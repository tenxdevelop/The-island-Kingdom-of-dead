using TheIslandKOD;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{

    [SerializeField] private Transform m_positionFoundation;
    [SerializeField] private Transform m_positionWall;
    [SerializeField] private Transform m_positionDoor;
    [SerializeField] private Transform m_positionWindow;
    [SerializeField] private Transform m_positionFloor;

    public Transform GetPosition(SnapPointType snapPointType)
    {
        if (snapPointType == SnapPointType.Foundation)
        {
            return m_positionFoundation;
        }
        if (snapPointType == SnapPointType.Wall)
        {
            return m_positionWall;
        }
        if (snapPointType == SnapPointType.Door)
        {
            return m_positionDoor;
        }
        if (snapPointType == SnapPointType.Window)
        {
            return m_positionWindow;
        }
        if (snapPointType == SnapPointType.Floor)
        {
            return m_positionFloor;
        }
        Debug.Log("Not found position snap");
        return null;
    }
}
