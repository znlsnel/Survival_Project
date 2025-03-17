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
            var movement = animator.GetComponent<Movement>();
            movement.Stop();
            movement.isMoveable = false;
        }
    
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.GetComponent<Movement>().isMoveable = true;
        }
    
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var movement = animator.GetComponent<Movement>();
            movement.isAttacking = true;
            movement.isComboAble = true;
            
            animator.GetComponent<Animation>().WhenAttack?.Invoke(true);
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var movement = animator.GetComponent<Movement>();
            Debug.Log(1);
            movement.isAttacking = false;
            
            animator.GetComponent<Animation>().WhenAttack?.Invoke(false);
        }
    }
}
