using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "TextureTerrainData", menuName = "Game/Terrain/Cretae New Texture Data")]
public class TextureTerrainData : UpdatableTerrainData
{
    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    [SerializeField] private Layer[] m_layers;


    private float m_savedMinHeight;
    private float m_savedMaxHeight;
    public void ApplyToMaterial(Material material)
    {
        material.SetInt("layerCount", m_layers.Length);
        material.SetColorArray("baseColors", m_layers.Select(x => x.tint).ToArray());
        material.SetFloatArray("baseStartHeights", m_layers.Select(x => x.startHeight).ToArray());
        material.SetFloatArray("baseBlends", m_layers.Select(x => x.blendStrength).ToArray());
        material.SetFloatArray("baseColorStrength", m_layers.Select(x => x.tintStrength).ToArray());
        material.SetFloatArray("baseTextureScales", m_layers.Select(x => x.textureScale).ToArray());

        Texture2DArray textureArray = GenerateTextureArray(m_layers.Select(x => x.texture).ToArray());
        material.SetTexture("baseTextures", textureArray);

        UpdatedMeshHeights(material, m_savedMinHeight, m_savedMaxHeight);
    }

    public void UpdatedMeshHeights(Material material, float minValue, float maxValue)
    {
        m_savedMinHeight = minValue;
        m_savedMaxHeight = maxValue;

        material.SetFloat("minHeight", minValue);
        material.SetFloat("maxHeight", maxValue);
    }

    private Texture2DArray GenerateTextureArray(Texture2D[] textures)
    {
        Texture2DArray textureArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);
        for (int i = 0; i < textures.Length; i++)
        {
            textureArray.SetPixels(textures[i].GetPixels(), i);
        }
        textureArray.Apply();
        return textureArray;
    }

    [System.Serializable]
    public class Layer
    {
        public Texture2D texture;
        public Color tint;
        [Range(0, 1)]
        public float tintStrength;
        [Range(0, 1)]
        public float startHeight;
        [Range(0, 1)]
        public float blendStrength;
        public float textureScale;
    }
}
