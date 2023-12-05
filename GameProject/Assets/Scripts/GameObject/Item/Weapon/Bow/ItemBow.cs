using UnityEngine;

namespace TheIslandKOD
{
    public class ItemBow : ActiveItem
    {
        private PoolArrow m_poolArrow;
        private PlayerLook m_playerLook;
        private PlayerInventory m_playerInventory;
        public ItemBow(IInventoryItemInfo info)
        {
            this.info = info;
            m_type = GetType();
            m_tagItemArm = "Bow";
            state = new InventoryItemState();
            m_poolArrow = PoolArrow.instance;
            m_playerLook = ReferenceSystem.instance.player.GetComponent<PlayerLook>();
            m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
        }

           


        protected override void UpdateActiveItem()
        {
            var haveItemArrow = m_playerInventory.inventory.GetItemAmount(typeof(ItemArrow));
            if (haveItemArrow > 0)
            {
                m_playerAnimation.BowAimState(m_inputManager.OnFoot.BowAim.inProgress);
                m_playerAnimation.BowFire(m_inputManager.OnFoot.Attach.triggered);
            }
            else
            {
                m_playerAnimation.BowAimState(false);
            }
        }

        protected override IInventoryItem BaseClone()
        {
            var cloneItemBow = new ItemBow(info);
            cloneItemBow.state.amount = state.amount;
            return cloneItemBow;
        }

        protected override void OnDisableItem()
        {
            BowAnimationEvent.OnFireEvent -= BowFire;

            var BowAnimation = ReferenceSystem.instance.player.GetComponentInChildren<BowAnimationEvent>();         
            BowAnimation.ResetString();
            BowAnimation.DisableArrow();
        }

        protected override void OnEnableItem()
        {
            BowAnimationEvent.OnFireEvent += BowFire;
        }

        private void BowFire()
        {
            var currentArrow = m_poolArrow.CreateArrow();
            if (currentArrow != null)
            {
                m_playerInventory.inventory.Remove(this, typeof(ItemArrow), 1);
                currentArrow.transform.position = m_playerLook.Camera.transform.position + m_playerLook.Camera.transform.forward;
                currentArrow.transform.localRotation = m_playerLook.Camera.transform.rotation * Quaternion.Euler(90, 0, 0);
                currentArrow.Fire(m_playerLook.Camera.transform.forward);
            }
        }
    }
}