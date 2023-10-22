using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Renderer m_renderer;

    public void DrawTexture(Texture2D texture)
    {
 
        m_renderer.sharedMaterial.mainTexture = texture;
        m_renderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
}
