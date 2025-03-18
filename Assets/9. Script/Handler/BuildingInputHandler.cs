using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingInputHandler : MonoBehaviour
{

    private void Start()
    {
        InputManager.RotateAction.started += RotateBuilding;
		InputManager.PlaceAction.started += PlaceBuilding;
		InputManager.CancelAction.started += CancelBuilding;
		InputManager.BuildingAction.started += StartBuilding;


		InputManager.GetInput(EPlayerInput.ToggleBuildMode).performed += ToggleBuildMode;
		InputManager.GetInput(EPlayerBuilding.ToggleBuildMode).performed += ToggleBuildMode;
    } 



    public void RotateBuilding(InputAction.CallbackContext context)
    {
            float rotationInput = context.ReadValue<float>();
            Debug.Log($"건축물 회전 {rotationInput}");
        
    }

    public void PlaceBuilding(InputAction.CallbackContext context)
    {

    }

    public void CancelBuilding(InputAction.CallbackContext context)
    {

    }
    public void ToggleBuildMode(InputAction.CallbackContext context)
    {

            EventManager.Instance.RequestToggleBuildMode();
        
    }

    public void StartBuilding(InputAction.CallbackContext context)
    {

            Debug.Log("빌딩시작");
            EventManager.Instance.RequestStartBuilding();
        
    }
}
