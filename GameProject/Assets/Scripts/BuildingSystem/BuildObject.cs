using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    private List<Collider> m_contacts = new List<Collider>();
    private List<Collider> m_haveGround = new List<Collider>();

    private bool m_isBuildable = false;

    [SerializeField] private Material m_greenMat;
    [SerializeField] private Material m_redMat;
    [SerializeField] private LayerType m_layerBuild;
    [SerializeField] private LayerType m_layerBuild2;

    [SerializeField] private List<MeshRenderer> m_renderer = new List<MeshRenderer>();
    public bool IsBuildable => m_isBuildable;

    private void Start()
    {
        if (m_renderer.Count == 0)
        {
            m_renderer.Add(GetComponent<MeshRenderer>());
        }
        SetMaterial(m_greenMat);
    }
    private void Update()
    {
        CheckBuildable();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == (int)LayerType.Terrain)
        {
            m_haveGround.Add(other);
            return;
        }
        if (other.gameObject.layer == (int)LayerType.SnapPoint)
            return;
        if (other.gameObject.layer == (int)m_layerBuild)
        {
            m_haveGround.Add(other);
            return;
        }
        if (other.gameObject.layer == (int)m_layerBuild2)
        {
            m_haveGround.Add(other);
            return;
        }
        m_contacts.Add(other);
 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)LayerType.Terrain)
        {
            m_haveGround.Remove(other);
            return;
        }
        if (other.gameObject.layer == (int)LayerType.SnapPoint)
            return;
        if (other.gameObject.layer == (int)m_layerBuild)
        {
            m_haveGround.Remove(other);
            return;
        }
        if (other.gameObject.layer == (int)m_layerBuild2)
        {
            m_haveGround.Remove(other);
            return;
        }
        m_contacts.Remove(other);

    }
    
    private void CheckBuildable()
    {
        if (m_contacts.Count == 0 && m_haveGround.Count != 0)
        {
            m_isBuildable = true;
            SetMaterial(m_greenMat);
        }
        else
        {
            m_isBuildable = false;
            SetMaterial(m_redMat);
        }
    }


    private void SetMaterial(Material material)
    {
        for (int i = 0; i < m_renderer.Count; i++)
        {
            m_renderer[i].sharedMaterial = material;
        }
    }

}
