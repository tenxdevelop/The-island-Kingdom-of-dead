using UnityEngine;

[CreateAssetMenu(fileName = "TextureTerrainData", menuName = "Game/Terrain/Cretae New Texture Data")]
public class TextureTerrainData : UpdatableTerrainData
{
    [SerializeField] private Color[] m_baseColors;
    [Range(0, 1)]
    [SerializeField] private float[] m_baseStartHeights;

    private float m_savedMinHeight;
    private float m_savedMaxHeight;
    public void ApplyToMaterial(Material material)
    {
        material.SetInt("baseColorsCount", m_baseColors.Length);
        material.SetColorArray("baseColors", m_baseColors);
        material.SetFloatArray("baseStartHeights", m_baseStartHeights);

        UpdatedMeshHeights(material, m_savedMinHeight, m_savedMaxHeight);
    }

    public void UpdatedMeshHeights(Material material, float minValue, float maxValue)
    {
        m_savedMinHeight = minValue;
        m_savedMaxHeight = maxValue;

        material.SetFloat("minHeight", minValue);
        material.SetFloat("maxHeight", maxValue);
    }
}
