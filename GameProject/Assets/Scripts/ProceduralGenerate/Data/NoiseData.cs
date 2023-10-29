using TheIslandKOD;
using UnityEngine;

[CreateAssetMenu(fileName = "NoiseData", menuName = "Game/Terrain/Cretae New Noise Data")]
public class NoiseData : UpdatableTerrainData
{
    [SerializeField] private Noise.NormalizeMode m_normalizeMode = Noise.NormalizeMode.Local;
    [Range(1, 100)]
    [SerializeField] private int m_octaves = 2;
    [SerializeField] private int m_seed = 0;
    [SerializeField] private float m_noiseScale = 0.004f;
    [Range(0f, 1f)]
    [SerializeField] private float m_persistance = 1f;
    [SerializeField] private float m_lacunarity = 0.5f;
    [SerializeField] private Vector2 m_offsetNoiseMap = Vector2.zero;

    public Noise.NormalizeMode normalizeMode => m_normalizeMode;
    public int octaves => m_octaves;
    public int seed => m_seed;
    public float noiseScale => m_noiseScale;
    public float persistance => m_persistance;
    public float lacunarity => m_lacunarity;
    public Vector2 offsetNoiseMap => m_offsetNoiseMap;
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (m_lacunarity < 1)
        {
            m_lacunarity = 1;
        }
        if (m_octaves < 0)
        {
            m_octaves = 0;
        }
        base.OnValidate();
    }
#endif
}
