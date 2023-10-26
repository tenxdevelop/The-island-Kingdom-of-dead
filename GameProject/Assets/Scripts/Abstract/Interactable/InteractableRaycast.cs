using UnityEngine;

namespace TheIslandKOD
{
    public abstract class InteractableRaycast : MonoBehaviour
    {
        public string promptMessage;

        public void BaseInteract()
        {
            Interact();
        }
        protected virtual void Interact()
        {

        }
    }
}
