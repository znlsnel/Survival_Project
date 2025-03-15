using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement: MonoBehaviour
    {
         private Rigidbody _rigidbody;

    [SerializeField] float moveSpeed = 5f;
    // runningSpeed
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    
    public LayerMask groundLayerMask; 
    // 기존 방식: 방향을 계산해서 이동한 뒤 회전도 적용하는 방식
    // 현재 방식: 무조건 앞쪽으로 이동(블렌더 트리로 가속도 적용) 회전은 별도로 계산
    float speedFactor = 8f; // 최대 속도 조절
    public float acceleration = 5f; // 가속도
    public float deceleration = 5f; // 감속도
    public float currentSpeed = 0f; // 현재 속도 (애니메이션에 반영)

    // Status로 관리할 수도 있음.
    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool isMoved;
    
    private bool? _isPrevGrounded = null;
    [HideInInspector] public bool isLanded = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    // 정지 상태 체크 필요
    public void Move(Vector2 moveInputValue)
    {
        isMoved = moveInputValue != Vector2.zero;

        if (!isMoved)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speedFactor, acceleration * Time.deltaTime);
        }
        
        // Vector3 direction = (transform.right * moveInputValue.x + transform.forward * moveInputValue.y).normalized;
        // Debug.Log(transform.forward);

        Vector3 moveVelocity = transform.forward * currentSpeed; // 서서히 증가
        moveVelocity.y = _rigidbody.velocity.y; // 점프 등 Y축 속도 유지
        _rigidbody.velocity = moveVelocity;
    }

    public Camera mainCamera;
    
    public void Rotate(Vector2 inputValue)
    {
        if (inputValue.sqrMagnitude <= 0.01f) return;
        
        // fix : 플레이어를 기준으로 해버리면 forward의 기준이 매번 변경되며 반전을 반복함
        
        // 카메라 회전에 대하여 옳바른 처리 필요
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraForward = mainCamera.transform.forward; 
        cameraForward.y = 0;

        Vector3 direction = (cameraRight * inputValue.x + (cameraForward * inputValue.y)).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        transform.rotation = targetRotation; // 즉시 회전
    }

    public void Jump()
    {
        Debug.Log("jump");
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
    // fix: 자기 자신과 충돌하는 현상 발생 - 레이어로 수정
    public bool IsGrounded()
    {
        bool value = Physics.CheckSphere(transform.position, 0.2f, groundLayerMask);
        if(_isPrevGrounded == false && value == true) isLanded = true; else isLanded = false;
        _isPrevGrounded = value;
        return value;
    }

    public void Knockback(Vector2 knockbackForce)
    {
        
        _rigidbody.AddForce(knockbackForce, ForceMode.Impulse);
    }
    
    public void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
    }
    }
}