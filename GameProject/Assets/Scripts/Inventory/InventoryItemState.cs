using System;

namespace TheIslandKOD
{   
    
    [Serializable]
    public class InventoryItemState : IInventoryItemState
    {
        private int itemAmount;
        private bool isItemEquipped;
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

        public IInventoryItemState Clone()
        {
            return new InventoryItemState(itemAmount, isItemEquipped);
        }
    }
}
