using TheIslandKOD;
using UnityEngine;

public class InteractableItemState : MonoBehaviour
{
    public IInventoryItemState state;

    private void Awake()
    {
        if (state == null)
        {
            state = new InventoryItemState(5);
        }
    }
}
