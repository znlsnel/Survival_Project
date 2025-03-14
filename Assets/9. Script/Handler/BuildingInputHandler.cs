using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingInputHandler : MonoBehaviour
{

    private void OnEnable()
    {
        InputManager.RotateAction.performed += RotateBuilding;
		InputManager.PlaceAction.performed += PlaceBuilding;
		InputManager.CancelAction.performed += CancelBuilding;
		InputManager.ToggleAction.performed += ToggleBuildMode;
		InputManager.BuildingAction.performed += StartBuilding;
    }

    private void OnDisable()
    {
		InputManager.RotateAction.performed -= RotateBuilding;
		InputManager.PlaceAction.performed -= PlaceBuilding;
		InputManager.CancelAction.performed -= CancelBuilding;
		InputManager.ToggleAction.performed -= ToggleBuildMode;
		InputManager.BuildingAction.performed -= StartBuilding;
	}


    public void RotateBuilding(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            float rotationInput = context.ReadValue<float>();
            Debug.Log($"건축물 회전 {rotationInput}");
        }
    }

    public void PlaceBuilding(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
        }
    }

    public void CancelBuilding(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
        }
    }
    public void ToggleBuildMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            EventManager.Instance.RequestToggleBuildMode();
        }
    }

    public void StartBuilding(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("빌딩시작");
            EventManager.Instance.RequestStartBuilding();
        }
    }
}
