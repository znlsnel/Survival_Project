using System;
using System.Collections;
using Actor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [RequireComponent(typeof(AnimationHandler), typeof(AudioHandler), typeof(MovementHandler))]
    [RequireComponent(typeof(NavigationHandler), typeof(ResourceHandler), typeof(RewardHandler))]
    public class Controller : MonoBehaviour
    {
        public AnimationHandler AnimationHandler { get; private set; }
        private AudioHandler audioHandler;
        private MovementHandler movementHandler; public Actor.IMovement MovementHandler => movementHandler;
        protected NavigationHandler navigationHandler;
        protected ResourceHandler resourceHandler;
        private RewardHandler rewardHandler;

        void Awake()
        {
            AnimationHandler = GetComponent<AnimationHandler>();
            audioHandler = GetComponent<AudioHandler>();
            movementHandler = GetComponent<MovementHandler>();
            navigationHandler = GetComponent<NavigationHandler>();
            resourceHandler = GetComponent<ResourceHandler>();
            rewardHandler = GetComponent<RewardHandler>();
        }

        protected virtual void Start()
        {
            AnimationHandler.WhenAttack += navigationHandler.StopByAnimation;
            
            navigationHandler.WhenChangedStatus += (state) =>
            {
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
            };

            resourceHandler.OnDeath += () =>
            {
                AnimationHandler.animator.SetTrigger(AnimationHandler.HashTriggerDeath);
                resourceHandler.isDying = true;
                // rewardHandler.DropItem();
            };
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player.HitPoint hitPoint))
            {
                if (resourceHandler.isDying) return;
                
                // if (!hitPoint.controller.GetComponent<Player.Movement>().isAttacking) return;
                if (hitPoint.hitEnemies.Contains(gameObject)) return;
                hitPoint.hitEnemies.Add(gameObject);
                
                var lookDirection = (hitPoint.controller.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(lookDirection);
                
                var knockBackDirection = (transform.position - hitPoint.controller.transform.position).normalized;
                knockBackDirection.y = 0;

                AnimationHandler.animator.SetTrigger(AnimationHandler.HashTriggerHit);

                navigationHandler.Agent.isStopped = true;
                // _navigation.Agent.enabled = false;
                
                movementHandler.ApplyKnockBack(knockBackDirection, 1f);
                StartCoroutine(RestoreNavMeshAgent());
                
                audioHandler.PlayRandomSound(Enemy.SoundType.Damaged);
                resourceHandler.ModifyHealth(-50);
            }
            
            
            IEnumerator RestoreNavMeshAgent()
            {
                yield return new WaitForSeconds(1f);
                movementHandler.Stop();
                navigationHandler.Agent.isStopped = false;
                // _navigation.Agent.enabled = true;
                navigationHandler.Agent.Warp(transform.position);
            }
        }
    }
    
}
