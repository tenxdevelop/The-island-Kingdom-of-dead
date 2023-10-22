using TheIslandKOD;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int m_mapWidth = 10;
    [SerializeField] private int m_mapHeight = 10;
    [SerializeField] private int m_octaves = 2;
    [SerializeField] private float m_noiseScale = 0.004f;
    [SerializeField] private float m_persistance = 1f;
    [SerializeField] private float m_lacunarity = 0.5f;
    [SerializeField] private bool autoUpdate = false;
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(m_mapWidth, m_mapHeight, m_noiseScale, 
                                                   m_octaves, m_persistance, m_lacunarity); 

        MapDisplay mapDisplay = gameObject.GetComponent<MapDisplay>();
        mapDisplay.DrawNoiseMap(noiseMap);
    }

    public bool GetAutoUpdate()
    {
        return autoUpdate;
    }
    
}
