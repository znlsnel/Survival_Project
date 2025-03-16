using System;
using System.Collections;
using Actor;
// ReSharper disable once CheckNamespace
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Animator))]
    public class Animation: MonoBehaviour
    {
        [HideInInspector] public Animator animator;
        [HideInInspector] public Renderer mesh;

        // feat: 기본 3가지
        public static readonly int HashBoolRun = Animator.StringToHash("Run");
        public static readonly int HashBoolAttack = Animator.StringToHash("Attack");
        public static readonly int HashTriggerHit = Animator.StringToHash("Hit");

        void Awake()
        {
            animator = GetComponent<Animator>();
            Renderer mesh = GetComponent<Renderer>();

        }
        
        public Action<bool> WhenAttack;

        
        // IEnumerator ChangeColorCoroutine()
        // {
        //     // Renderer 컴포넌트를 가져와 원래 색상을 저장합니다.
        //     Color originalColor = mesh.material.color;
        //
        //     // 색상을 빨간색으로 변경합니다.
        //     mesh.material.color = Color.red;
        //
        //     // 0.5초 동안 대기합니다.
        //     yield return new WaitForSeconds(0.5f);
        //
        //     // 원래 색상으로 복구합니다.
        //     mesh.material.color = originalColor;
        // }
        //
        // public void ChangeHitColor()
        // {
        //     StartCoroutine(ChangeColorCoroutine());
        // }
    }
}