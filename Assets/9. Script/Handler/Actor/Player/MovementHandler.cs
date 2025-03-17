using System;
using System.Collections;
using Actor;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody), (typeof(Collider)))]
    public class MovementHandler: MonoBehaviour, IMovement
    {
         private Rigidbody _rigidbody;

         public Camera mainCamera;

        [SerializeField] float moveSpeed = 5f;
        // runningSpeed
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float rotationSpeed = 0.1f; 
        [HideInInspector] public Vector2 currMoveInputValue = Vector2.zero;
        
        public LayerMask groundLayerMask; 
        float speedFactor = 8f; // 최대 속도 조절
        public float acceleration = 1f; // 가속도
        public float deceleration = 1f; // 감속도
        public float currentSpeed = 0f; // 현재 속도 (애니메이션에 반영)

        // Status로 관리할 수도 있음.
        [HideInInspector] public bool canAttack;
        [HideInInspector] public bool isMoved;
        [HideInInspector] public bool isMoveable = true;
        
        private bool? _isPrevGrounded = null;
        [HideInInspector] public bool isLanded = false;
        [HideInInspector] public bool isComboAble = true;
        [HideInInspector] public bool isAttacking = false;

        public Quaternion currTargetRotation;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Move(currMoveInputValue);
            transform.rotation = Quaternion.Slerp(transform.rotation, currTargetRotation, Time.deltaTime * rotationSpeed);
        }

        // 정지 상태 체크 필요
        public void Move(Vector2 moveInputValue)
        {
            if (!isMoveable) return;
            
            isMoved = moveInputValue != Vector2.zero;
            
            if (!isMoved) currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.deltaTime);
            else currentSpeed = Mathf.Lerp(currentSpeed, speedFactor, acceleration * Time.deltaTime);
            
            Vector3 moveVelocity = transform.forward * currentSpeed; // 서서히 증가
            moveVelocity.y = _rigidbody.velocity.y; // 점프 등 Y축 속도 유지
            _rigidbody.velocity = moveVelocity;
        }

        
        public void Rotate(Vector2 inputValue)
        {
            if (inputValue.sqrMagnitude <= 0.01f) return;
            
            Vector3 cameraRight = mainCamera.transform.right;
            Vector3 cameraForward = mainCamera.transform.forward; 
            cameraForward.y = 0;

            Vector3 direction = (cameraRight * inputValue.x + (cameraForward * inputValue.y)).normalized;
            currTargetRotation = Quaternion.LookRotation(direction);
        }
        
        public void Jump()
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        // fix: 자기 자신과 충돌하는 현상 발생 - 레이어로 수정
        public bool IsGrounded()
        {
            bool value = Physics.CheckSphere(transform.position, 0.2f, groundLayerMask);
            if(_isPrevGrounded == false && value) isLanded = true; else isLanded = false;
            _isPrevGrounded = value;
            return value;
        }
        
        
        public void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        public void ApplyKnockBack(Vector3 knockBackDirection, float force)
        {
            isMoveable = false;
            _rigidbody.AddForce(knockBackDirection * force, ForceMode.Impulse);
            StartCoroutine(GetStun());
        }

        private IEnumerator GetStun()
        {
            yield return new WaitForSeconds(0.5f);
            isMoveable = true;
        }
    }
}