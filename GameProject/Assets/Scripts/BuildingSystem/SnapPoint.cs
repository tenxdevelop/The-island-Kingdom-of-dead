using TheIslandKOD;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    [SerializeField] private SnapPointType m_snapPointType;
    [SerializeField] private Transform m_positionFoundation;
    public SnapPointType snapPointType => m_snapPointType;
    public Transform positionFoundation => m_positionFoundation;
}
