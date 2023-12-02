using System.Collections.Generic;
using UnityEngine;


namespace TheIslandKOD
{
    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDstThreshold;

    }
    public class Chunk 
    {

        private static int number;
        private Vector2 m_position;
        private GameObject m_meshObject;
        
        private Bounds bounds;

        private LODInfo[] m_detailLevels;
        private LODMesh[] m_lodMeshes;

        private MapData m_mapData;
        private float[,] m_falloffMap;
        private GenerateMap m_generateMap;
        private MapGenerator m_mapGenerator;

        private MeshFilter m_meshFilter;
        private MeshRenderer m_meshRenderer;
        private MeshCollider m_meshCollider;

        private bool m_mapDataReceived;
        private int m_previousLODIndex = -1;

        private List<PrefabTerrain> m_prefabsTerrain;
        private Transform m_gameObjectParent;
        private bool m_hasRequestedGeneratePrefabs = false;
        private bool m_hasCollider = false;

        public bool loadChunk => m_hasRequestedGeneratePrefabs;
        public Chunk(Vector2 coordinate, int size, LODInfo[] detailsLevels, Transform parent, Transform parentPrefab, Material material,
                     MapGenerator mapGenerator, GenerateMap generateMap, float[,] falloffMap, float scale, List<PrefabTerrain> prefabsTerrain)
        {
            m_prefabsTerrain = prefabsTerrain;
            m_gameObjectParent = parentPrefab;

            m_detailLevels = detailsLevels;
            m_generateMap = generateMap;
            m_falloffMap = falloffMap;
            m_mapGenerator = mapGenerator;

            m_position = coordinate * size;
            Vector3 positionV3 = new Vector3(m_position.x, 0, m_position.y);
            bounds = new Bounds(m_position, Vector2.one * size);
            
            m_meshObject = new GameObject("TerrainChunk" + number++);
            m_meshObject.layer = (int)LayerType.Terrain;
            m_meshFilter = m_meshObject.AddComponent<MeshFilter>();
            m_meshRenderer = m_meshObject.AddComponent<MeshRenderer>();
            m_meshCollider = m_meshObject.AddComponent<MeshCollider>();

            m_meshObject.transform.parent = parent;
            m_meshObject.transform.position = positionV3 * scale;
            m_meshObject.transform.localScale = Vector3.one * scale;
            m_meshRenderer.sharedMaterial = material;

            SetVisible(false);

            m_lodMeshes = new LODMesh[m_detailLevels.Length];
            for (int i = 0; i < m_detailLevels.Length; i++)
            {
                m_lodMeshes[i] = new LODMesh(m_detailLevels[i].lod, m_mapGenerator);
            }

            m_mapGenerator.RequestMapData(m_position, OnMapDataReceived);
        }

        public Bounds GetBounds()
        {
            return bounds;
        }
        public void SetVisible(bool visible)
        {
            m_meshObject.SetActive(visible);
        }

        public void UpdateChunk(bool visible, float distance)
        {
            if (m_mapDataReceived)
            {
                if (visible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < m_detailLevels.Length - 1; i++)
                    {
                        if (distance > m_detailLevels[i].visibleDstThreshold)
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
                            m_meshCollider.sharedMesh = lodMesh.mesh;
                            m_hasCollider = true;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(m_mapData);             
                            UpdateChunk(visible, distance);
                        }
                        
                    }

                    GenerateMap.terrainChunksVisibleLastUpdate.Add(this);

                }

                SetVisible(visible);
                if (!m_hasRequestedGeneratePrefabs && visible && m_hasCollider)
                {
                    GeneratePrefab(m_mapData);
                    m_hasRequestedGeneratePrefabs = true;
                }
            }
        }

        public bool IsVisible()
        {
            return m_meshObject.activeSelf;
        }

        private void OnMapDataReceived(MapData mapData)
        {

            m_mapData = GetFalloffMap(mapData);
            
            m_mapDataReceived = true;

            float viewerDstFromNearestEddge = Mathf.Sqrt(bounds.SqrDistance(m_generateMap.GetViewPosition()));
            bool isVisible = viewerDstFromNearestEddge <= m_generateMap.GetMaxViewDst();
            UpdateChunk(isVisible, viewerDstFromNearestEddge);

        }

        private MapData GetFalloffMap(MapData mapData)
        {
            float[,] heightMap = mapData.heightMap;
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                for (int x = 0; x < heightMap.GetLength(0); x++)
                {
                    heightMap[x, y] = Mathf.Clamp01(heightMap[x, y] - m_falloffMap[x, y]);
                }
            }

            return new MapData(heightMap);
        }

        private void GeneratePrefab(MapData mapData)
        {
            int size = mapData.heightMap.GetLength(0);

            var heightMap = mapData.heightMap;
            
            float[,] noiseMap = new float[size, size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    noiseMap[x, y] = Mathf.PerlinNoise(x * m_generateMap.prefabNoiseScale, y * m_generateMap.prefabNoiseScale);
                }
            }

            for (int i = 0; i < m_prefabsTerrain.Count; i++)
            {
                int step = m_prefabsTerrain[i].rangePrefabs;
                Vector2 rangeSpawn = m_prefabsTerrain[i].SpawnRangeHeight;
                GameObject prefab = m_prefabsTerrain[i].prefab;
                Vector2 scaleRange = m_prefabsTerrain[i].scaleRangeRandom;
                Vector2 rotationRange = m_prefabsTerrain[i].rotationRangeRandom;
                float offsetHeigt = m_prefabsTerrain[i].offsetHeight;
                Vector2 randomNoise = m_prefabsTerrain[i].NoiseRandomSpawn;
                float scale = Random.Range(scaleRange.x, scaleRange.y);
                for (int y = 0; y < size; y += step)
                {
                    for (int x = 0; x < size; x += step)
                    {
                        var worldPos = Mathf.Lerp(m_mapGenerator.terrainData.minHeight, m_mapGenerator.terrainData.maxHeight, m_mapGenerator.terrainData.heightCurve.Evaluate(heightMap[x, y]));
                        if (worldPos > rangeSpawn.x && worldPos < rangeSpawn.y)
                        {
                            var v = Random.Range(randomNoise.x, randomNoise.y);
                            if (noiseMap[x,y] > v)
                            {
                                var gameObject = GameObject.Instantiate(prefab, m_meshObject.transform);
                                
                                gameObject.transform.localPosition = new Vector3(GetPosition(x, size), m_mapGenerator.terrainData.maxHeight, GetPosition(-y + size, size));
                                gameObject.transform.rotation = Quaternion.Euler(0, Random.Range(rotationRange.x, rotationRange.y), 0);
                                GetCorrectHeightPositionPrefab(gameObject, offsetHeigt);
                                gameObject.transform.localScale = Vector3.one * scale;                            
                                gameObject.transform.parent = m_gameObjectParent;
                            }
                        }
                    }
                
                }
            }
        }

        private float GetPosition(int coordinate, int size)
        {
            return coordinate - size / 2;
        }


        private void GetCorrectHeightPositionPrefab(GameObject prefab, float offset)
        {
            Ray ray = new Ray(prefab.transform.position, -prefab.transform.up);
            Physics.Raycast(ray, out RaycastHit hit, 1000, (int)Mathf.Pow(2, (int)LayerType.Terrain));
            if (hit.collider)
            {
                prefab.transform.position = new Vector3(prefab.transform.position.x, hit.point.y + offset, prefab.transform.position.z);
            }
        }
    }
}
