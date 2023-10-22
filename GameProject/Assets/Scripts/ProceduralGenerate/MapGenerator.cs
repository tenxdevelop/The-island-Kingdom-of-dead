using TheIslandKOD;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        None,
        ColorMap,
        NoiseMap,
        Mesh

    }
    [SerializeField] private DrawMode m_drawMode = DrawMode.None;
    [SerializeField] private int m_mapWidth = 10;
    [SerializeField] private int m_mapHeight = 10;
    [Range(1, 100)]
    [SerializeField] private int m_octaves = 2;
    [SerializeField] private int m_seed = 0;
    [SerializeField] private float m_noiseScale = 0.004f;
    [Range(0f, 1f)]
    [SerializeField] private float m_persistance = 1f;
    [SerializeField] private float m_lacunarity = 0.5f;
    [SerializeField] private Vector2 m_offsetNoiseMap = Vector2.zero;
    [SerializeField] private bool autoUpdate = false;

    [SerializeField] private TerrainType[] m_regions;
    
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(m_mapWidth, m_mapHeight, m_noiseScale, m_seed,
                                                   m_octaves, m_persistance, m_lacunarity, m_offsetNoiseMap);


        Color[] colorMap = GenerateColorMap(noiseMap);

        MapDisplay mapDisplay = gameObject.GetComponent<MapDisplay>();
        if (m_drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (m_drawMode == DrawMode.ColorMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, m_mapWidth, m_mapHeight));
        }
        else if (m_drawMode == DrawMode.Mesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap),
                                TextureGenerator.TextureFromColorMap(colorMap, m_mapWidth, m_mapHeight));
        }
    }

    public bool GetAutoUpdate()
    {
        return autoUpdate;
    }

    private void OnValidate()
    {
        if(m_mapHeight < 1)
            m_mapHeight = 1;
        if(m_mapWidth < 1)
            m_mapWidth = 1;

    }
    private Color[] GenerateColorMap(float[,] heightMap)
    {
        Color[] colorMap = new Color[m_mapWidth * m_mapHeight];
        for (int y = 0; y < m_mapHeight; y++)
        {
            for (int x = 0; x < m_mapWidth; x++)
            {
                float currentHeight = heightMap[x, y];
                for (int i = 0; i < m_regions.Length; i++)
                {
                    if (currentHeight <= m_regions[i].height)
                    {
                        colorMap[y * m_mapWidth + x] = m_regions[i].color;
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