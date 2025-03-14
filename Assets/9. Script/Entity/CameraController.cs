// comment: 시점을 자유롭게 변경하기 위해서 플레이어 외부에서 카메라 처리

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraController: MonoBehaviour
{
    void Update()
    {
        // if (!isLocked) return;
        // Debug.Log(_mouseDelta);
    }

    void FixedUpdate()
    {
        CameraLook();
        TraceTarget();
    }
    
    
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 1, -10);
    [SerializeField] private float followSpeed = 5f; // 따라가는 속도
    void TraceTarget()
    {
        if (!target) return; // 플레이어가 없으면 실행 X
        
        // 부드럽게 따라가는 이동
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    
    // cameraMovementController
    [SerializeField] float minXLook;  // 캠 회전 제한
    [SerializeField] float maxXLook;  // 캠 회전 제한
    private float camCurXRot;   // 카메라 회전 각도
    private float camCurYRot = 0f;

    [FormerlySerializedAs("lookSensitivity")] [SerializeField] float sensitivity = 1f;   // 마우스 감도
    public bool isLocked;

    // fix: 카메라의 역할이 아님
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // Cursor.lockState = CursorLockMode.None;
    }

    // fix: 화면을 벗어나도 회전하는 점에 대해서
    void CameraLook()
    {
        // 누적된 값으로 관리
        camCurXRot -= _mouseDelta.y * sensitivity;
        camCurYRot += _mouseDelta.x * sensitivity;
        
        // 상하 회전 제한 (필요 시)
        //camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        transform.rotation = Quaternion.Euler(camCurXRot, camCurYRot, 0);
    }
    
    // feat: cameraInputHandler
    private Vector2 _mouseDelta;
    public bool isFPV; // 시점 변경
    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    // UI 또는 게임 매니저에서 관리
    public void OnStop(InputAction.CallbackContext context)
    {
        
    }
}