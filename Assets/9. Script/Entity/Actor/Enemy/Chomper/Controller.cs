using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.Chomper
{
    [RequireComponent(typeof(Navigation), typeof(Audio), typeof(Resource))]
public class Controller: MonoBehaviour
{
    private Animation _animation;
    private Navigation _navigation;
    private Movement _movement;
    private Audio _audio; 
    public Resource resource;
    

    void Awake()
    {
        _animation = GetComponent<Animation>();
        _navigation = GetComponent<Navigation>();
        _audio = GetComponent<Audio>();
        resource = GetComponent<Resource>();
        _movement = GetComponent<Movement>();
    }

    void Start()
    {
        _animation.OnIsAttacking += resource.SetIsAttacking;
    }


    // fix: 상태가 변경되지 않으면 업데이트 할 필요 없도록 캐싱 필요
    void FixedUpdate()
    {
        if (_movement.isKnockBacked)
        {
            _animation.animator.ResetTrigger(Animation.HashAttackTrigger);

            return;
        }

        _navigation.UpdateNavigation();
        // if (!_navigation.IsStatusChanged()) return;

        if (_navigation.currStatus == Navigation.Status.Idle)
        {
            _navigation.SetTracing(false);
            // _animation.animator.SetBool(ChomperAnimationHandler.HashIsNearBase, false);
            _animation.animator.SetBool(Animation.HashIsRunning, false);
            return;
        }
        
        if (_navigation.currStatus == Navigation.Status.Detected)
        {
            _navigation.SetTracing(true);
            // _animation.animator.SetBool(ChomperAnimationHandler.HashIsNearBase, true);
            _animation.animator.SetBool(Animation.HashIsRunning, true);
            return;
        }

        if (_navigation.currStatus == Navigation.Status.Attackable)
        {
            _navigation.SetTracing(false);
            transform.rotation = Quaternion.LookRotation((_navigation.target.transform.position - transform.position).normalized);
            _animation.animator.SetTrigger(Animation.HashAttackTrigger);
        }
    }

    // notice: 현재 무기의 특성상 트리거 형태로 동작
    void OnTriggerStay(Collider other)
    {
        // do: 무기 추상화 필요
        if (other.gameObject.TryGetComponent(out Weapon.Melee.Controller weaponController))
        {
            // if (_movement.isKnockBacked) return;
            if (!weaponController.isAttacking) return;
            
            if (!weaponController.hitObjects.Contains(gameObject))
            {
                Vector3 direction = (transform.position - other.transform.position).normalized;
                direction.y = 0f;
                
                // if(_movement.KnockBackCoroutine != null) StopCoroutine(_movement.KnockBackCoroutine);
                // _movement.KnockBackCoroutine = StartCoroutine(_movement.ApplyKnockBack(_navigation.agent, direction, weaponController.knockBackForce));
                
                _animation.animator.SetTrigger(Animation.HashHitTrigger);
            
                weaponController.hitObjects.Add(gameObject);
               _audio.PlayRandomSound(ChomperSoundType.Damaged);
            }
        }
    }
}
}
