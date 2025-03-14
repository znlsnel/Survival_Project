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

	// equipment 관련 클래스 하나..

	// 착용 시 따라오는 방향으로 코드 변경( 
	public MeleeWeaponController meleeWeaponController;
	public ActiveItem usableItem;
	// problem : 장비 상태를 변경하는 작업이 플레이어랑 연결되어야 하는 상황

	void Awake()
	{
		_input = GetComponent<PlayerInputHandler>();
		_movement = GetComponent<PlayerMovementHandler>();
		_animation = GetComponent<PlayerAnimationHandler>();

		_audio = GetComponent<PlayerAudioHandler>();

		_tempCondition = GetComponent<PlayerTempCondition>();
	}

	void Start()
	{
		_animation.animator.SetTrigger(PlayerAnimationHandler.BreakIdleTrigger);
		_animation.OnMeleeAttackAvailable += meleeWeaponController.SetMeleeAttackAvailable;
	}

	void FixedUpdate()
	{

		// notice: 애니메이션 진행 정도를 파악하여 콤보로 연결할 지 조건 분기
		// question: 도끼로 채집 등인 경우, 콤보 기능 끄기 필요한 지?
		// fix: stateMachine에서 할 수 있는 지 체크
		_animation.animator.SetFloat(PlayerAnimationHandler.HashStateTime, Mathf.Repeat(_animation.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));

		// rotation
		_movement.Rotate(_input.movementValue);

		// move
		// var isRunning = _input.IsDashPressed; // running 방식: shift 또는 가속도
		_movement.Move(_input.movementValue);
		_animation.animator.SetFloat(PlayerAnimationHandler.HashForwardSpeed, _movement.currentSpeed);
		// _animation.animator.SetFloat(_animation.HashForwardSpeed, 0.5f); // 0인경우 제외
		// _audio.PlayFootstep(_movement.currentSpeed);

		// attack check
		if (_animation.meeleStateMachine.isInStateMachine) { _movement.Stop(); }
		// attack
		if (_input.isClicked)
		{
			_input.isClicked = false; // fix - learn : 내부 로직 도중 에러가 나면 false 처리가 안되면서 무한 재생되는 버그 발생
			if (_input.IsJumpPressed) return;

			if (!_animation.animator.GetBool(PlayerAnimationHandler.HashIsAbleRegisterCombo)) return;


			_animation.animator.SetTrigger(PlayerAnimationHandler.MeleeAttackTrigger);
			_audio.PlayRandomSound(PlayerSoundType.Attack); // notice: 클래스 내부 enum의 경우 plyWeight에 따라 자동으로 static 처리
															// meleeWeaponController.audioHandler.PlayerOneShot(MeleeWeaponAudioHandler.SoundType.Attack);
			_animation.animator.SetBool(PlayerAnimationHandler.HashIsAbleRegisterCombo, false);

			// feat: 쿨타임 개념 필요
			comboCount += 1; if (comboCount >= 2) comboCount = 0;
		}


		// jump
		if (_input.IsJumpPressed && jumpCount > 0)
		{
			// 컨디션 부족하면 점프 불가
			// _condition.UseStemina(5);
			jumpCount -= 1;

			_movement.Jump();
			_audio.PlayRandomSound(PlayerSoundType.Attack); // do: 점프 사운드로 변경 필요
		}

		 
		if (_movement.isLanded)
		{
			_audio.PlayRandomSound(PlayerSoundType.Landed);
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
	void OnTriggerStay(Collider other)
	{
		Debug.Log(other.gameObject.name);

		var chomper = other.gameObject.GetComponentInParent<ChomperController>();

		if (!chomper) return;
		if (!chomper.resource.isAttacking) return;
		chomper.resource.isAttacking = false;

		Vector3 direction = (transform.position - other.transform.position).normalized;
		direction.y = 0; // 위로 날라가는 현상 발생

		_animation.animator.SetTrigger(PlayerAnimationHandler.HashHurtTrigger);
		_audio.PlayRandomSound(PlayerSoundType.Damaged);
		_movement.Knockback(direction * knockBackForce);
	}
}
