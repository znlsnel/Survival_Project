using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class MeleeAction: StateMachineBehaviour
    {
        public bool isInStateMachine = false;
        public bool isAnimating = false;
    
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            var movement = animator.GetComponent<MovementHandler>();
            movement.Stop();
            movement.isMoveable = false;
        }
    
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.GetComponent<MovementHandler>().isMoveable = true;
            // 콤보 이후 공격 모션 일시 방지
            animator.GetComponent<AnimationHandler>().animator.ResetTrigger(AnimationHandler.MeleeAttackTrigger);
        }
    
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var movement = animator.GetComponent<MovementHandler>();
            movement.isAttacking = true;
            
            animator.GetComponent<AudioHandler>().PlayRandomSound(PlayerSoundType.Attack);
            animator.GetComponent<AnimationHandler>().WhenAttack?.Invoke(true);
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var movement = animator.GetComponent<MovementHandler>();
            movement.isAttacking = false;
            
            animator.GetComponent<AnimationHandler>().WhenAttack?.Invoke(false);
        }
    }
}
