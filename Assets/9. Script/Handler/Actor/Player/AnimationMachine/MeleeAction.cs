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
        }
    
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var movement = animator.GetComponent<MovementHandler>();
            movement.isAttacking = true;
            movement.isComboAble = true;
            
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
