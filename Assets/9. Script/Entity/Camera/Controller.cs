using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCamera
{
    public class Controller: MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float radius = 5f; // 타겟을 중심으로 카메라 position 반경
        [SerializeField] private float sensitivity = 3f; // 회전 감지 민감도
        [SerializeField] private float verticalSensitivity = 3f; // 회전 감지 민감도
        [SerializeField] private float height = 5f; // 플레이어를 기준으로 얼마나 위에서 찍을 지 고정

        private Vector2 _cumulativeMoveAmount = Vector2.zero; // 누적 마우스 델타 이동 값
        private float _computedRadius = 5f;

        private void Start()
        {
            if(!target) throw new UnityException("camera target is null");
        }


        // fix: fixedUpdate 시 끊김 현상 발생
        void Update()
        {
            Vector2 inputValue = Mouse.current.delta.ReadValue();
        
            var currentHeight = target.position.y + height;
        
            _cumulativeMoveAmount.x += inputValue.x * sensitivity; // 회전 각도 누석
            _cumulativeMoveAmount.x %= 360; // 회전 값만 파악
        
            Quaternion currentRotation = Quaternion.Euler(0, _cumulativeMoveAmount.x, 0);
        
            // 상하 회전에 의해 높이 변경 및 반경 변동 발생
            // refactor : 감도 계산 필요
            _cumulativeMoveAmount.y += inputValue.y / verticalSensitivity;
            _cumulativeMoveAmount.y = Mathf.Clamp(_cumulativeMoveAmount.y, -2, 8);
            currentHeight -= _cumulativeMoveAmount.y / 2;
            _computedRadius = radius - (Mathf.Abs(_cumulativeMoveAmount.y / 6)); // 높이 값에 의한 반경 변경 발생 
        
        
            Vector3 offset = currentRotation * Vector3.back * _computedRadius; // 반경만큼 플레이어 뒤쪽에서 촬영
        
            Vector3 targetPosition = new Vector3(target.position.x + offset.x, currentHeight, target.position.z + offset.z);
            transform.position = targetPosition;
            // transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3f); // fix : lerp를 사용하면 거리간 직선 이동 발생

            transform.LookAt(target);
        }
    }
}
