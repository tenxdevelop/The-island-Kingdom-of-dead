using System.Collections;
using UnityEngine;

namespace TheIslandKOD
{
    public class BuildingSystem
    {
        private Coroutine m_coroutine;

        public BuildingSystem()
        {

        }

        public void StartCoroutine()
        {
            if (m_coroutine == null)
            {
                m_coroutine = CoroutineSystem.StartRoutine(OnBuilding());
            }

        }

        public void StopCoroutine()
        {
            CoroutineSystem.StopRoutine(m_coroutine);
            m_coroutine = null;
        }

        private IEnumerator OnBuilding()
        {
            
            Debug.Log("Build");
            yield return new WaitForSeconds(Time.deltaTime);
   
        }

    }
}
