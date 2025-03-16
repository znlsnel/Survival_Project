using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace Player
{
    [RequireComponent(typeof(Movement))]
    public class Controller : MonoBehaviour
    {
        // private Input _input;
        
        private Movement _movement; public Actor.IMovement Movement => _movement;
        private Animation _animation;
        public Audio Audio {get; private set;}
        public int addedJumpCount = 1;
        // public int comboCount = 0;
        // public int knockBackForce = 10; // 상대의 공격에 따라 달라질 수 있음
        
        // equipment 관련 클래스 하나..
        
        // 착용 시 따라오는 방향으로 코드 변경
        // public Weapon.Melee.Controller meleeWeapon;
        // problem : 장비 상태를 변경하는 작업이 플레이어랑 연결되어야 하는 상황
      
        void Awake()
        {
            // _input = GetComponent<Input>();
            _movement = GetComponent<Movement>();
            _animation = GetComponent<Animation>();
            Audio = GetComponent<Audio>();
        }
        

        void Start()
        {
            // _animation.animator.SetTrigger(Animation.BreakIdleTrigger); // 애니메이터 자체를 수정
            // _animation.OnMeleeAttackAvailable += meleeWeapon.SetMeleeAttackAvailable;
            
            InputManager.Instance.Move.action.performed += (context) =>
            {
                _movement.Rotate(context.ReadValue<Vector2>());
            };

            // Vector.Zero 에서 다른 값으로 변경될 때
            InputManager.Instance.Move.action.started += (context) =>
            {
                _movement.currMoveInputValue = (context.ReadValue<Vector2>());

            };
            // Vector.Zero가 호출 될 때
            InputManager.Instance.Move.action.canceled += (context) =>
            {
                _movement.currMoveInputValue = (context.ReadValue<Vector2>());
            };

            InputManager.Instance.Jump.action.performed += (context) =>
            {
                if (addedJumpCount == 0) return; 
                addedJumpCount -= 1;
                _movement.Jump();
                Audio.PlayRandomSound(PlayerSoundType.Jump);
            };
        }
        
        void FixedUpdate()
        {
            // 애니메이터의 역할
            _animation.animator.SetFloat(Animation.HashStateTime, Mathf.Repeat(_animation.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            _animation.animator.SetFloat(Animation.HashForwardSpeed, _movement.currentSpeed);
   
            // question: 도끼로 채집 등인 경우, 콤보 기능 끄기 필요한 지?
            // fix: stateMachine에서 할 수 있는 지 체크
            
            // // attack check
            // if (_animation.meleeStateMachine.isInStateMachine) { _movement.Stop(); }
            // // attack
            // if (_input.isClicked)
            // {
            //     _input.isClicked = false; // fix - learn : 내부 로직 도중 에러가 나면 false 처리가 안되면서 무한 재생되는 버그 발생
            //     if (_input.IsJumpPressed) return;
            //     
            //     if (!_animation.animator.GetBool(Animation.HashIsAbleRegisterCombo)) return;
            //
            //     
            //     _animation.animator.SetTrigger(Animation.MeleeAttackTrigger); 
            //     _audio.PlayRandomSound(PlayerSoundType.Attack); // notice: 클래스 내부 enum의 경우 plyWeight에 따라 자동으로 static 처리
            //     // meleeWeaponController.audioHandler.PlayerOneShot(MeleeWeaponAudioHandler.SoundType.Attack);
            //     _animation.animator.SetBool(Animation.HashIsAbleRegisterCombo, false);
            //     
            //     // feat: 쿨타임 개념 필요
            //     comboCount += 1; if(comboCount >= 2) comboCount = 0;
            // }
           
            // movement의 역할
            if (_movement.IsGrounded())
            {
                addedJumpCount = 1; // bug: 뛰는 순간 바닥으로 인지되어 한번 더 뛰게 됨
                _animation.animator.SetBool(Animation.IsGrounded, true);
            }
            else
            {
                _animation.animator.SetBool(Animation.IsGrounded, false);
            }
            if (_movement.isLanded)
            {
                Audio.PlayRandomSound(PlayerSoundType.Landed);
            }
        }

        // fix: 공격이 확실할 때만 되도록 개선 필요
        void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out HitPoint hitPoint)) return;
            if (hitPoint.hitEnemies.Contains(gameObject)) return;

            _animation.animator.SetTrigger(Animation.HashHurtTrigger);
            Audio.PlayRandomSound(PlayerSoundType.Damaged);

            Vector3 knockBackDirection = (transform.position - other.transform.position).normalized;
            knockBackDirection.y = 0;
            Movement.ApplyKnockBack(knockBackDirection, 5f);
            
            hitPoint.hitEnemies.Add(gameObject);
        }
    }
}
