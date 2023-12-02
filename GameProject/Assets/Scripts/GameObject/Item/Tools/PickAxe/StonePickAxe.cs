using UnityEngine;

namespace TheIslandKOD
{
    public class StonePickAxe : ActiveItem
    {
        public StonePickAxe(IInventoryItemInfo info) : base()
        {
            this.info = info;
            m_type = GetType();
            m_tagItemArm = "StonePickAxe";
            state = new InventoryItemState();
        }

        protected override IInventoryItem BaseClone()
        {
            var cloneStonePickAxe = new StonePickAxe(info);
            cloneStonePickAxe.state.amount = state.amount;
            return cloneStonePickAxe;
        }

        protected override void OnDisableItem()
        {

        }

        protected override void OnEnableItem()
        {

        }

        protected override void UpdateActiveItem()
        {
            m_playerAnimation.RightAttachTools(m_inputManager.OnFoot.Attach.inProgress);
        }

    }
}
