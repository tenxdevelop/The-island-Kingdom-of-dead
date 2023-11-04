using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    [SerializeField] private int m_sizeMap = 2;
    
    [SerializeField] private LODInfo[] m_detailLevels;
    
    [SerializeField] private Transform m_viewer;

    [SerializeField] private float m_prefabNoiseSclae;

    [SerializeField] private PrefabTerrainData m_prefabData;
    
    private Vector2 m_viewerPosition;
    private float[,] m_falloffMap;
    private float m_maxViewDst;
    private int m_chunkSize;
    private int m_chunkSizeFalloffMap;

    private Dictionary<Vector2, Chunk> m_terrainChunkDictionary = new Dictionary<Vector2, Chunk>();
    
    public static List<Chunk> terrainChunksVisibleLastUpdate = new List<Chunk>();
    public float prefabNoiseScale => m_prefabNoiseSclae;

    private static MapGenerator m_mapGenerator;

    public Vector2 GetViewPosition()
    {
        return m_viewerPosition;
    }

    public float GetMaxViewDst()
    {
        return m_maxViewDst;
    }
    private void Start()
    {
        m_maxViewDst = m_detailLevels[m_detailLevels.Length - 1].visibleDstThreshold;
        m_mapGenerator = GetComponent<MapGenerator>();

        m_chunkSize = MapGenerator.MAX_CHUNK_SIZE - 1;
        m_chunkSizeFalloffMap = m_chunkSize + 3;

        m_falloffMap = FalloffGenerator.GenerateFalloffMap(m_chunkSizeFalloffMap * (m_sizeMap+1));
        GenerateTerrainMap(m_chunkSize);
    }

    private void Update()
    {
        m_viewerPosition = new Vector2(m_viewer.position.x, m_viewer.position.z) / m_mapGenerator.terrainData.uniformScale;
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(m_viewerPosition.x / m_chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(m_viewerPosition.y / m_chunkSize);

        for (int yOffset = 0; yOffset <= m_sizeMap; yOffset++)
        {
            for (int xOffset = 0; xOffset <= m_sizeMap; xOffset++)
            {
                Vector2 chunkCoordinate = new Vector2(xOffset, yOffset);
                Chunk chunk = m_terrainChunkDictionary[chunkCoordinate];
                float viewerDstFromNearestEddge = Mathf.Sqrt(chunk.GetBounds().SqrDistance(m_viewerPosition));
                bool isVisible = viewerDstFromNearestEddge <= m_maxViewDst;

                chunk.UpdateChunk(isVisible, viewerDstFromNearestEddge);
            }
        }

    }
    private void GenerateTerrainMap(int chunkSize)
    {

        for (int yOffset = 0; yOffset <= m_sizeMap; yOffset++)
        {
            for (int xOffset = 0; xOffset <= m_sizeMap; xOffset++)
            {
                Vector2 chunkCoordinate = new Vector2(xOffset, yOffset);
                float[,] currentFalloffMap = GetFalloffMapOffset(m_falloffMap, m_chunkSizeFalloffMap, m_sizeMap, chunkCoordinate);
                m_terrainChunkDictionary.Add(chunkCoordinate, new Chunk(chunkCoordinate, chunkSize, m_detailLevels, transform, m_mapGenerator.terrainMaterial,
                                                                        m_mapGenerator, this, currentFalloffMap, m_mapGenerator.terrainData.uniformScale, m_prefabData.prefabTerrains));
            }
        }
    }


    private float[,] GetFalloffMapOffset(float[,] falloffMap, int chunkSize,int sizeMap, Vector2 chunkCoordinate)
    {
        int xOffset = (chunkSize) * (int)chunkCoordinate.x;
        int yOffset = (chunkSize) * (sizeMap - (int)chunkCoordinate.y);
        float[,] mapOffset = new float[chunkSize, chunkSize];
        for (int y = 0; y < mapOffset.GetLength(1); y++)
        {
            for (int x = 0; x < mapOffset.GetLength(0); x++)
            {
                mapOffset[x,y] = falloffMap[x + xOffset,y + yOffset];
            }
        }
        
        return mapOffset;
    }
}
