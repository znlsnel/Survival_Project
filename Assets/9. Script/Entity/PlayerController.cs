using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler _input;
    private PlayerMovementHandler _movement;
    private PlayerAnimationHandler _animation;
    private PlayerAudioHandler _audio;

    private PlayerCondition _condition;
    public int jumpCount = 2;
    
    public MeleeWeapon MeleeWeapon;
    
    void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
        _movement = GetComponent<PlayerMovementHandler>();
        _animation = GetComponent<PlayerAnimationHandler>();
        _audio = GetComponent<PlayerAudioHandler>();
        
        _condition = GetComponent<PlayerCondition>();
    }
    
    void FixedUpdate()
    {
        // move
        // var isRunning = _input.IsDashPressed; // running 방식: shift 또는 가속도
        _movement.Move(_input.movementValue);
        
        if (_movement.isMoved)
        {
            _animation.animator.SetTrigger(_animation.BreakIdleTrigger);
        }
        _animation.animator.SetFloat(_animation.HashForwardSpeed, _movement.currentSpeed);
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
            
            _animation.animator.SetTrigger(_animation.MeleeAttackTrigger);
            _input.isClicked = false;
        }

        // refactor : 캐싱 필요
        if (_movement.IsGrounded())
        {
            // fix: 뛰는 순간 바닥으로 인지되어 한번 더 뛰게 됨
            jumpCount = 1;
            _animation.animator.SetBool(_animation.IsGrounded, true);
        }
        else
        {
            _animation.animator.SetBool(_animation.IsGrounded, false);
        }
    }
}
