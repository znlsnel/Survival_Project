using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputHandler: MonoBehaviour
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

    // 공격은 되는데 콤보를 어떻게 넣는가?
    public void OnClick(InputAction.CallbackContext context)
    {
        isClicked = context.performed; // notice: 기본적으로 누르고 있으면 지속 적용됨
    }
    
    // 카메라에게 필요한 정보
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}