using UnityEngine;

public class ChomperAnimationHandler: MonoBehaviour
{
    [HideInInspector] public Animator animator;
    
    // notice: static으로 관리하여 plyWeight를 통해 비용 절감 필요
    public static readonly int HashIsRunning = Animator.StringToHash("InPursuit");
    public static readonly int HashIsNearBase = Animator.StringToHash("NearBase");
    public static readonly int HashAttackTrigger = Animator.StringToHash("Attack");
    public static readonly int HashHit = Animator.StringToHash("Hit");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
}