using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController: MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float radius = 5f; // 타겟을 중심으로 카메라 position 반경
    [SerializeField] private float sensitivity = 3f; // 회전 감지 민감도
    [SerializeField] private float height = 5f; // 플레이어를 기준으로 얼마나 위에서 찍을 지 고정

    private Vector2 _cumulativeMoveAmount = Vector2.zero; // 누적 마우스 델타 이동 값
    private float _computedRadius = 5f;

    private void Start()
    {
        if(!target) throw new UnityException("camera target is null");
    } 

    
    void FixedUpdate()
    {
        Vector2 inputValue = Mouse.current.delta.ReadValue();
        
        var currentHeight = target.position.y + height;
        
        _cumulativeMoveAmount.x += inputValue.x * sensitivity; // 회전 각도 누석
        _cumulativeMoveAmount.x %= 360; // 회전 값만 파악
        
        Quaternion currentRotation = Quaternion.Euler(0, _cumulativeMoveAmount.x, 0);

        // bug: 모션이 발생하는 도중 회전이 생길 경우
        // if (inputValue != Vector2.zero) // 마우스 회전 발생 시에만 동기화.
        // {
            // target.rotation = currentRotation;
        // }
        
        // 상하 회전에 의해 높이 변경 및 반경 변동 발생
        // refactor : 감도 계산 필요
        _cumulativeMoveAmount.y += inputValue.y;
        _cumulativeMoveAmount.y = Mathf.Clamp(_cumulativeMoveAmount.y, -3, 3);
        currentHeight += _cumulativeMoveAmount.y;
        _computedRadius = radius - (Mathf.Abs(_cumulativeMoveAmount.y / 3)); // 높이 값에 의한 반경 변경 발생 
        
        
        Vector3 offset = currentRotation * Vector3.back * _computedRadius; // 반경만큼 플레이어 뒤쪽에서 촬영
        
        Vector3 targetPosition = new Vector3(target.position.x + offset.x, currentHeight, target.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * 8f);

        transform.LookAt(target);
    }
}
