
namespace TheIslandKOD
{
    public class ItemBow : ActiveItem
    {
  
        public ItemBow(IInventoryItemInfo info)
        {
            this.info = info;
            m_type = GetType();
            m_tagItemArm = "Bow";
            state = new InventoryItemState();
           
        }

        protected override void UpdateActiveItem()
        {          
            m_playerAnimation.BowFire(m_inputManager.OnFoot.Attach.triggered);
            m_playerAnimation.BowAimState(m_inputManager.OnFoot.BowAim.inProgress);
        }

        protected override IInventoryItem BaseClone()
        {
            var cloneItemBow = new ItemBow(info);
            cloneItemBow.state.amount = state.amount;
            return cloneItemBow;
        }

        protected override void OnDisableItem()
        {
            var BowAnimation = ReferenceSystem.instance.player.GetComponentInChildren<BowAnimationEvent>();
            BowAnimation.ResetString();
            BowAnimation.DisableArrow();
        }

        protected override void OnEnableItem()
        {
            
        }
    }
}