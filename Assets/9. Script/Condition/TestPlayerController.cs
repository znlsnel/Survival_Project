using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;  // 현재 입력 값
    public float jumpPower;
    public LayerMask groundLayerMask;  // 레이어 정보


    private Rigidbody rigidbody;
    private PlayerCondition playerCondition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerCondition = GetComponent<PlayerCondition>();
    }

  

    // 물리 연산
    private void FixedUpdate()
    {
        Move();
    }

    // 카메라 연산 -> 모든 연산이 끝나고 카메라 움직임
  

    // 입력값 처리
   

    // 입력값 처리
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        // 현재 입력의 y 값은 z 축(forward, 앞뒤)에 곱한다.
        // 현재 입력의 x 값은 x 축(right, 좌우)에 곱한다.
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;  // 방향에 속력을 곱해준다.
        dir.y = rigidbody.velocity.y;  // y값은 velocity(변화량)의 y 값을 넣어준다.

        rigidbody.velocity = dir;  // 연산된 속도를 velocity(변화량)에 넣어준다.
    }


    bool IsGrounded()
    {
        // 4개의 Ray를 만든다.
        // 플레이어(transform)을 기준으로 앞뒤좌우 0.2씩 떨어뜨려서.
        // 0.01 정도 살짝 위로 올린다.
        // 하이라이트 부분의 차이점과 그 외 부분을 나눠서 분석해보세요.
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        // 4개의 Ray 중 groundLayerMask에 해당하는 오브젝트가 충돌했는지 조회한다.
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }


}
