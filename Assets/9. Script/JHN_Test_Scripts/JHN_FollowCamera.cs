using UnityEngine;

public class JHN_FollowCamera : MonoBehaviour
{
    public Transform target;  // 따라갈 대상 (플레이어)
    public Vector3 offset = new Vector3(0, 2, -4); // 카메라 위치 오프셋
    public float rotationSpeed = 3f; // 마우스 회전 속도

    private float currentX = 0f;
    private float currentY = 25f; // 시작 각도를 25도로 설정
    private float minY = -30f, maxY = 60f; // 위/아래 회전 제한

    private void Start()
    {
        // 초기 회전값 적용
        Quaternion initialRotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + initialRotation * offset;
        transform.rotation = initialRotation;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 마우스로 회전 (오른쪽 버튼을 누를 때만)
        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, minY, maxY);
        }

        // 회전 적용
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * offset;
        transform.rotation = rotation; // LookAt() 사용 안 함
    }
}
