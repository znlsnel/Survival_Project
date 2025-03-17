using System;
using System.Collections;
using Actor;
// ReSharper disable once CheckNamespace
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Animator))]
    public class AnimationHandler: MonoBehaviour
    {
        [HideInInspector] public Animator animator;
        [HideInInspector] public Renderer mesh;

        // feat: 기본 3가지
        public static readonly int HashBoolRun = Animator.StringToHash("Run");
        public static readonly int HashBoolAttack = Animator.StringToHash("Attack");
        public static readonly int HashTriggerHit = Animator.StringToHash("Hit");
        public static readonly int HashTriggerDeath = Animator.StringToHash("Death");

        void Awake()
        {
            animator = GetComponent<Animator>();
            Renderer mesh = GetComponent<Renderer>();

        }
        
        public Action<bool> WhenAttack;
    }
}