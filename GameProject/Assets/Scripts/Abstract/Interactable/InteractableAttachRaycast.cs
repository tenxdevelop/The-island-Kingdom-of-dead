using UnityEngine;

namespace TheIslandKOD
{
    public abstract class InteractableAttachRaycast : MonoBehaviour
    {
       
        [SerializeField] private LayerType m_layer;
        [SerializeField] private GameObject m_effectAttack;
        public LayerType layer => m_layer;

        public void BaseInteract()
        {
            Interact();
        }
        public void BaseEffects(Vector3 position, Quaternion rotation)
        {
            OnEffects(position, rotation);
        }
        protected virtual void Interact()
        {

        }

        protected virtual void OnEffects(Vector3 position, Quaternion rotation)
        {
            if (m_effectAttack != null)
            {
                Instantiate(m_effectAttack, position, rotation);
            }
        }

    }
}
