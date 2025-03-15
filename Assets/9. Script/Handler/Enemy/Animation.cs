using System;
// ReSharper disable once CheckNamespace
using UnityEngine;

namespace Enemy
{
    public class Animation: MonoBehaviour
    {
        [HideInInspector] public Animator animator;
    
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Move = Animator.StringToHash("Move");
        public static readonly int Hit = Animator.StringToHash("Hit");

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public Action<bool> WhenAttack;
    
        public void SetAttack(int isAttacking)
        {
            bool isValid = isAttacking != 0;
            WhenAttack?.Invoke(isValid);
        }
    }
}