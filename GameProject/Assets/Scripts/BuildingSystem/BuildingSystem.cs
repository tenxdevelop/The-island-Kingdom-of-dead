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

        private Coroutine m_coroutine;
        private CinemachineVirtualCamera m_camera;
       
        private int m_layerIgnore = (int)Mathf.Pow(2, (int)LayerType.LastLayer) - 1;

        private RaycastHit m_hit;
        
        public BuildingSystem()
        {
            m_camera = ReferenceSystem.instance.player.GetComponent<PlayerLook>().Camera;
            m_inputManager = ReferenceSystem.instance.player.GetComponent<InputManager>();
            AddIgnoreLayer((int)LayerType.Interactable);
            AddIgnoreLayer((int)LayerType.Trees);
            AddIgnoreLayer((int)LayerType.Player);
            AddIgnoreLayer((int)LayerType.Water);
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

        protected virtual void OnBuildingUpdate()
        {

        }

        protected virtual void CreateBuildObject(RaycastHit hit)
        {

        }
        protected virtual void PlaceBuildObject(RaycastHit hit, Quaternion rotation)
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
        private void MoveBuildObject(RaycastHit hit)
        {
            if (m_currentBuildObject != null)
            {
                m_currentBuildObject.transform.position = hit.point + m_offsetBuildObject;
            }
        }
        private IEnumerator OnBuilding()
        {

            while (true)
            {
                BuildRay();
                if (m_currentBuildObject != null)
                {
                    if (m_inputManager.OnFoot.Attach.triggered && m_currentBuildObject.IsBuildable)
                    {
                        PlaceBuildObject(m_hit, m_currentBuildObject.transform.rotation);
                    }
                    if (m_inputManager.OnFoot.RotateBuild.triggered)
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
            Physics.Raycast(ray, out m_hit, 5, m_layerIgnore);
            if (m_hit.collider)
            {
                CreateBuildObject(m_hit);
            }
            else
            {
                DestroyBuildObject(m_hit);
            }

            MoveBuildObject(m_hit);
        }

        private void AddIgnoreLayer(int layer)
        {
            m_layerIgnore -= (int)Mathf.Pow(2, layer); 
        }

    }
}
