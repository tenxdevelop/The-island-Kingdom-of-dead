using System.Collections.Generic;
using System.Linq;
using TheIslandKOD;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabTerrainData", menuName = "Game/Terrain/Cretae New Prefab Terrain Data")]
public class PrefabTerrainData : ScriptableObject
{
    [SerializeField] private PrefabTerrain[] m_prefabTerrains;

    public List<PrefabTerrain> prefabTerrains => m_prefabTerrains.ToList();
}

[System.Serializable]
public class PrefabTerrain
{
    public GameObject prefab;
    public float offsetHeight;
    public int rangePrefabs;

    public Vector2 SpawnRangeHeight;
    public Vector2 NoiseRandomSpawn;
    public Vector2 rotationRangeRandom;
    public Vector2 scaleRangeRandom;

}
