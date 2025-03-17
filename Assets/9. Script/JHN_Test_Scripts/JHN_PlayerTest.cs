using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        float moveX = UnityEngine.Input.GetAxis("Horizontal"); // A, D
        float moveZ = UnityEngine.Input.GetAxis("Vertical");   // W, S

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        float mouseX = UnityEngine.Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // ���Ʒ� ȸ�� ����

        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
