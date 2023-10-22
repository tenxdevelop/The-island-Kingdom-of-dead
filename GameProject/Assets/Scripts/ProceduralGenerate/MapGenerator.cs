using TheIslandKOD;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const int MAX_CHUNK_SIZE = 241;
    public enum DrawMode
    {
        None,
        ColorMap,
        NoiseMap,
        Mesh

    }
    [SerializeField] private DrawMode m_drawMode = DrawMode.None;
    [Range(0, 6)]
    [SerializeField] private int m_levelOfDetail = 0;
    [SerializeField] private int m_mapWidth = 10;
    [SerializeField] private int m_mapHeight = 10;
    [Range(1, 100)]
    [SerializeField] private int m_octaves = 2;
    [SerializeField] private int m_seed = 0;
    [SerializeField] private float m_noiseScale = 0.004f;
    [Range(0f, 1f)]
    [SerializeField] private float m_persistance = 1f;
    [SerializeField] private float m_lacunarity = 0.5f;
    [SerializeField] private float m_heightMultiplier = 10f;
    [SerializeField] private AnimationCurve m_heightCurve;
    [SerializeField] private Vector2 m_offsetNoiseMap = Vector2.zero;
    [SerializeField] private bool autoUpdate = false;

    [SerializeField] private TerrainType[] m_regions;
    
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MAX_CHUNK_SIZE, MAX_CHUNK_SIZE, m_noiseScale, m_seed,
                                                   m_octaves, m_persistance, m_lacunarity, m_offsetNoiseMap);


        Color[] colorMap = GenerateColorMap(noiseMap);

        MapDisplay mapDisplay = gameObject.GetComponent<MapDisplay>();
        if (m_drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (m_drawMode == DrawMode.ColorMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, MAX_CHUNK_SIZE, MAX_CHUNK_SIZE));
        }
        else if (m_drawMode == DrawMode.Mesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, m_heightMultiplier, m_heightCurve, m_levelOfDetail),
                                TextureGenerator.TextureFromColorMap(colorMap, MAX_CHUNK_SIZE, MAX_CHUNK_SIZE));
        }
    }

    public bool GetAutoUpdate()
    {
        return autoUpdate;
    }


    private Color[] GenerateColorMap(float[,] heightMap)
    {
        Color[] colorMap = new Color[MAX_CHUNK_SIZE * MAX_CHUNK_SIZE];
        for (int y = 0; y < MAX_CHUNK_SIZE; y++)
        {
            for (int x = 0; x < MAX_CHUNK_SIZE; x++)
            {
                float currentHeight = heightMap[x, y];
                for (int i = 0; i < m_regions.Length; i++)
                {
                    if (currentHeight <= m_regions[i].height)
                    {
                        colorMap[y * MAX_CHUNK_SIZE + x] = m_regions[i].color;
                        break;
                    }
                }
            }
        }
        return colorMap;
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}