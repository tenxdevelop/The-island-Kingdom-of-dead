using TheIslandKOD;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{

    [SerializeField] private Transform m_positionFoundation;
    [SerializeField] private Transform m_positionWall;

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
        Debug.Log("Not found position snap");
        return null;
    }
}
