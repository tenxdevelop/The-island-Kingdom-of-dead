using TheIslandKOD;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimation : MonoBehaviour
{
    private const string TAG_AMINATION_MOVE_X = "x";
    private const string TAG_AMINATION_MOVE_Y = "y";

    private const string TAG_AMINATION_JUMP = "jump";
    private const string TAG_AMINATION_ISGROUND = "isGround";

    private const string TAG_AMINATION_ATTACH_TOOLS = "RightAttach";
    private const string TAG_ANIMATION_ACTIVE_TOOLS = "RightArm";

    private const string TAG_ANIMATION_ACTIVE_BOW = "BowAimOverdraw";
    private const string TAG_ANIMATION_BOW_FIRE = "BowFire";
    private const string TAG_ANIMATION_BOW_AIM = "BowAim";

    private const string TAG_AMIMATION_RIFLE_ACTIVE = "RifleAimOverdraw";
    private const string TAG_AMIMATION_RIFLE_FIRE = "RifleFire";
    private const string TAG_ANIMATION_RIFLE_RELOAD = "RifleReload";

    [SerializeField] private MultiAimConstraint m_bodyRotation;
    [SerializeField] private MultiAimConstraint m_RightTriger;
    private Animator m_animatorPLayer;

    private void Start()
    {
        m_animatorPLayer = GetComponentInChildren<Animator>();
    }

    public void SetItemState(bool state, ItemType type)
    {
        
        switch (type)
        {
            case ItemType.Tools:
                m_animatorPLayer.SetBool(TAG_ANIMATION_ACTIVE_TOOLS, state);
                break;
            case ItemType.Weapon:
                m_animatorPLayer.SetBool(TAG_AMIMATION_RIFLE_ACTIVE, state);
                break;
            case ItemType.Bow:
                m_animatorPLayer.SetBool(TAG_ANIMATION_ACTIVE_BOW, state);
                break;
        }

    }


    public void Jump()
    {
        m_animatorPLayer.SetTrigger(TAG_AMINATION_JUMP);
    }
    public void UpdateJump(bool state)
    {
        m_animatorPLayer.SetBool(TAG_AMINATION_ISGROUND, state);
    }

    public void Move(float x, float y)
    {
        m_animatorPLayer.SetFloat(TAG_AMINATION_MOVE_X, x);
        m_animatorPLayer.SetFloat(TAG_AMINATION_MOVE_Y, y);
    }

    public void UpdateMove(Vector2 derection)
    {
        if (derection.x < 0.3 && derection.x > -0.3 && derection.y < 0.3 && derection.y > -0.3)
        {
            m_bodyRotation.weight = 1f;
        }
        else
        {
            m_bodyRotation.weight = 0f;
        }
    }

    public void RightAttachTools(bool state)
    {
        m_animatorPLayer.SetBool(TAG_AMINATION_ATTACH_TOOLS, state);
    }

    public void BowAimState(bool state)
    {
        m_animatorPLayer.SetBool(TAG_ANIMATION_BOW_AIM, state);
    }

    public void BowFire(bool isFire)
    {
        m_animatorPLayer.SetBool(TAG_ANIMATION_BOW_FIRE, isFire);
    }

    public void RifleFire(bool isFire)
    {
        m_animatorPLayer.SetBool(TAG_AMIMATION_RIFLE_FIRE, isFire);
    }

    public void ReflieReload()
    {
        m_animatorPLayer.SetTrigger(TAG_ANIMATION_RIFLE_RELOAD);
    }

}
