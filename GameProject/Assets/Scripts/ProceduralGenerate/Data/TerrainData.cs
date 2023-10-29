using UnityEngine;

[CreateAssetMenu(fileName = "TerrainData", menuName = "Game/Terrain/Cretae New Terrain Data")]
public class TerrainData : UpdatableTerrainData
{
    [SerializeField] private float m_uniformScale = 1f;
    [SerializeField] private float m_heightMultiplier = 10f;
    [SerializeField] private AnimationCurve m_heightCurve;
    [SerializeField] private bool m_useFalloffMap = false;

    public float uniformScale => m_uniformScale;
    public float heightMultiplier => m_heightMultiplier;
    public AnimationCurve heightCurve => m_heightCurve;
    public bool useFalloffMap => m_useFalloffMap;

    public float minHeight => m_uniformScale * m_heightMultiplier * m_heightCurve.Evaluate(0);
    public float maxHeight => m_uniformScale * m_heightMultiplier * m_heightCurve.Evaluate(1);
}
