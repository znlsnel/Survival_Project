using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Input: MonoBehaviour
    {
        [HideInInspector] public Vector2 movementValue = Vector2.zero;
        private bool _isJumpPressed = false;
        public bool IsJumpPressed
        {
            get
            {
                bool prevValue = _isJumpPressed;
                _isJumpPressed = false;
                return prevValue;
            }
        }
        private bool _isDashPressed = false;
        public bool IsDashPressed
        {
            get
            {
                bool prevValue = _isDashPressed;
                _isDashPressed = false;
                return prevValue;
            }
        }
        
        [HideInInspector] public bool isClicked = false;
        [HideInInspector] public Vector2 mouseDelta;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            movementValue = context.ReadValue<Vector2>();
        }
    
        public void OnJump(InputAction.CallbackContext context)
        {
            _isJumpPressed = context.performed;
        }
    
        public void OnDash(InputAction.CallbackContext context)
        {
            _isDashPressed = context.performed;
        }
    
        public void OnClick(InputAction.CallbackContext context)
        {
            // "Press (Release Only)" 설정 필수
            if (context.phase == InputActionPhase.Canceled) 
            {
                isClicked = true;
            }
        }
        
        // 카메라에게 필요한 정보
        public void OnLook(InputAction.CallbackContext context)
        {
            mouseDelta = context.ReadValue<Vector2>();
        }
    }
}
