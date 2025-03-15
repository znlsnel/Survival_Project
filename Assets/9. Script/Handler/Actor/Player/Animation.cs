using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Animation: MonoBehaviour
    {
        [HideInInspector] public Animator animator;
        [FormerlySerializedAs("meeleStateMachine")] [HideInInspector] public MeleeAction meleeStateMachine;
    
        public static readonly int IsGrounded = Animator.StringToHash("Grounded");
        public static readonly int HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
    
        // notice: 공격을 당한 경우, 좌표를 계산해서 공격 당함
        public static readonly int HashHurtTrigger = Animator.StringToHash("Hurt");
        public static readonly int HashDeath = Animator.StringToHash("Death");
    
        // 근접 공격 트리거
        public static readonly int MeleeAttackTrigger = Animator.StringToHash("MeleeAttack");
        // idle 상태 종료
        public static readonly int TimeoutToIdleTrigger = Animator.StringToHash("TimeoutToIdle");
        public static readonly int BreakIdleTrigger = Animator.StringToHash("BreakIdle");

        // 콤보 연계를 위한 시간 체크
        public static readonly int HashStateTime = Animator.StringToHash("StateTime");
        public static readonly int HashIsAbleRegisterCombo = Animator.StringToHash("IsAbleRegisterCombo");

        public static readonly int HashEllenCombo1 = Animator.StringToHash("EllenCombo1");
        public static readonly int HashEllenCombo2 = Animator.StringToHash("EllenCombo2");
        public static readonly int HashEllenCombo3 = Animator.StringToHash("EllenCombo3");
        public static readonly int HashEllenCombo4 = Animator.StringToHash("EllenCombo4");

        void Awake()
        {
            animator = GetComponent<Animator>();
            meleeStateMachine = animator.GetBehaviour<MeleeAction>();
        }


        public Action<bool> OnMeleeAttackAvailable;
    
        // notice: animation event에선 bool을 파라미터로 받지 못함
        public void SetIsMeleeAttackAvailable(int isActive)
        {
            bool isAvailable = isActive != 0;
            OnMeleeAttackAvailable?.Invoke(isAvailable);
        }
    }
}
