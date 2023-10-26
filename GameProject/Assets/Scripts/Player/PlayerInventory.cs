using System.Collections.Generic;
using UnityEngine;

namespace TheIslandKOD

{
    public class PlayerInventory 
    {
        public InventoryWithSlots inventory { get; }

        private int m_capacity;
        public PlayerInventory(int capacity)
        {
            m_capacity = capacity;

            inventory = new InventoryWithSlots(m_capacity);
        }
    }
}
