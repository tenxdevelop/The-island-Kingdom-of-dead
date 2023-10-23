using JetBrains.Annotations;
using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    [SerializeField] private int m_sizeMap = 2;
    [SerializeField] private float m_scale = 1f;
    [SerializeField] private int m_chunkSize = 240;
    [SerializeField] private LODInfo[] m_detailLevels;
    
    [SerializeField] private Transform m_viewer;

    private Vector2 m_viewerPosition;
    private float[,] m_falloffMap;
    private float m_maxViewDst;

    private Dictionary<Vector2, Chunk> m_terrainChunkDictionary = new Dictionary<Vector2, Chunk>();
    public static List<Chunk> terrainChunksVisibleLastUpdate = new List<Chunk>();

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
        m_falloffMap = FalloffGenerator.GenerateFalloffMap((m_chunkSize+1) * (m_sizeMap+1));
        GenerateTerrainMap(m_chunkSize, m_falloffMap);
    }

    private void Update()
    {
        m_viewerPosition = new Vector2(m_viewer.position.x, m_viewer.position.z) / m_scale;
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
    private void GenerateTerrainMap(int chunkSize, float[,] falloffMap)
    {

        for (int yOffset = 0; yOffset <= m_sizeMap; yOffset++)
        {
            for (int xOffset = 0; xOffset <= m_sizeMap; xOffset++)
            {
                Vector2 chunkCoordinate = new Vector2(xOffset, yOffset);
                float[,] currentFalloffMap = GetFalloffMapOffset(m_falloffMap, m_chunkSize, m_sizeMap, chunkCoordinate);
                m_terrainChunkDictionary.Add(chunkCoordinate, new Chunk(chunkCoordinate, chunkSize, m_detailLevels, transform, 
                                                                        m_mapGenerator, this, currentFalloffMap, m_scale));
            }
        }
    }


    private float[,] GetFalloffMapOffset(float[,] falloffMap, int chunkSize,int sizeMap, Vector2 chunkCoordinate)
    {
        int xOffset = (chunkSize+1) * (int)chunkCoordinate.x;
        int yOffset = (chunkSize+1) * (sizeMap - (int)chunkCoordinate.y);
        float[,] mapOffset = new float[chunkSize + 1, chunkSize + 1];
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