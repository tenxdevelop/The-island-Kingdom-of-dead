using System.Collections.Generic;
using UnityEngine;

namespace TheIslandKOD

{
    public class PlayerInventory : MonoBehaviour
    {
        
        [SerializeField] private int m_capacity;

        public InventoryWithSlots inventory { get; private set; }
   
        private void Start()
        {
            inventory = new InventoryWithSlots(m_capacity);
        }
        
    }
}
