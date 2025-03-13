using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler _input;
    private PlayerMovementHandler _movement;
    private PlayerAnimationHandler _animation;
    private PlayerAudioHandler _audio;

    private PlayerTempCondition _tempCondition;
    public int jumpCount = 2;
    public int comboCount = 0;
    public int knockBackForce = 10; // 상대의 공격에 따라 달라질 수 있음
    
    // 착용 시 따라오는 방향으로 코드 변경
    public MeleeWeaponController meleeWeaponController;
    
    void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
        _movement = GetComponent<PlayerMovementHandler>();
        _animation = GetComponent<PlayerAnimationHandler>();
        _audio = GetComponent<PlayerAudioHandler>();
        
        _tempCondition = GetComponent<PlayerTempCondition>();
    }
    
    void FixedUpdate()
    {
        // notice: 애니메이션 진행 정도를 파악하여 콤보로 연결할 지 조건 분기
        _animation.animator.SetFloat(PlayerAnimationHandler.HashStateTime, Mathf.Repeat(_animation.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));

        // move
        // var isRunning = _input.IsDashPressed; // running 방식: shift 또는 가속도
        _movement.Move(_input.movementValue);
        
        if (_movement.isMoved)
        {
            _animation.animator.SetTrigger(PlayerAnimationHandler.BreakIdleTrigger);
        }
        
        _animation.animator.SetFloat(PlayerAnimationHandler.HashForwardSpeed, _movement.currentSpeed);
        // _animation.animator.SetFloat(_animation.HashForwardSpeed, 0.5f); // 0인경우 제외
        // _audio.PlayFootstep(_movement.currentSpeed);
        _movement.Rotate(_input.movementValue);

        // jump
        if (_input.IsJumpPressed && jumpCount > 0)
        {
            // 컨디션 부족하면 점프 불가
            // _condition.UseStemina(5);
            jumpCount -= 1;

            _movement.Jump();
        }

        // attack : 점프 중 공격?
        if (_input.isClicked)
        {
            // _condition.UseStemina(10);
            
            _animation.animator.SetTrigger(PlayerAnimationHandler.MeleeAttackTrigger);
            _audio.PlayerOneShot(PlayerAudioHandler.SoundType.Attack); // notice: 클래스 내부 enum의 경우 plyWeight에 따라 자동으로 static 처리
            meleeWeaponController.audioHandler.PlayerOneShot(MeleeWeaponAudioHandler.SoundType.Attack);
            
            // feat: 쿨타임 개념 필요
            comboCount += 1;
            if(comboCount >= 2) comboCount = 0;
            
            _input.isClicked = false;
        }

        // refactor : 캐싱 필요
        if (_movement.IsGrounded())
        {
            // fix: 뛰는 순간 바닥으로 인지되어 한번 더 뛰게 됨
            jumpCount = 1;
            _animation.animator.SetBool(PlayerAnimationHandler.IsGrounded, true);
        }
        else
        {
            _animation.animator.SetBool(PlayerAnimationHandler.IsGrounded, false);
        }
    }

    // fix: 공격이 확실할 때만 되도록 개선 필요
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out ChomperController chomperController))
        {
            Vector3 direction = (transform.position - other.transform.position).normalized;
            direction.y = 0; // 위로 날라가는 현상 발생
            
            _animation.animator.SetTrigger(PlayerAnimationHandler.HashHurtTrigger);
            _audio.PlayerOneShot(PlayerAudioHandler.SoundType.Damaged);
            _movement.Knockback(direction * knockBackForce);
        }
    }
}
