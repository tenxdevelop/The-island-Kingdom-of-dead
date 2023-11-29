using System;
using System.Collections;
using UnityEngine;

namespace TheIslandKOD
{
    public abstract class ActiveItem : IInventoryItem
    {

        public IInventoryItemInfo info { get; protected set; }

        public IInventoryItemState state { get; protected set; }

        protected string m_tagItemArm;
        protected UIQuickSlot m_uIQuickSlot;
        protected PlayerAnimation m_playerAnimation;
        protected PlayerItemArm m_playerItemArm;
        protected InputManager m_inputManager;
        protected Type m_type;
        private Coroutine m_coroutine;

        protected GameObject m_ArmItem;
        public Type type => m_type;
        public ActiveItem()
        {
            m_uIQuickSlot = UIQuickSlot.instance;
            m_playerAnimation = ReferenceSystem.instance.player.GetComponent<PlayerAnimation>();
            m_playerItemArm = ReferenceSystem.instance.player.GetComponent<PlayerItemArm>();
            m_inputManager = ReferenceSystem.instance.player.GetComponent<InputManager>();

        }
        protected abstract IInventoryItem BaseClone();
        protected void StartCoroutine()
        {
            if (m_coroutine == null)
            {
                m_coroutine = CoroutineSystem.StartRoutine(CoroutineMethod());
            }

        }
        public void OnDisable()
        {
            m_uIQuickSlot.OnQuickSlotActiveChangedEvent -= OnQuickSlotChangedEvent;
            m_ArmItem = null;
        }

        public void OnEnable()
        {
            m_uIQuickSlot.OnQuickSlotActiveChangedEvent += OnQuickSlotChangedEvent;
            if (m_ArmItem == null)
            {
                m_ArmItem = m_playerItemArm.ItemArm.Find(i => i.key == m_tagItemArm).item;
            }
        }

        private void OnQuickSlotChangedEvent(InventoryWithSlots inventory, IInventorySlot slot, bool isActive)
        {

            if (isActive)
            {
                if (slot.itemType == type)
                {
                    m_playerAnimation.SetItemState(true, slot.item.info.itemType);
                    m_ArmItem.SetActive(true);
                    OnEnableItem();
                    StartCoroutine();
                }                
            }
            else
            {
                
                DisableSlot(slot);
            }

        }

        protected void StopCoroutine()
        {
            CoroutineSystem.StopRoutine(m_coroutine);
            m_coroutine = null;
        }

        private IEnumerator CoroutineMethod()
        {
            while (true)
            {
                UpdateActiveItem();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        private void DisableSlot(IInventorySlot slot)
        {
            m_playerAnimation.SetItemState(false, slot.item.info.itemType);
            m_ArmItem.SetActive(false);
            OnDisableItem();
            StopCoroutine();
        }

        protected abstract void UpdateActiveItem();
        protected abstract void OnDisableItem();
        protected abstract void OnEnableItem();
        public IInventoryItem Clone()
        {
            return BaseClone();
        }

    }
}