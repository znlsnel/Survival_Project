using System;
using System.Collections;
using Actor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.Skeleton
{
    // [RequireComponent(typeof(AnimationHandler), typeof(AudioHandler), typeof(MovementHandler))]
    // [RequireComponent(typeof(NavigationHandler), typeof(ResourceHandler), typeof(RewardHandler))]
    public class Controller : Enemy.Controller
    {
        protected override void Start()
        {
            AnimationHandler.WhenAttack += navigationHandler.StopByAnimation;
            
            navigationHandler.WhenChangedStatus += (state) =>
            {
                // warn: 자칫 애니메이션 빠져나가는 현상 발생할 수 있음
                AnimationHandler.animator.SetBool(AnimationHandler.HashBoolEscape, false);
                AnimationHandler.animator.SetBool(AnimationHandler.HashBoolAttack, false);

                if (state == Status.Idle)
                {
                    AnimationHandler.animator.SetBool(AnimationHandler.HashBoolRun, false);
                }
                if (state == Status.Detected)
                {
                    AnimationHandler.animator.SetBool(AnimationHandler.HashBoolRun, true);
                }

                if (state == Status.Attackable)
                {
                    AnimationHandler.animator.SetBool(AnimationHandler.HashBoolRun, false);
                    AnimationHandler.animator.SetBool(AnimationHandler.HashBoolAttack, true);
                }

                if (state == Status.Escape)
                {
                    AnimationHandler.animator.SetBool(AnimationHandler.HashBoolEscape, true);
                }
            };

            resourceHandler.OnDeath += () =>
            {
                AnimationHandler.animator.SetTrigger(AnimationHandler.HashTriggerDeath);
                resourceHandler.isDying = true;
                // rewardHandler.DropItem();
            };
        }
        
    }
    
}
