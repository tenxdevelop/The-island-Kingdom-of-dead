
namespace TheIslandKOD
{
    public class ItemBow : ActiveItem
    {
        private PoolArrow m_poolArrow;
        private PlayerLook m_playerLook;
        public ItemBow(IInventoryItemInfo info)
        {
            this.info = info;
            m_type = GetType();
            m_tagItemArm = "Bow";
            state = new InventoryItemState();
            m_poolArrow = PoolArrow.instance;
            m_playerLook = ReferenceSystem.instance.player.GetComponent<PlayerLook>();
        }

        protected override void UpdateActiveItem()
        {          
            m_playerAnimation.BowFire(m_inputManager.OnFoot.Attach.triggered);
            m_playerAnimation.BowAimState(m_inputManager.OnFoot.BowAim.inProgress);
            if (m_inputManager.OnFoot.Attach.triggered)
            {
                var currentArrow = m_poolArrow.CreateArrow();
                if (currentArrow != null)
                {
                    currentArrow.transform.position = m_playerLook.Camera.transform.position;
                }
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
            var BowAnimation = ReferenceSystem.instance.player.GetComponentInChildren<BowAnimationEvent>();
            BowAnimation.ResetString();
            BowAnimation.DisableArrow();
        }

        protected override void OnEnableItem()
        {
            
        }
    }
}