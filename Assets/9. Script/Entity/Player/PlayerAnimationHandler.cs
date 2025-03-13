using UnityEngine;

public class PlayerAnimationHandler: MonoBehaviour
{
    [HideInInspector] public Animator animator;
    
    public readonly int IsGrounded = Animator.StringToHash("Grounded");
    public readonly int HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
    public readonly int HashHurt = Animator.StringToHash("Hurt");
    public readonly int HashDeath = Animator.StringToHash("Death");
    
    // 근접 공격 트리거
    public readonly int MeleeAttackTrigger = Animator.StringToHash("MeleeAttack");
    // idle 상태 종료
    public readonly int TimeoutToIdleTrigger = Animator.StringToHash("TimeoutToIdle");
    public readonly int BreakIdleTrigger = Animator.StringToHash("BreakIdle");
    
    public readonly int HashEllenCombo1 = Animator.StringToHash("EllenCombo1");
    public readonly int HashEllenCombo2 = Animator.StringToHash("EllenCombo2");
    public readonly int HashEllenCombo3 = Animator.StringToHash("EllenCombo3");
    public readonly int HashEllenCombo4 = Animator.StringToHash("EllenCombo4");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
}