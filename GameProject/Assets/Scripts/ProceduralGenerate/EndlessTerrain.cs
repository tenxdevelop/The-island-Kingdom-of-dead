using UnityEngine;
using System.Collections.Generic;
public class EndlessTerrain : MonoBehaviour
{
    private const float MAX_VIEW_DST = 450f;

    [SerializeField] private Transform m_viewer;

    private static Vector2 m_viewerPosition;

    private int m_chunkSize;
    private int m_chunksvisibleInViewDst;

    private Dictionary<Vector2, TerrainChunk> m_terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> m_terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
    private void Start()
    {
        m_chunkSize = MapGenerator.MAX_CHUNK_SIZE - 1;
        m_chunksvisibleInViewDst = Mathf.RoundToInt(MAX_VIEW_DST / m_chunkSize);
    }

    private void Update()
    {
        m_viewerPosition = new Vector2(m_viewer.position.x, m_viewer.position.z);
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (int i = 0; i < m_terrainChunksVisibleLastUpdate.Count; i++)
        {
            m_terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        m_terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(m_viewerPosition.x / m_chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(m_viewerPosition.y / m_chunkSize);

        for (int yOffset = -m_chunksvisibleInViewDst; yOffset <= m_chunksvisibleInViewDst; yOffset++)
        {
            for (int xOffset = -m_chunksvisibleInViewDst; xOffset <= m_chunksvisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (m_terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    m_terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    if (m_terrainChunkDictionary[viewedChunkCoord].IsVisible())
                    {
                        m_terrainChunksVisibleLastUpdate.Add(m_terrainChunkDictionary[viewedChunkCoord]);
                    }
                }
                else
                {
                    m_terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, m_chunkSize, transform));
                }
            }
        }
    }

    public class TerrainChunk
    {
        private GameObject m_meshObject;
        private Vector2 m_position;
        private Bounds bounds;

        public TerrainChunk(Vector2 coordinate, int size, Transform parent)
        {
            m_position = coordinate * size;
            bounds = new Bounds(m_position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(m_position.x, 0, m_position.y);

            m_meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            m_meshObject.transform.parent = parent;
            m_meshObject.transform.position = positionV3;
            m_meshObject.transform.localScale = Vector3.one * size / 10f;
            SetVisible(false);
        }

        public void UpdateTerrainChunk()
        {
            float viewerDstFromNearestEddge =  Mathf.Sqrt(bounds.SqrDistance(m_viewerPosition));
            bool isVisible = viewerDstFromNearestEddge <= MAX_VIEW_DST;
            SetVisible(isVisible);
        }

        public void SetVisible(bool visible)
        {
            m_meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return m_meshObject.activeSelf;
        }

    }

}
