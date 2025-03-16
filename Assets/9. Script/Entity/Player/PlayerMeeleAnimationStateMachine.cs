using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMeeleAnimationStateMachine: StateMachineBehaviour
{
    public bool isInStateMachine = false;
    public bool isAnimating = false;
    
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        isInStateMachine = true;
    }
    
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        isInStateMachine = false;
        animator.SetBool(PlayerAnimationHandler.HashIsAbleRegisterCombo, true);
    }
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isAnimating = true;
        animator.SetBool(PlayerAnimationHandler.HashIsAbleRegisterCombo, true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isAnimating = false;
    }
}