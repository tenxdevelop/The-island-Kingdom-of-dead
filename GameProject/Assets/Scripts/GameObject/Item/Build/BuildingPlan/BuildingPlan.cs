using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheIslandKOD
{
    public class BuildingPlan : BuildingSystem, IInventoryItem
    {
        private const string TAG_BUILD_OBJECT_WALL = "wall";
        private const string TAG_BUILD_OBJECT_FOUNDATION = "foundation";

        public IInventoryItemInfo info { get; }

        public IInventoryItemState state { get; }

        public Type type => GetType();

        private UIQuickSlot m_uIQuickSlot;

        private List<BuildObjectType> m_buildObjects;

        private BuildObjectType m_currentBuildObjectType;

        private InventoryWithSlots m_inventory;

        private int m_currentBuildType = 0;

        public BuildingPlan(IInventoryItemInfo info)
        {
            this.info = info;
            state = new InventoryItemState();
            m_uIQuickSlot = UIQuickSlot.instance;
            m_buildObjects = info.buildObjects;
            m_currentBuildObjectType = m_buildObjects[m_currentBuildType];
            m_offsetBuildObject = m_currentBuildObjectType.objectOffset;
            m_rotationBuildObject = m_currentBuildObjectType.objectRotation;
            m_currentBuildSnap = m_currentBuildObjectType.snapPointType;
            AddIgnoreLayer((int)LayerType.Build);
        }

        public IInventoryItem Clone()
        {
            var clone = new BuildingPlan(info);
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

        protected override void OnBuildingUpdate()
        {
            if (m_inputManager.OnFoot.NextBuild.triggered)
            {
                m_currentBuildType += 1;
                if(m_currentBuildType >= m_buildObjects.Count)
                    m_currentBuildType = 0;
                m_currentBuildObjectType = m_buildObjects[m_currentBuildType];
                m_offsetBuildObject = m_currentBuildObjectType.objectOffset;
                m_rotationBuildObject = m_currentBuildObjectType.objectRotation;
                m_currentBuildSnap = m_currentBuildObjectType.snapPointType;
            }
        }

        protected override void CreateBuildObject(RaycastHit hit)
        {
            if (m_currentBuildObject == null)
            {
                m_currentBuildObject = GameObject.Instantiate(m_currentBuildObjectType.buildObject).GetComponentInChildren<BuildObject>();
            }
        }

        protected override void DestroyBuildObject(RaycastHit hit)
        {
            base.DestroyBuildObject(hit);
        }
        protected override void PlaceBuildObject(Vector3 position, Quaternion rotation)
        {
            GameObject.Instantiate(m_currentBuildObjectType.prefab, position, rotation);
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