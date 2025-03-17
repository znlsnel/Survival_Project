using UnityEngine;

namespace Enemy
{
    public class AnimationMachine : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // problem: 애니메이션 이름을 직접 할당 중
            if (!animator.TryGetComponent(out AnimationHandler animation) || !stateInfo.IsName("Attack")) return;
            animation.WhenAttack?.Invoke(true);
            animator.GetComponent<NavigationHandler>().SetAttacking(true);
        }


        private int prevLoopCount = 0;
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent(out AnimationHandler animation) || !stateInfo.IsName("Attack")) return;
            
            int currentLoopCount = Mathf.FloorToInt(stateInfo.normalizedTime);
            if (currentLoopCount > prevLoopCount)
            {
                prevLoopCount = currentLoopCount;
                animation.WhenAttack?.Invoke(false);
                animator.GetComponent<NavigationHandler>().SetAttacking(false);
            }

        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.IsName("Die"))
            {
                Destroy(animator.gameObject);
                return;
            }
            
            if (!animator.TryGetComponent(out AnimationHandler animation) || !stateInfo.IsName("Attack")) return;
            animation.WhenAttack?.Invoke(false);
            animator.GetComponent<NavigationHandler>().SetAttacking(false);
            
        }
    }
}