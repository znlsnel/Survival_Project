using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace Player
{
    [RequireComponent(typeof(Movement), typeof(Resource))]
    public class Controller : MonoBehaviour
    {
        private Input _input;
        public Resource Resource {get; private set;}
        private Movement _movement; public Actor.IMovement Movement => _movement;
        public Animation Animation { get; private set; }
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
            _input = GetComponent<Input>();
            Resource = GetComponent<Resource>();
            _movement = GetComponent<Movement>();
            Animation = GetComponent<Animation>();
            Audio = GetComponent<Audio>();
        }
        

        void Start()
        {
            // _animation.animator.SetTrigger(Animation.BreakIdleTrigger); // 애니메이터 자체를 수정
            // _animation.OnMeleeAttackAvailable += meleeWeapon.SetMeleeAttackAvailable;
            
            // InputManager.Instance.Move.action.performed += (context) =>
            // {
            //     _movement.Rotate(context.ReadValue<Vector2>());
            // };
            //
            // // Vector.Zero 에서 다른 값으로 변경될 때
            // InputManager.Instance.Move.action.started += (context) =>
            // {
            //     _movement.currMoveInputValue = (context.ReadValue<Vector2>());
            //
            // };
            // // Vector.Zero가 호출 될 때
            // InputManager.Instance.Move.action.canceled += (context) =>
            // {
            //     _movement.currMoveInputValue = (context.ReadValue<Vector2>());
            // };
            //
            // InputManager.Instance.Jump.action.performed += (context) =>
            // {
            //     if (addedJumpCount == 0) return; 
            //     addedJumpCount -= 1;
            //     _movement.Jump();
            //     Audio.PlayRandomSound(PlayerSoundType.Jump);
            // };
            //
            // InputManager.Instance.Click.action.performed += (context) =>
            // {
            //     if (!_movement.IsGrounded()) return;
            //     
            //     // Debug.Log(_movement.isComboAble);
            //     // if (!_movement.isComboAble) return;
            //     
            //     Animation.animator.SetTrigger(Animation.MeleeAttackTrigger); 
            //     Audio.PlayRandomSound(PlayerSoundType.Attack);
            //     
            //     // _movement.isComboAble = false;
            //     
            //     // meleeWeaponController.audioHandler.PlayerOneShot(MeleeWeaponAudioHandler.SoundType.Attack);
            //     // _animation.animator.SetBool(Animation.HashIsAbleRegisterCombo, false);
            // };
        }
        
        void FixedUpdate()
        {
            // 애니메이터의 역할
            Animation.animator.SetFloat(Animation.HashStateTime, Mathf.Repeat(Animation.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            Animation.animator.SetFloat(Animation.HashForwardSpeed, _movement.currentSpeed);
   
            // movement의 역할
            if (_movement.IsGrounded())
            {
                addedJumpCount = 1; // bug: 뛰는 순간 바닥으로 인지되어 한번 더 뛰게 됨
                Animation.animator.SetBool(Animation.IsGrounded, true);
            }
            else
            {
                Animation.animator.SetBool(Animation.IsGrounded, false);
            }
            if (_movement.isLanded)
            {
                Audio.PlayRandomSound(PlayerSoundType.Landed);
            }
        }

        // fix: 공격이 확실할 때만 되도록 개선 필요
        void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Enemy.HitPoint hitPoint)) return;
            if (hitPoint.hitEnemies.Contains(gameObject)) return;
            
            Debug.Log("akwdma");

            Vector3 lookDirection = (hitPoint.controller.transform.position - transform.position).normalized;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            
            Animation.animator.SetTrigger(Animation.HashHurtTrigger);
            Audio.PlayRandomSound(PlayerSoundType.Damaged);
            Resource.Modify(-30);
            
            Vector3 knockBackDirection = (transform.position - hitPoint.controller.transform.position).normalized;
            knockBackDirection.y = 0;
            Movement.ApplyKnockBack(knockBackDirection, 10f);
            
            hitPoint.hitEnemies.Add(gameObject);
        }
    }
}
