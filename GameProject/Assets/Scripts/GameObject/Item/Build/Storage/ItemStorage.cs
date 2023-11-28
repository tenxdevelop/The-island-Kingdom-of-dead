using System;
using UnityEngine;

namespace TheIslandKOD
{

    public class ItemStorage : BuildingSystem, IInventoryItem
    {
        private const string TAG_BUILD_OBJECT = "storage";
        public IInventoryItemInfo info { get; }

        public IInventoryItemState state { get; }

        public Type type => GetType();

        private UIQuickSlot m_uIQuickSlot;

        private BuildObjectType m_buildStorage;

        private InventoryWithSlots m_inventory;
        public ItemStorage(IInventoryItemInfo info)
        {
            this.info = info;
            state = new InventoryItemState();
            m_uIQuickSlot = UIQuickSlot.instance;
            m_buildStorage = info.buildObjects.Find(i => i.tagObject == TAG_BUILD_OBJECT);
            m_offsetBuildObject = m_buildStorage.objectOffset;
            m_rotationBuildObject = m_buildStorage.objectRotation;
            AddIgnoreLayer((int)LayerType.SnapPoint);
            GetResetLayerRayCast();
        }

        public IInventoryItem Clone()
        {
            var clone = new ItemStorage(info);
            clone.state.amount = state.amount;
            return clone;
        }

        public void OnDisable()
        {
            m_uIQuickSlot.OnQuickSlotActiveChangedEvent -= OnQuickSlotChangedEvent;
        }

        public void OnEnable()
        {
            m_uIQuickSlot.OnQuickSlotActiveChangedEvent += OnQuickSlotChangedEvent;
        }

        protected override void CreateBuildObject(RaycastHit hit)
        {
            if (m_currentBuildObject == null)
            {
                m_currentBuildObject = GameObject.Instantiate(m_buildStorage.buildObject).GetComponentInChildren<BuildObject>(); 
            }
        }

        protected override void DestroyBuildObject(RaycastHit hit)
        {
            base.DestroyBuildObject(hit);
        }
        protected override void PlaceBuildObject(Vector3 position, Quaternion rotation)
        {
            GameObject.Instantiate(m_buildStorage.prefab, position, rotation);
            StopCoroutine();
            m_inventory.Remove(this, type);
        }
        private void OnQuickSlotChangedEvent(InventoryWithSlots inventory, IInventorySlot slot, bool isActive)
        {
            if (isActive)
            {
                if (slot.itemType == type)
                {
                    StartCoroutine();
                    m_inventory = inventory;
                }
            }
            else
            {
                StopCoroutine();
                m_inventory = null;
            }
        }

    }
}
