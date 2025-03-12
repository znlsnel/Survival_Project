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


    public InputAction RotateAction => rotateAction;
    public InputAction PlaceAction => placeAction;
    public InputAction CancelAction => cancelAction;
    public InputAction ToggleAction => toggleAction;


    private void OnValidate()   // �̸�
    {
        if (inputActions != null)
        {
            var playerBuilding = inputActions.FindActionMap("PlayerBuilding");

            rotateAction = playerBuilding.FindAction("RotateObject");
            placeAction = playerBuilding.FindAction("PlaceObject");
            cancelAction = playerBuilding.FindAction("CancelBuild");
            toggleAction = playerBuilding.FindAction("ToggleBuildMode");


        }
    }
    private void OnEnable()
    {
        rotateAction.performed += RotateBuilding;
        placeAction.performed += PlaceBuilding;
        cancelAction.performed += CancelBuilding;
        toggleAction.performed += ToggleBuildMode;

        rotateAction.Enable();
        placeAction.Enable();
        cancelAction.Enable();
        toggleAction.Enable();
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
            Debug.Log($"���๰ ȸ�� {rotationInput}");
        }
    }

    public void PlaceBuilding(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("��ġ");
        }
    }

    public void CancelBuilding(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("���");
        }
    }
    public void ToggleBuildMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("��庯��");
        }
        EventManager.Instance.RequestToggleBuildMode();
    }
}
