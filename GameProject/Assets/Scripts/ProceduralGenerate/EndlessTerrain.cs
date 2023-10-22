using UnityEngine;
using System.Collections.Generic;
using TheIslandKOD;

public class EndlessTerrain : MonoBehaviour
{
    private const float MAX_VIEW_DST = 450f;

    [SerializeField] private Transform m_viewer;
    [SerializeField] private Material m_mapMaterial;

    private static Vector2 m_viewerPosition;
    private static MapGenerator m_mapGenerator;

    private int m_chunkSize;
    private int m_chunksvisibleInViewDst;
    

    private Dictionary<Vector2, TerrainChunk> m_terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> m_terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
    private void Start()
    {
        m_mapGenerator = GetComponent<MapGenerator>();
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
                    m_terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, 
                                                                                    m_chunkSize, 
                                                                                    transform,
                                                                                    m_mapMaterial));
                }
            }
        }
    }

    public class TerrainChunk
    {
        private static int number = 1;
        private GameObject m_meshObject;
        private Vector2 m_position;
        private Bounds bounds;

        private MeshFilter m_meshFilter;
        private MeshRenderer m_meshRenderer;
        public TerrainChunk(Vector2 coordinate, int size, Transform parent, Material material)
        {
            m_position = coordinate * size;
            bounds = new Bounds(m_position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(m_position.x, 0, m_position.y);

            m_meshObject = new GameObject("TerrainChunk" + number++);
            m_meshFilter = m_meshObject.AddComponent<MeshFilter>();
            m_meshRenderer = m_meshObject.AddComponent<MeshRenderer>();

            m_meshRenderer.material = material;

            m_meshObject.transform.parent = parent;
            m_meshObject.transform.position = positionV3;
            
            SetVisible(false);

            m_mapGenerator.RequestMapData(OnMapDataReceived);
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

        private void OnMapDataReceived(MapData mapData)
        {
            m_mapGenerator.RequestMeshData(mapData, OnMeshDataReceived);
        }

        private void OnMeshDataReceived(MeshData meshData)
        {
            m_meshFilter.mesh = meshData.CreateMesh();
        }

    }

}
