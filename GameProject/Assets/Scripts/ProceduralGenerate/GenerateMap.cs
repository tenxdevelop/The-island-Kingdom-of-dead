using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    [SerializeField] private int m_widthMap = 4;
    [SerializeField] private int m_heightMap = 4;
    [SerializeField] private int m_chunkSize = 240;
    [SerializeField] private float m_maxViewDst = 600;
    [SerializeField] private LODInfo[] m_detailLevels;

    [SerializeField] private Transform m_viewer;

    private Vector2 m_viewerPosition;

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
        m_mapGenerator = GetComponent<MapGenerator>();
        GenerateTerrainMap(m_chunkSize);
    }

    private void Update()
    {
        m_viewerPosition = new Vector2(m_viewer.position.x, m_viewer.position.z);
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

        for (int yOffset = 0; yOffset <= m_heightMap; yOffset++)
        {
            for (int xOffset = 0; xOffset <= m_widthMap; xOffset++)
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

        for (int yOffset = 0; yOffset <= m_heightMap; yOffset++)
        {
            for (int xOffset = 0; xOffset <= m_widthMap; xOffset++)
            {
                Vector2 chunkCoordinate = new Vector2(xOffset, yOffset);
                m_terrainChunkDictionary.Add(chunkCoordinate, new Chunk(chunkCoordinate, chunkSize, m_detailLevels, transform, m_mapGenerator, this));
            }
        }
    }
}
