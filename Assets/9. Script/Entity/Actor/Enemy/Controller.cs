using System;
using System.Collections;
using Actor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [RequireComponent(typeof(Animation), typeof(Audio), typeof(Movement))]
    [RequireComponent(typeof(Navigation), typeof(Resource))]
    public class Controller : MonoBehaviour
    {
        public Animation Animation { get; private set; }
        private Audio _audio;
        private Movement _movement; public Actor.IMovement Movement => _movement;
        private Navigation _navigation;
        private Resource _resource;

        void Awake()
        {
            Animation = GetComponent<Animation>();
            _audio = GetComponent<Audio>();
            _movement = GetComponent<Movement>();
            _navigation = GetComponent<Navigation>();
            _resource = GetComponent<Resource>();
        }

        void Start()
        {
            Animation.WhenAttack += _navigation.StopByAnimation;
            
            _navigation.WhenChangedStatus += (state) =>
            {
                Animation.animator.SetBool(Animation.HashBoolAttack, false);

                if (state == Navigation.Status.Idle)
                {
                    Animation.animator.SetBool(Animation.HashBoolRun, false);
                }
                if (state == Navigation.Status.Detected)
                {
                    Animation.animator.SetBool(Animation.HashBoolRun, true);
                }

                if (state == Navigation.Status.Attackable)
                {
                    Animation.animator.SetBool(Animation.HashBoolRun, false);
                    Animation.animator.SetBool(Animation.HashBoolAttack, true);
                }
            };
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player.HitPoint hitPoint))
            {
                // if (!hitPoint.controller.GetComponent<Player.Movement>().isAttacking) return;
                if (hitPoint.hitEnemies.Contains(gameObject)) return;
                hitPoint.hitEnemies.Add(gameObject);
                
                Debug.Log("one time");
                
                var knockBackDirection = (transform.position - hitPoint.controller.transform.position).normalized;
                knockBackDirection.y = 0;

                Animation.animator.SetTrigger(Animation.HashTriggerHit);

                _navigation.Agent.isStopped = true;
                // _navigation.Agent.enabled = false;
                
                _movement.ApplyKnockBack(knockBackDirection, 1f);
                StartCoroutine(RestoreNavMeshAgent());
                
                _audio.PlayRandomSound(Enemy.SoundType.Damaged);
                _resource.Modify(10);
            }
            
            
            IEnumerator RestoreNavMeshAgent()
            {
                yield return new WaitForSeconds(1f);
                _movement.Stop();
                _navigation.Agent.isStopped = false;
                // _navigation.Agent.enabled = true;
                _navigation.Agent.Warp(transform.position);
            }
        }
    }
    
}
