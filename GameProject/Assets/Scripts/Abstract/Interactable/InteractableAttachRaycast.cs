using UnityEngine;

namespace TheIslandKOD
{
    public enum LayerType
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Water = 4,
        UI = 5,
        Interactable = 6,
        Trees = 7
    }

    public abstract class InteractableAttachRaycast : MonoBehaviour
    {
       
        [SerializeField] private LayerType m_layer;
        public LayerType layer => m_layer;

        public void BaseInteract()
        {
            Interact();
        }
        protected virtual void Interact()
        {

        }
    }
}
