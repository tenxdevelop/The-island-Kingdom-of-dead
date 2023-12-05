using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace TheIslandKOD
{
    public class ItemM4 : ActiveItem
    {
        private PlayerLook m_playerLook;

        private m4Particle m_particle;
        private PlayerInventory m_playerInventory;
        private PoolBullet m_poolBullet;

        private int m_amountBulletReload;

        private int m_maxBulletInRifle = 30;
        private int m_currentHaveBulletInRifle;
        public ItemM4(IInventoryItemInfo info)
        {
            this.info = info;
            m_type = GetType();
            m_tagItemArm = "M4";
            state = new InventoryItemState();
            m_playerLook = ReferenceSystem.instance.player.GetComponent<PlayerLook>();
            m_playerInventory = ReferenceSystem.instance.player.GetComponent<PlayerInventory>();
            m_poolBullet = PoolBullet.instance;
            m_currentHaveBulletInRifle = 0;
        }

        protected override void UpdateActiveItem()
        {
            if (m_currentHaveBulletInRifle > 0)
            {
                m_playerAnimation.RifleFire(m_inputManager.OnFoot.Attach.inProgress);
            }
            else
            {
                m_playerAnimation.RifleFire(false);
            }

            if (m_inputManager.OnFoot.RotateBuild.triggered && m_currentHaveBulletInRifle != m_maxBulletInRifle)
            {
                var haveBullet = m_playerInventory.inventory.GetItemAmount(typeof(ItemBullet));
                if (haveBullet > 0)
                {
                    if (m_currentHaveBulletInRifle + haveBullet < m_maxBulletInRifle)
                    {
                        m_amountBulletReload = m_currentHaveBulletInRifle + haveBullet;
                    }
                    else
                    {
                        m_amountBulletReload = m_maxBulletInRifle;
                    }
                    m_playerAnimation.ReflieReload();
                }
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
            RifleAnimationEvent.instance.OnReloadedEvent -= Reloaded;
        }

        protected override void OnEnableItem()
        {
            RifleAnimationEvent.instance.m_clipFire = SoundSystem.instance.rifleFires.Find(i => i.key == "m4").clip;
            RifleAnimationEvent.instance.OnFireEvent += Fire;
            RifleAnimationEvent.instance.OnReloadedEvent += Reloaded;
            m_particle = m_ArmItem.GetComponent<m4Particle>();
        }


        private void Fire()
        {
            var currentBullet = m_poolBullet.CreateBullet();
            if (currentBullet != null)
            {
                m_currentHaveBulletInRifle -= 1;
                m_particle.Activate();
                currentBullet.transform.position = m_playerLook.Camera.transform.position + m_playerLook.Camera.transform.forward;
                currentBullet.transform.localRotation = m_playerLook.Camera.transform.rotation * Quaternion.Euler(90, 0, 0);
                currentBullet.Fire(m_playerLook.Camera.transform.forward);
            }
        }

        private void Reloaded()
        {
            m_playerInventory.inventory.Remove(this, typeof(ItemBullet), (m_amountBulletReload - m_currentHaveBulletInRifle));
            m_currentHaveBulletInRifle = m_amountBulletReload;
        }

    }
}
