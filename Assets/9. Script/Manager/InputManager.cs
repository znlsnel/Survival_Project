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


    [SerializeField] private PlayerInput playerInput;	// 플레이어 인풋연결
    [SerializeField] private BuildingInputHandler buildingInputHandler;	// 건축에 쓰이는 핸들러


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



    // 나중에 일반 핸들러 생기면 옮길게요
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
