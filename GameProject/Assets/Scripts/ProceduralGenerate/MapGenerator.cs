using TheIslandKOD;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public const int MAX_CHUNK_SIZE = 239;
    public enum DrawMode
    {
        None,
        NoiseMap,
        Mesh,
        FalloffMap

    }
    [SerializeField] private DrawMode m_drawMode = DrawMode.None;
    
    [Range(0, 6)]
    [SerializeField] private int m_editorPreViewLevelOfDetail = 0;
    
    [SerializeField] private TerrainData m_terrainData;
    [SerializeField] private NoiseData m_noiseData;
    [SerializeField] private TextureTerrainData m_textureData;
    [SerializeField] private Material m_terrainMaterial;


    [SerializeField] private bool m_autoUpdate = false;

    private float[,] m_falloffMap;

    private Queue<MapThreadInfo<MapData>> m_mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    private Queue<MapThreadInfo<MeshData>> m_meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public TerrainData terrainData => m_terrainData;
    public bool GetAutoUpdate() => m_autoUpdate;
   
    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay mapDisplay = GetComponent<MapDisplay>();
        if (m_drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (m_drawMode == DrawMode.Mesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, m_terrainData.heightMultiplier, m_terrainData.heightCurve, m_editorPreViewLevelOfDetail));
        }
        else if (m_drawMode == DrawMode.FalloffMap)
        {

            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(MAX_CHUNK_SIZE + 2)));

        }
    }

    public void RequestMapData(Vector2 centre, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(centre, callback);
        };

        new Thread(threadStart).Start();
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };

        new Thread(threadStart).Start();
    }

    private void OnValidate()
    {
        if (m_terrainData != null)
        {
            m_terrainData.OnValuesUpdatedEvent -= OnValuesUpdated;
            m_terrainData.OnValuesUpdatedEvent += OnValuesUpdated;
        }
        if (m_noiseData != null)
        {
            m_noiseData.OnValuesUpdatedEvent -= OnValuesUpdated;
            m_noiseData.OnValuesUpdatedEvent += OnValuesUpdated;
        }
        if (m_textureData != null)
        {
            m_textureData.OnValuesUpdatedEvent -= OnTextureValuesUpdated;
            m_textureData.OnValuesUpdatedEvent += OnTextureValuesUpdated;
        }
       
    }
    private void Update()
    {
        if (m_mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < m_mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> mapThreadInfo = m_mapDataThreadInfoQueue.Dequeue();
                mapThreadInfo.callback(mapThreadInfo.parametnr);
            }
        }
        if (m_meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < m_meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> meshThreadInfo = m_meshDataThreadInfoQueue.Dequeue();
                meshThreadInfo.callback(meshThreadInfo.parametnr);
            }
        }
    }


    private void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    private void OnTextureValuesUpdated()
    {
        m_textureData.ApplyToMaterial(m_terrainMaterial);
    }
    private void MapDataThread(Vector2 centre, Action<MapData> callback)
    {
        MapData mapData = GenerateMapData(centre);
        lock (m_mapDataThreadInfoQueue)
        {
            MapThreadInfo<MapData> mapThreadInfo = new MapThreadInfo<MapData>(callback, mapData);
            m_mapDataThreadInfoQueue.Enqueue(mapThreadInfo);
        }
    }

    private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, m_terrainData.heightMultiplier,
                                                              m_terrainData.heightCurve, lod);
        lock (m_meshDataThreadInfoQueue)
        {
            MapThreadInfo<MeshData> meshThreadInfo = new MapThreadInfo<MeshData>(callback, meshData);
            m_meshDataThreadInfoQueue.Enqueue(meshThreadInfo);
        }
    }

    private MapData GenerateMapData(Vector2 centre)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MAX_CHUNK_SIZE + 2, MAX_CHUNK_SIZE + 2, m_noiseData.noiseScale, m_noiseData.seed, m_noiseData.normalizeMode,
                                                   m_noiseData.octaves, m_noiseData.persistance, m_noiseData.lacunarity, centre + m_noiseData.offsetNoiseMap);

        noiseMap = GenerateFalloffMap(noiseMap);

        m_textureData.UpdatedMeshHeights(m_terrainMaterial, m_terrainData.minHeight, m_terrainData.maxHeight);

        return new MapData(noiseMap);
    }

    private float[,] GenerateFalloffMap(float[,] heightMap)
    {
        int height = heightMap.GetLength(1);
        int width = heightMap.GetLength(0);
        float[,] result = new float[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result[x, y] = heightMap[x, y];
            }
        }
        if (m_terrainData.useFalloffMap)
        {
            if (m_falloffMap == null)
            {
                m_falloffMap = FalloffGenerator.GenerateFalloffMap(MAX_CHUNK_SIZE + 2);
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (m_terrainData.useFalloffMap)
                    {
                        result[x, y] = Mathf.Clamp01(result[y, x] - m_falloffMap[x, y]);
                    }
                }

            }
        }
        return result;
    }
    private struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parametnr;

        public MapThreadInfo(Action<T> callback, T value)
        {
            this.callback = callback;
            this.parametnr = value;
        }
    }
}


public struct MapData
{
    public readonly float[,] heightMap;
    

    public MapData(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }

}