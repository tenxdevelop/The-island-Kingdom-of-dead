using TheIslandKOD;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Renderer m_renderer;
    [SerializeField] private MeshFilter m_meshFilter;
    [SerializeField] private MeshRenderer m_meshRenderer;

    private MapGenerator m_mapGenerator;
    
    public void DrawTexture(Texture2D texture)
    {

        m_renderer.sharedMaterial.mainTexture = texture;
        m_renderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData)
    {
        m_meshFilter.sharedMesh = meshData.CreateMesh();
        m_meshFilter.transform.localScale = Vector3.one * m_mapGenerator.terrainData.uniformScale;
    }
    private void OnValidate()
    {
        if (m_mapGenerator == null)
        {
            m_mapGenerator = GetComponent<MapGenerator>();
        }
    }
}
