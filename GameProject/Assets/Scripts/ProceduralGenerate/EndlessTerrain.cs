using UnityEngine;
using System.Collections.Generic;
using TheIslandKOD;

public class EndlessTerrain : MonoBehaviour
{

    private const float VIEW_MOVE_THRESHOLD_FOR_CHUNK_UPDATE = 25f;
    private const float SQR_VIEW_MOVE_THRESHOLD_FOR_CHUNK_UPDATE = VIEW_MOVE_THRESHOLD_FOR_CHUNK_UPDATE * VIEW_MOVE_THRESHOLD_FOR_CHUNK_UPDATE;

    [SerializeField] private LODInfo[] m_detailLevels;
    private static float m_maxViewDst;
    [SerializeField] private Transform m_viewer;
    [SerializeField] private Material m_mapMaterial;


    private static Vector2 m_viewerPosition;
    private Vector2 m_viewerPositionOld;
    private static MapGenerator m_mapGenerator;

    private int m_chunkSize;
    private int m_chunksvisibleInViewDst;
    

    private Dictionary<Vector2, TerrainChunk> m_terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> m_terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
    private void Start()
    {
        m_mapGenerator = GetComponent<MapGenerator>();

        m_maxViewDst = m_detailLevels[m_detailLevels.Length - 1].visibleDstThreshold;
        m_chunkSize = MapGenerator.MAX_CHUNK_SIZE - 1;
        m_chunksvisibleInViewDst = Mathf.RoundToInt(m_maxViewDst / m_chunkSize);
        UpdateVisibleChunks();
    }

    private void Update()
    {
        m_viewerPosition = new Vector2(m_viewer.position.x, m_viewer.position.z);

        if ((m_viewerPositionOld - m_viewerPosition).sqrMagnitude > SQR_VIEW_MOVE_THRESHOLD_FOR_CHUNK_UPDATE)
        {
            m_viewerPositionOld = m_viewerPosition;
            UpdateVisibleChunks();
        }

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
                    m_terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, m_chunkSize,
                                                                                    m_detailLevels, transform,
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
        
        private LODInfo[] m_detailLevels;
        private LODMesh[] m_lodMeshes;

        private MeshFilter m_meshFilter;
        private MeshRenderer m_meshRenderer;
        private MapData m_mapData;
        private bool m_mapDataReceived;
        private int m_previousLODIndex = -1;
        public TerrainChunk(Vector2 coordinate, int size, LODInfo[] detailLevels, Transform parent, Material material)
        {
            m_detailLevels = detailLevels;

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

            m_lodMeshes = new LODMesh[m_detailLevels.Length];
            for (int i = 0; i < m_detailLevels.Length; i++)
            {
                m_lodMeshes[i] = new LODMesh(m_detailLevels[i].lod, UpdateTerrainChunk);
            }

            m_mapGenerator.RequestMapData(m_position, OnMapDataReceived);
        }

        public void UpdateTerrainChunk()
        {
            if (m_mapDataReceived)
            {
                float viewerDstFromNearestEddge = Mathf.Sqrt(bounds.SqrDistance(m_viewerPosition));
                bool isVisible = viewerDstFromNearestEddge <= m_maxViewDst;

                if (isVisible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < m_detailLevels.Length - 1; i++)
                    {
                        if (viewerDstFromNearestEddge > m_detailLevels[i].visibleDstThreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (lodIndex != m_previousLODIndex)
                    {
                        LODMesh lodMesh = m_lodMeshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            m_previousLODIndex = lodIndex;
                            m_meshFilter.mesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(m_mapData);
                        }
                    }
                }

                SetVisible(isVisible);
            }
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
            this.m_mapData = mapData;
            this.m_mapDataReceived = true;

            Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.MAX_CHUNK_SIZE, 
                                                                     MapGenerator.MAX_CHUNK_SIZE);
            m_meshRenderer.material.mainTexture = texture;
            UpdateTerrainChunk();
        }

    }

    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDstThreshold;


    }

    private class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        public int lod;
        private System.Action m_updateCallback;

        public LODMesh(int lod, System.Action updateCallback)
        {
            this.lod = lod;
            this.m_updateCallback = updateCallback;
        }

        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            m_mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecieved);
        }

        private void OnMeshDataRecieved(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            m_updateCallback();
        }

    }
}
