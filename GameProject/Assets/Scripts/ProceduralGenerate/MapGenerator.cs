using TheIslandKOD;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public const int MAX_CHUNK_SIZE = 241;
    public enum DrawMode
    {
        None,
        ColorMap,
        NoiseMap,
        Mesh

    }
    [SerializeField] private DrawMode m_drawMode = DrawMode.None;
    [Range(0, 6)]
    [SerializeField] private int m_levelOfDetail = 0;
    [SerializeField] private int m_mapWidth = 10;
    [SerializeField] private int m_mapHeight = 10;
    [Range(1, 100)]
    [SerializeField] private int m_octaves = 2;
    [SerializeField] private int m_seed = 0;
    [SerializeField] private float m_noiseScale = 0.004f;
    [Range(0f, 1f)]
    [SerializeField] private float m_persistance = 1f;
    [SerializeField] private float m_lacunarity = 0.5f;
    [SerializeField] private float m_heightMultiplier = 10f;
    [SerializeField] private AnimationCurve m_heightCurve;
    [SerializeField] private Vector2 m_offsetNoiseMap = Vector2.zero;
    [SerializeField] private bool autoUpdate = false;

    [SerializeField] private TerrainType[] m_regions;


    private Queue<MapThreadInfo<MapData>> m_mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    private Queue<MapThreadInfo<MeshData>> m_meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();
    public bool GetAutoUpdate()
    {
        return autoUpdate;
    }

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData();
        MapDisplay mapDisplay = gameObject.GetComponent<MapDisplay>();
        if (m_drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (m_drawMode == DrawMode.ColorMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, MAX_CHUNK_SIZE, MAX_CHUNK_SIZE));
        }
        else if (m_drawMode == DrawMode.Mesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, m_heightMultiplier, m_heightCurve, m_levelOfDetail),
                                TextureGenerator.TextureFromColorMap(mapData.colorMap, MAX_CHUNK_SIZE, MAX_CHUNK_SIZE));
        }

    }

    public void RequestMapData(Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(callback);
        };

        new Thread(threadStart).Start();
    }

    public void RequestMeshData(MapData mapData, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, callback);
        };

        new Thread(threadStart).Start();
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

    private void MapDataThread(Action<MapData> callback)
    {
        MapData mapData = GenerateMapData();
        lock (m_mapDataThreadInfoQueue)
        {
            MapThreadInfo<MapData> mapThreadInfo = new MapThreadInfo<MapData>(callback, mapData);
            m_mapDataThreadInfoQueue.Enqueue(mapThreadInfo);
        }
    }

    private void MeshDataThread(MapData mapData, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, m_heightMultiplier,
                                                              m_heightCurve, m_levelOfDetail);
        lock (m_meshDataThreadInfoQueue)
        {
            MapThreadInfo<MeshData> meshThreadInfo = new MapThreadInfo<MeshData>(callback, meshData);
            m_meshDataThreadInfoQueue.Enqueue(meshThreadInfo);
        }
    }

    private MapData GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MAX_CHUNK_SIZE, MAX_CHUNK_SIZE, m_noiseScale, m_seed,
                                                   m_octaves, m_persistance, m_lacunarity, m_offsetNoiseMap);

        Color[] colorMap = GenerateColorMap(noiseMap);

        return new MapData(noiseMap, colorMap);
    }

    private Color[] GenerateColorMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = heightMap[x, y];
                for (int i = 0; i < m_regions.Length; i++)
                {
                    if (currentHeight <= m_regions[i].height)
                    {
                        colorMap[y * width + x] = m_regions[i].color;
                        break;
                    }
                }
            }
        }
        return colorMap;
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

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }

}