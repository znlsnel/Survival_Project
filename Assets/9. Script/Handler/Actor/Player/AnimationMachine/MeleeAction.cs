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
            animator.GetComponent<Movement>().isComboAble = true;
        }
    }
}
