using UnityEngine;

namespace Enemy
{
    public class AnimationMachine : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // problem: 애니메이션 이름을 직접 할당 중
            if (!animator.TryGetComponent(out Animation animation) || !stateInfo.IsName("Attack(Hand)")) return;
            animation.WhenAttack?.Invoke(true);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent(out Animation animation) || !stateInfo.IsName("Attack(Hand)")) return;
            animation.WhenAttack?.Invoke(false);
        }
    }
}