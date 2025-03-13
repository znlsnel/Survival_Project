using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;  // ���� �Է� ��
    public float jumpPower;
    public LayerMask groundLayerMask;  // ���̾� ����


    private Rigidbody rigidbody;
    private PlayerCondition playerCondition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerCondition = GetComponent<PlayerCondition>();
    }

  

    // ���� ����
    private void FixedUpdate()
    {
        Move();
    }

    // ī�޶� ���� -> ��� ������ ������ ī�޶� ������
  

    // �Է°� ó��
   

    // �Է°� ó��
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
        // ���� �Է��� y ���� z ��(forward, �յ�)�� ���Ѵ�.
        // ���� �Է��� x ���� x ��(right, �¿�)�� ���Ѵ�.
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;  // ���⿡ �ӷ��� �����ش�.
        dir.y = rigidbody.velocity.y;  // y���� velocity(��ȭ��)�� y ���� �־��ش�.

        rigidbody.velocity = dir;  // ����� �ӵ��� velocity(��ȭ��)�� �־��ش�.
    }


    bool IsGrounded()
    {
        // 4���� Ray�� �����.
        // �÷��̾�(transform)�� �������� �յ��¿� 0.2�� ����߷���.
        // 0.01 ���� ��¦ ���� �ø���.
        // ���̶���Ʈ �κ��� �������� �� �� �κ��� ������ �м��غ�����.
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        // 4���� Ray �� groundLayerMask�� �ش��ϴ� ������Ʈ�� �浹�ߴ��� ��ȸ�Ѵ�.
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
