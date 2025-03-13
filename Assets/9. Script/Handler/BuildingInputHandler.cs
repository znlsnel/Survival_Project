using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private InputAction rotateAction;
    [SerializeField] private InputAction placeAction;
    [SerializeField] private InputAction cancelAction;
    [SerializeField] private InputAction toggleAction;
    [SerializeField] private InputAction buildingAction;



    public InputAction RotateAction => rotateAction;
    public InputAction PlaceAction => placeAction;
    public InputAction CancelAction => cancelAction;
    public InputAction ToggleAction => toggleAction;
    public InputAction BuildingAction => buildingAction;



    private void OnValidate()   // 미리
    {
        if (inputActions != null)
        {
            var playerBuilding = inputActions.FindActionMap("PlayerBuilding");

            rotateAction = playerBuilding.FindAction("RotateObject");
            placeAction = playerBuilding.FindAction("PlaceObject");
            cancelAction = playerBuilding.FindAction("CancelBuild");
            toggleAction = playerBuilding.FindAction("ToggleBuildMode");
            buildingAction = playerBuilding.FindAction("StartBuilding");

        }
    }
    private void OnEnable()
    {
        rotateAction.performed += RotateBuilding;
        placeAction.performed += PlaceBuilding;
        cancelAction.performed += CancelBuilding;
        toggleAction.performed += ToggleBuildMode;
        buildingAction.performed += StartBuilding;


        rotateAction.Enable();
        placeAction.Enable();
        cancelAction.Enable();
        toggleAction.Enable();
        buildingAction.Enable();

    }

    private void OnDisable()
    {
        rotateAction.performed -= RotateBuilding;
        placeAction.performed -= PlaceBuilding;
        cancelAction.performed -= CancelBuilding;
        toggleAction.performed -= ToggleBuildMode;
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
            Debug.Log("배치");
        }
    }

    public void CancelBuilding(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("취소");
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
