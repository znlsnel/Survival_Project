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
	public int knockBackForce = 10; // ����� ���ݿ� ���� �޶��� �� ����

	// equipment ���� Ŭ���� �ϳ�..

	// ���� �� ������� �������� �ڵ� ����( 
	public MeleeWeaponController meleeWeaponController;
	public ActiveItem usableItem;
	// problem : ��� ���¸� �����ϴ� �۾��� �÷��̾�� ����Ǿ�� �ϴ� ��Ȳ

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

		// notice: �ִϸ��̼� ���� ������ �ľ��Ͽ� �޺��� ������ �� ���� �б�
		// question: ������ ä�� ���� ���, �޺� ��� ���� �ʿ��� ��?
		// fix: stateMachine���� �� �� �ִ� �� üũ
		_animation.animator.SetFloat(PlayerAnimationHandler.HashStateTime, Mathf.Repeat(_animation.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));

		// rotation
		_movement.Rotate(_input.movementValue);

		// move
		// var isRunning = _input.IsDashPressed; // running ���: shift �Ǵ� ���ӵ�
		_movement.Move(_input.movementValue);
		_animation.animator.SetFloat(PlayerAnimationHandler.HashForwardSpeed, _movement.currentSpeed);
		// _animation.animator.SetFloat(_animation.HashForwardSpeed, 0.5f); // 0�ΰ�� ����
		// _audio.PlayFootstep(_movement.currentSpeed);

		// attack check
		if (_animation.meeleStateMachine.isInStateMachine) { _movement.Stop(); }
		// attack
		if (_input.isClicked)
		{
			_input.isClicked = false; // fix - learn : ���� ���� ���� ������ ���� false ó���� �ȵǸ鼭 ���� ����Ǵ� ���� �߻�
			if (_input.IsJumpPressed) return;

			if (!_animation.animator.GetBool(PlayerAnimationHandler.HashIsAbleRegisterCombo)) return;


			_animation.animator.SetTrigger(PlayerAnimationHandler.MeleeAttackTrigger);
			_audio.PlayRandomSound(PlayerSoundType.Attack); // notice: Ŭ���� ���� enum�� ��� plyWeight�� ���� �ڵ����� static ó��
															// meleeWeaponController.audioHandler.PlayerOneShot(MeleeWeaponAudioHandler.SoundType.Attack);
			_animation.animator.SetBool(PlayerAnimationHandler.HashIsAbleRegisterCombo, false);

			// feat: ��Ÿ�� ���� �ʿ�
			comboCount += 1; if (comboCount >= 2) comboCount = 0;
		}


		// jump
		if (_input.IsJumpPressed && jumpCount > 0)
		{
			// ����� �����ϸ� ���� �Ұ�
			// _condition.UseStemina(5);
			jumpCount -= 1;

			_movement.Jump();
			_audio.PlayRandomSound(PlayerSoundType.Attack); // do: ���� ����� ���� �ʿ�
		}

		 
		if (_movement.isLanded)
		{
			_audio.PlayRandomSound(PlayerSoundType.Landed);
		}

		// refactor : ĳ�� �ʿ�
		if (_movement.IsGrounded())
		{
			// fix: �ٴ� ���� �ٴ����� �����Ǿ� �ѹ� �� �ٰ� ��
			jumpCount = 1;
			_animation.animator.SetBool(PlayerAnimationHandler.IsGrounded, true);
		}
		else
		{
			_animation.animator.SetBool(PlayerAnimationHandler.IsGrounded, false);
		}
	}

	// fix: ������ Ȯ���� ���� �ǵ��� ���� �ʿ�
	void OnTriggerStay(Collider other)
	{
		Debug.Log(other.gameObject.name);

		var chomper = other.gameObject.GetComponentInParent<ChomperController>();

		if (!chomper) return;
		if (!chomper.resource.isAttacking) return;
		chomper.resource.isAttacking = false;

		Vector3 direction = (transform.position - other.transform.position).normalized;
		direction.y = 0; // ���� ���󰡴� ���� �߻�

		_animation.animator.SetTrigger(PlayerAnimationHandler.HashHurtTrigger);
		_audio.PlayRandomSound(PlayerSoundType.Damaged);
		_movement.Knockback(direction * knockBackForce);
	}
}
