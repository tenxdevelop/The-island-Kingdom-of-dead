using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    private List<Collider> contacts = new List<Collider>();

    private bool m_isBuildable = false;

    [SerializeField] private Material m_greenMat;
    [SerializeField] private Material m_redMat;
    [SerializeField] private LayerType m_IgnoreLayer;

    private MeshRenderer m_renderer;
    public bool IsBuildable => m_isBuildable;

    private void Start()
    {
        m_renderer = GetComponent<MeshRenderer>();   
        m_renderer.sharedMaterial = m_greenMat;
    }
    private void Update()
    {
        CheckBuildable();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == (int)LayerType.Terrain)
            return;
        if (other.gameObject.layer == (int)LayerType.SnapPoint)
            return;
        if (other.gameObject.layer == (int)m_IgnoreLayer)
            return;

        contacts.Add(other);
 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)LayerType.Terrain)
            return;
        if (other.gameObject.layer == (int)LayerType.SnapPoint)
            return;
        if (other.gameObject.layer == (int)m_IgnoreLayer)
            return;


        contacts.Remove(other);

    }
    
    private void CheckBuildable()
    {
        if (contacts.Count == 0)
        {
            m_isBuildable = true;
            m_renderer.sharedMaterial = m_greenMat;
        }
        else
        {
            m_isBuildable = false;
            m_renderer.sharedMaterial = m_redMat;
        }
    }
}
