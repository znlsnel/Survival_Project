using UnityEngine;

public class JHN_FollowCamera : MonoBehaviour
{
    public Transform target;  // ���� ��� (�÷��̾�)
    public Vector3 offset = new Vector3(0, 2, -4); // ī�޶� ��ġ ������
    public float rotationSpeed = 3f; // ���콺 ȸ�� �ӵ�

    private float currentX = 0f;
    private float currentY = 25f; // ���� ������ 25���� ����
    private float minY = -30f, maxY = 60f; // ��/�Ʒ� ȸ�� ����

    private void Start()
    {
        // �ʱ� ȸ���� ����
        Quaternion initialRotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + initialRotation * offset;
        transform.rotation = initialRotation;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ���콺�� ȸ�� (������ ��ư�� ���� ����)
        if (UnityEngine.Input.GetMouseButton(1))
        {
            currentX += UnityEngine.Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= UnityEngine.Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, minY, maxY);
        }

        // ȸ�� ����
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * offset;
        transform.rotation = rotation; // LookAt() ��� �� ��
    }
}
