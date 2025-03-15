using System;
// ReSharper disable once CheckNamespace
using UnityEngine;

namespace Enemy
{
    public class Animation: MonoBehaviour
    {
        [HideInInspector] public Animator animator;

        // feat: 기본 3가지
        public static readonly int HashBoolRun = Animator.StringToHash("Run");
        public static readonly int HashBoolAttack = Animator.StringToHash("Attack");
        public static readonly int HashTriggerHit = Animator.StringToHash("Hit");

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        
        public Action<bool> WhenAttack;
    }
}