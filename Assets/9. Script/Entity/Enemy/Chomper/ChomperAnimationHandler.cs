using UnityEngine;

public class ChomperAnimationHandler: MonoBehaviour
{
    public Animator animator;
    
    public static readonly int HashInPursuit = Animator.StringToHash("InPursuit");
    public static readonly int HashAttack = Animator.StringToHash("Attack");
    public static readonly int HashHit = Animator.StringToHash("Hit");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
}