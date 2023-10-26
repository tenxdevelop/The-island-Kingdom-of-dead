using System;

namespace TheIslandKOD
{   
    
    [Serializable]
    public class InventoryItemState : IInfentoryItemState
    {
        public int itemAmount;
        public bool isItemEquipped;
        public bool isEquipped { get => isItemEquipped; set => isItemEquipped = value; }
        public int amount { get => itemAmount; set => itemAmount = value; }

        public InventoryItemState() 
        { 
            itemAmount = 0;
            isItemEquipped = false;
        }
        public InventoryItemState(int amount, bool isEquipped = false)
        {
            itemAmount = amount;
            isItemEquipped = isEquipped;
        }
    }
}
