using System;
using UnityEngine;

// 추상화할 수 있는 부분 없음
public class ChomperAnimationHandler: MonoBehaviour
{
    [HideInInspector] public Animator animator;
    
    // notice: static으로 관리하여 plyWeight를 통해 비용 절감 필요
    public static readonly int HashIsRunning = Animator.StringToHash("InPursuit");
    public static readonly int HashIsNearBase = Animator.StringToHash("NearBase");
    public static readonly int HashAttackTrigger = Animator.StringToHash("Attack");
    public static readonly int HashHitTrigger = Animator.StringToHash("Hit");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Action<bool> OnIsAttacking;
    
    public void IsAttacking(int isAttacking)
    {
        bool isValid = isAttacking != 0;
        OnIsAttacking?.Invoke(isValid);
    }
}