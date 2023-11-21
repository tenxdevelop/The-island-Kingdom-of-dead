
namespace TheIslandKOD
{
    public class StoneAxe : ActiveItem
    {
        public StoneAxe(IInventoryItemInfo info) : base()
        {
            this.info = info;
            m_type = GetType();
            m_tagItemArm = "StoneAxe";
            state = new InventoryItemState();
        }

        protected override IInventoryItem BaseClone()
        {
            var cloneStoneAxe = new StoneAxe(info);
            cloneStoneAxe.state.amount = state.amount;
            return cloneStoneAxe;
        }

        protected override void UpdateActiveItem()
        {
           m_playerAnimation.RightAttachTools(m_inputManager.OnFoot.Attach.inProgress);   
        }

    }
}
