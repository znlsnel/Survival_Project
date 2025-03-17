using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace Player
{
    [RequireComponent(typeof(MovementHandler), typeof(ResourceHandler), typeof(EquipHandler))]
    public class Controller : MonoBehaviour
    {

        private EquipHandler equipHandler;

        private Input _input;
        //public ResourceHandler ResourceHandler {get; private set;}
        private PlayerCondition playerCondition;
        private UICondition uICondition;
        private MovementHandler movementHandler; public Actor.IMovement MovementHandler => movementHandler;
        public AnimationHandler AnimationHandler { get; private set; }
        public AudioHandler AudioHandler {get; private set;}
        
        public int addedJumpCount = 1;
        // public int comboCount = 0;
        // public int knockBackForce = 10; // 상대의 공격에 따라 달라질 수 있음
        
        // equipment 관련 클래스 하나..
        
        // 착용 시 따라오는 방향으로 코드 변경
        // public Weapon.Melee.Controller meleeWeapon;
        // problem : 장비 상태를 변경하는 작업이 플레이어랑 연결되어야 하는 상황
      
        void Awake() 
        {
			equipHandler = GetComponent<EquipHandler>();
			_input = GetComponent<Input>();
           // ResourceHandler = GetComponent<ResourceHandler>();

            playerCondition = GetComponent<PlayerCondition>();
            UICondition uICondition = GetComponent<UICondition>();

            movementHandler = GetComponent<MovementHandler>();
            AnimationHandler = GetComponent<AnimationHandler>();
            AudioHandler = GetComponent<AudioHandler>();
        }
        

        void Start()
        {
            // _animation.animator.SetTrigger(Animation.BreakIdleTrigger); // 애니메이터 자체를 수정
            // _animation.OnMeleeAttackAvailable += meleeWeapon.SetMeleeAttackAvailable;
            
            InputManager.Move.performed += (context) =>
            {
                movementHandler.CheckRotateValue(context.ReadValue<Vector2>());
            };
            
            // Vector.Zero 에서 다른 값으로 변경될 때
            InputManager.Move.started += (context) =>
            {
                movementHandler.currMoveInputValue = (context.ReadValue<Vector2>());
            
            };
            // Vector.Zero가 호출 될 때
            InputManager.Move.canceled += (context) =>
            {
                movementHandler.currMoveInputValue = (context.ReadValue<Vector2>());
            };
            
            InputManager.Jump.performed += (context) =>
            {
                if (addedJumpCount == 0) return; 
                addedJumpCount -= 1;
                movementHandler.Jump();
                AudioHandler.PlayRandomSound(PlayerSoundType.Jump);
            };

            InputManager.LeftMouse.performed += InputLeftMouse;
                
                
                // _movement.isComboAble = false;
                
                // meleeWeaponController.audioHandler.PlayerOneShot(MeleeWeaponAudioHandler.SoundType.Attack);
                // _animation.animator.SetBool(Animation.HashIsAbleRegisterCombo, false);
            
        }
        
        void FixedUpdate()
        {
            // 애니메이터의 역할
            AnimationHandler.animator.SetFloat(AnimationHandler.HashStateTime, Mathf.Repeat(AnimationHandler.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            AnimationHandler.animator.SetFloat(AnimationHandler.HashForwardSpeed, movementHandler.currentSpeed);
   
            // movement의 역할
            if (movementHandler.IsGrounded())
            {
                addedJumpCount = 1; // bug: 뛰는 순간 바닥으로 인지되어 한번 더 뛰게 됨
                AnimationHandler.animator.SetBool(AnimationHandler.IsGrounded, true);
            }
            else
            {
                AnimationHandler.animator.SetBool(AnimationHandler.IsGrounded, false);
            }
            if (movementHandler.isLanded)
            {
                AudioHandler.PlayRandomSound(PlayerSoundType.Landed);
            }
        }

        // fix: 공격이 확실할 때만 되도록 개선 필요
        void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Enemy.HitPoint hitPoint)) 
                return;

            if (hitPoint.hitEnemies.Contains(gameObject)) 
                return;


			Vector3 lookDirection = (hitPoint.controller.transform.position - transform.position).normalized;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            
            AnimationHandler.animator.SetTrigger(AnimationHandler.HashHurtTrigger);
            AudioHandler.PlayRandomSound(PlayerSoundType.Damaged);
            //ResourceHandler.Modify(-30);
            
            Vector3 knockBackDirection = (transform.position - hitPoint.controller.transform.position).normalized;
            knockBackDirection.y = 0;
            MovementHandler.ApplyKnockBack(knockBackDirection, 10f);
            
            hitPoint.hitEnemies.Add(gameObject);
        }


        private void InputLeftMouse(InputAction.CallbackContext context)
        {
			if (!movementHandler.IsGrounded()) return;


            ActiveItem activeItem = equipHandler.GetActiveItem();
            if (activeItem != null)
            {
				AnimationHandler.animator.SetTrigger(AnimationHandler.MeleeAttackTrigger);
                activeItem.Trigger();
			}

		}
    }
}
