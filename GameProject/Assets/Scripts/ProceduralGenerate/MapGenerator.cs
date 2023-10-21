using TheIslandKOD;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int m_mapWidth = 10;
    [SerializeField] private int m_mapHeight = 10;
    [SerializeField] private float m_noiseScale = 0.004f;


    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(m_mapWidth, m_mapHeight, m_noiseScale); 

        MapDisplay mapDisplay = gameObject.GetComponent<MapDisplay>();
        mapDisplay.DrawNoiseMap(noiseMap);
    }

    
}
