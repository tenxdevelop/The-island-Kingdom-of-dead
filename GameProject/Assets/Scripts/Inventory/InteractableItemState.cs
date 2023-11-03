using TheIslandKOD;
using UnityEngine;

public class InteractableItemState : MonoBehaviour
{
    public IInventoryItemState state;
    public IInventoryItem item;

    private void Awake()
    {
        if (state == null)
        {
            state = new InventoryItemState(5);
        }

    }
}
