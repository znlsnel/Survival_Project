using System;
using UnityEngine;

[RequireComponent(typeof(ChomperNavigationHandler))]
public class ChomperController: MonoBehaviour
{
    private ChomperAnimationHandler _animation;
    private ChomperNavigationHandler _navigation;
    private ChomperBehaviourHandler _behaviour;
    [HideInInspector] public ChomperResourceHandler resource;
    
    private Rigidbody _rigidbody;

    void Awake()
    {
        _animation = GetComponent<ChomperAnimationHandler>();
        _navigation = GetComponent<ChomperNavigationHandler>();
        _behaviour = GetComponent<ChomperBehaviourHandler>();
        resource = GetComponent<ChomperResourceHandler>();
        
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // StartCoroutine(_behaviour.WanderLoop(_navigation.agent));
    }

    // fix: 상태가 변경되지 않으면 업데이트 할 필요 없도록 캐싱 필요
    void FixedUpdate()
    {
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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MeleeWeaponController weapon))
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            direction.y = 0f;
            
            _rigidbody.AddForce(direction * weapon.knockBackForce, ForceMode.Impulse);
            
            Debug.Log($"{weapon.power} - 플레이어의 공격");
        }
    }
}