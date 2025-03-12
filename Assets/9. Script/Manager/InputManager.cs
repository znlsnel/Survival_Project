using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	[Header("Input Info")]
	[SerializeField] private InputActionAsset inputSystem;
	[SerializeField] private InputActionReference move;
	[SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference toggleBuilding;


    [SerializeField] private PlayerInput playerInput;	// �÷��̾� ��ǲ����
    [SerializeField] private BuildingInputHandler buildingInputHandler;	// ���࿡ ���̴� �ڵ鷯


    public InputActionReference Move => move; 
	public InputActionReference Jump => jump;
    public InputActionReference ToggleBuilding => toggleBuilding;


    public InputAction RotateBuilding => buildingInputHandler?.RotateAction;
    public InputAction PlaceBuilding => buildingInputHandler?.PlaceAction;
    public InputAction CancelBuild => buildingInputHandler?.CancelAction;


    protected override void Awake()
	{
		base.Awake();
		inputSystem.Enable();
    }

	public static void SetActive(bool active)
	{
		if (active)
			Instance.inputSystem.Enable(); 
		else
			Instance.inputSystem.Disable();
	}



    // ���߿� �Ϲ� �ڵ鷯 ����� �ű�Կ�
    private void OnEnable()
    {
        if (toggleBuilding != null)
        {
            toggleBuilding.action.performed += ToggleBuildMode;
            toggleBuilding.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (toggleBuilding != null)
        {
            toggleBuilding.action.performed -= ToggleBuildMode;
            toggleBuilding.action.Disable();
        }
    }
    public void ToggleBuildMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            EventManager.Instance.RequestToggleBuildMode();
        }
    }
}
