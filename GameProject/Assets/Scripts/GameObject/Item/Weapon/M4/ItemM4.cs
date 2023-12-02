using UnityEngine;

namespace TheIslandKOD
{
    public class ItemM4 : ActiveItem
    {
        private PlayerLook m_playerLook;
        public ItemM4(IInventoryItemInfo info)
        {
            this.info = info;
            m_type = GetType();
            m_tagItemArm = "M4";
            state = new InventoryItemState();
            m_playerLook = ReferenceSystem.instance.player.GetComponent<PlayerLook>();
            
        }

        protected override void UpdateActiveItem()
        {
            m_playerAnimation.RifleFire(m_inputManager.OnFoot.Attach.inProgress);
            if (m_inputManager.OnFoot.RotateBuild.triggered)
            {
                m_playerAnimation.ReflieReload();
            }
        }

        protected override IInventoryItem BaseClone()
        {
            var cloneItemBow = new ItemM4(info);
            cloneItemBow.state.amount = state.amount;
            return cloneItemBow;
        }

        protected override void OnDisableItem()
        {
            RifleAnimationEvent.instance.OnFireEvent -= Fire;
        }

        protected override void OnEnableItem()
        {
            RifleAnimationEvent.instance.m_clipFire = SoundSystem.instance.rifleFires.Find(i => i.key == "m4").clip;
            RifleAnimationEvent.instance.OnFireEvent += Fire;
        }


        private void Fire()
        {

        }

    }
}
