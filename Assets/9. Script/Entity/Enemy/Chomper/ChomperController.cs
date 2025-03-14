using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(ChomperNavigationHandler), typeof(ChomperAudioHandler))]
public class ChomperController: MonoBehaviour
{
    private ChomperAnimationHandler _animation;
    private ChomperNavigationHandler _navigation;
    private ChomperMovementHandler _movement;
    private ChomperAudioHandler _audio; 
    public ChomperResourceHandler resource;
    

    void Awake()
    {
        _animation = GetComponent<ChomperAnimationHandler>();
        _navigation = GetComponent<ChomperNavigationHandler>();
        _audio = GetComponent<ChomperAudioHandler>();
        resource = GetComponent<ChomperResourceHandler>();
        _movement = GetComponent<ChomperMovementHandler>();
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
            _animation.animator.ResetTrigger(ChomperAnimationHandler.HashAttackTrigger);

            return;
        }

        _navigation.UpdateNavigation();
        // if (!_navigation.IsStatusChanged()) return;

        if (_navigation.currStatus == ChomperNavigationHandler.Status.Idle)
        {
            _navigation.SetTracing(false);
            // _animation.animator.SetBool(ChomperAnimationHandler.HashIsNearBase, false);
            _animation.animator.SetBool(ChomperAnimationHandler.HashIsRunning, false);
            return;
        }
        
        if (_navigation.currStatus == ChomperNavigationHandler.Status.Detected)
        {
            _navigation.SetTracing(true);
            // _animation.animator.SetBool(ChomperAnimationHandler.HashIsNearBase, true);
            _animation.animator.SetBool(ChomperAnimationHandler.HashIsRunning, true);
            return;
        }

        if (_navigation.currStatus == ChomperNavigationHandler.Status.Attackable)
        {
            _navigation.SetTracing(false);
            transform.rotation = Quaternion.LookRotation((_navigation.target.transform.position - transform.position).normalized);
            _animation.animator.SetTrigger(ChomperAnimationHandler.HashAttackTrigger);
        }
    }

    // notice: 현재 무기의 특성상 트리거 형태로 동작
    void OnTriggerStay(Collider other)
    {
        // do: 무기 추상화 필요
        if (other.gameObject.TryGetComponent(out MeleeWeaponController weaponController))
        {
            // if (_movement.isKnockBacked) return;
            if (!weaponController.isAttacking) return;
            
            if (!weaponController.hitObjects.Contains(gameObject))
            {
                Vector3 direction = (transform.position - other.transform.position).normalized;
                direction.y = 0f;
                
                if(_movement.KnockBackCoroutine != null) StopCoroutine(_movement.KnockBackCoroutine);
                _movement.KnockBackCoroutine = StartCoroutine(_movement.ApplyKnockBack(_navigation.agent, direction, weaponController.knockBackForce));
                _animation.animator.SetTrigger(ChomperAnimationHandler.HashHitTrigger);
            
                weaponController.hitObjects.Add(gameObject);
               _audio.PlayRandomSound(ChomperSoundType.Damaged);
            }
        }
    }
}