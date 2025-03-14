using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	private KeyCode[] numKeyCodes = {
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
	};

	[Header("Input Info")]
	[SerializeField] private InputActionAsset inputSystem;
	[SerializeField] private InputActionReference move;
	[SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference toggleBuilding;
    [SerializeField] private PlayerInput playerInput;	// 플레이어 인풋연결
    [SerializeField] private BuildingInputHandler buildingInputHandler;	// 건축에 쓰이는 핸들러
	[SerializeField] private InputActionReference interaction;
	[SerializeField] private InputActionReference inventory;

	public event Action<int> inputNumber;

	public InputActionReference Interaction => interaction;
	public InputActionReference Inventory => inventory;
	public InputActionReference Move => move;
	public InputActionReference Jump => jump;
	public InputActionReference ToggleBuilding => toggleBuilding;
	public InputAction RotateBuilding => buildingInputHandler?.RotateAction;
	public InputAction PlaceBuilding => buildingInputHandler?.PlaceAction;
	public InputAction CancelBuild => buildingInputHandler?.CancelAction;

	private bool[] numKeyDown;
	

	private void OnDestroy()
	{
		inputSystem.Disable();
	}

    protected override void Awake()
	{
		base.Awake();
		inputSystem.Enable();
		numKeyDown = new bool[numKeyCodes.Length];

	}
	private void Update()
	{
		CheckInputNumber();
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
	 
    private void CheckInputNumber()
    {
		for (int i = 0; i < numKeyCodes.Length; i++)
		{
			if (Input.GetKeyDown(numKeyCodes[i]) && !numKeyDown[i])
			{
				numKeyDown[i] = true;
				inputNumber?.Invoke(i + 1);
			}

			if (Input.GetKeyUp(numKeyCodes[i]))
			{
				numKeyDown[i] = false;
			}
		}
	}
}
