using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimation : MonoBehaviour
{
    private const string TAG_AMINATION_MOVE_X = "x";
    private const string TAG_AMINATION_MOVE_Y = "y";

    private const string TAG_AMINATION_JUMP = "jump";
    private const string TAG_AMINATION_ISGROUND = "isGround";

    private const string TAG_AMINATION_ATTACH = "RightAttach";

    [SerializeField] private MultiAimConstraint m_bodyRotation;
    [SerializeField] private MultiAimConstraint m_RightTriger;
    private Animator m_animatorPLayer;

    private void Start()
    {
        m_animatorPLayer = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        
    }

    public void SetItemState(bool state)
    {
        if (state)
        {     
            m_animatorPLayer.SetLayerWeight(1, 1);
        }
        else
        {
            m_RightTriger.weight = 0f;
            m_animatorPLayer.SetLayerWeight(1, 0);
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

    public void RightAttach(bool state)
    {
        m_animatorPLayer.SetBool(TAG_AMINATION_ATTACH, state);
    }
}
