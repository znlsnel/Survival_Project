using UnityEngine;

public class PlayerMovementHandler: MonoBehaviour
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


        // 입력 벡터를 3D 공간의 이동 방향으로 변환
        Vector3 direction = new Vector3(moveInputValue.x, 0f, moveInputValue.y).normalized;

        // 현재 속도를 유지하면서 이동 처리
        // Vector3 moveVelocity = direction * moveSpeed;
        Vector3 moveVelocity = direction * currentSpeed; // 서서히 증가
        
        moveVelocity.y = _rigidbody.velocity.y; // 점프 등 Y축 속도 유지
        _rigidbody.velocity = moveVelocity;
    }

    public void Rotate(Vector2 inputValue)
    {
        if (inputValue.sqrMagnitude <= 0.01f) return;
        
        Vector3 direction = new Vector3(inputValue.x, 0, inputValue.y);
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = targetRotation; // 즉시 회전
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1440 * Time.deltaTime);
    }

    public void Jump()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
    public bool IsGrounded()
    {
        bool value = Physics.CheckSphere(transform.position, 0.2f, groundLayerMask);
        return value;
    }

    public void Knockback(Vector2 knockbackForce)
    {
        
        _rigidbody.AddForce(knockbackForce, ForceMode.Impulse);
    }
    
    // public float maxDistance = 0.3f;
    // bool IsGrounded()
    // {
    //     Ray[] rays = new Ray[4]
    //     {
    //         new(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
    //         new(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
    //         new(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
    //         new(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
    //     };
    //
    //     for (int i = 0; i < rays.Length; i++)
    //     {
    //         if (Physics.Raycast(rays[i], maxDistance, groundLayerMask))
    //         {
    //             // _animation.animator.SetBool(_animation.HashGrounded, true); // 확인 후 외부에서 처리하도록 관리
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //
    //     // 네 개의 Ray 생성
    //     Ray[] rays = new Ray[4]
    //     {
    //         new(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
    //         new(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
    //         new(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
    //         new(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
    //     };
    //
    //     foreach (var ray in rays)
    //     {
    //         Gizmos.DrawRay(ray.origin, ray.direction * maxDistance); // 길이 1m로 Ray 표시
    //     }
    // }
}