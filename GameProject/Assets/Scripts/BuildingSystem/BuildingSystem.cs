using Cinemachine;
using System.Collections;
using UnityEngine;

namespace TheIslandKOD
{
    public class BuildingSystem
    {

        protected InputManager m_inputManager;
        protected BuildObject m_currentBuildObject;
        protected Vector3 m_offsetBuildObject;
        protected Vector3 m_rotationBuildObject;
        protected SnapPointType m_currentBuildSnap;

        protected bool m_canRotate = true;
        protected int currentIgnoreLayer => m_currentIgnoreLayer;

        private Vector3 m_snapPosition;
        private bool m_snap;

        private Coroutine m_coroutine;
        private CinemachineVirtualCamera m_camera;
       
        private int m_layerIgnore = (int)Mathf.Pow(2, (int)LayerType.LastLayer) - 1;

        private int m_currentIgnoreLayer;

        private RaycastHit m_hit;
        
        public BuildingSystem()
        {
            m_camera = ReferenceSystem.instance.player.GetComponent<PlayerLook>().Camera;
            m_inputManager = ReferenceSystem.instance.player.GetComponent<InputManager>();
            AddIgnoreLayer((int)LayerType.Interactable);
            AddIgnoreLayer((int)LayerType.Trees);
            AddIgnoreLayer((int)LayerType.Player);
            AddIgnoreLayer((int)LayerType.Water);
            GetResetLayerRayCast();
        }

        protected void StartCoroutine()
        {
            if (m_coroutine == null)
            {
                m_coroutine = CoroutineSystem.StartRoutine(OnBuilding());
            }

        }

        protected void StopCoroutine()
        {
            CoroutineSystem.StopRoutine(m_coroutine);
            DestroyBuildObject(m_hit);
            m_coroutine = null;
        }

        protected void ReCreateBuildObject()
        {
            DestroyBuildObject(m_hit);
            CreateBuildObject(m_hit);
        }
        protected virtual void OnBuildingUpdate()
        {

        }

        protected virtual void CreateBuildObject(RaycastHit hit)
        {

        }
        protected virtual void PlaceBuildObject(Vector3 position, Quaternion rotation)
        {

        }

        protected virtual void RotateBuildObject()
        {
            m_currentBuildObject.transform.rotation *= Quaternion.Euler(m_rotationBuildObject);
        }

        protected virtual void DestroyBuildObject(RaycastHit hit)
        {
            if (m_currentBuildObject != null)
            {
                GameObject.Destroy(m_currentBuildObject.gameObject);
                m_currentBuildObject = null;
            }
            
        }
        private void MoveBuildObject(Vector3 position)
        {
            if (m_currentBuildObject != null)
            {

                m_currentBuildObject.transform.position = position;

            }
        }
        private IEnumerator OnBuilding()
        {

            while (true)
            {
                BuildRay();
                Snapping();
                MoveBuildObject(m_snapPosition);
                if (m_currentBuildObject != null)
                {
                    
                    if (m_inputManager.OnFoot.Attach.triggered && (m_currentBuildObject.IsBuildable))
                    {
                        PlaceBuildObject(m_snapPosition, m_currentBuildObject.transform.rotation);
                    }
                    if (m_inputManager.OnFoot.RotateBuild.triggered && m_canRotate)
                    {
                        RotateBuildObject();
                    }
                }
                OnBuildingUpdate();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        private void BuildRay()
        {
            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
            Physics.Raycast(ray, out m_hit, 5, m_currentIgnoreLayer);
            if (m_hit.collider)
            {
                CreateBuildObject(m_hit);
            }
            else
            {
                DestroyBuildObject(m_hit);
            }            
        }


        private void Snapping()
        {
            if (m_hit.collider && m_hit.collider.gameObject.layer == (int)LayerType.SnapPoint)
            {
                SnapPoint snapPoint = m_hit.collider.GetComponent<SnapPoint>();
                if (snapPoint)
                {
                    m_snap = true;
                    m_snapPosition = snapPoint.GetPosition(m_currentBuildSnap).transform.position;
                    m_currentBuildObject.transform.rotation = snapPoint.GetPosition(m_currentBuildSnap).transform.localRotation;
                }
                else
                {
                    m_snap = false;
                    m_snapPosition = m_hit.point;
                }
            }
            else
            {
                m_snap = false;
                m_snapPosition = m_hit.point;
            }

            if (!m_snap)
            {
                m_snapPosition += m_offsetBuildObject;     
            }
            
        }
        
        protected void GetResetLayerRayCast()
        {
            m_currentIgnoreLayer = m_layerIgnore;
        }

        protected void SetLayerRayCast(int layer)
        {
            m_currentIgnoreLayer = layer;
        }

        protected void AddIgnoreLayer(int layer)
        {
            m_layerIgnore -= (int)Mathf.Pow(2, layer); 
        }

    }
}
