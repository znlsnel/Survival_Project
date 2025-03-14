using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public enum EPlayerInput
{
	move,
	interaction,
	Inventory,
	ToggleBuildMode,
	Jump
}

public enum EPlayerBuilding
{
	RotateObject,
	PlaceObject,
	CancelBuild,
	StartBuilding,
	ToggleBuildMode,
}

public class InputManager : Singleton<InputManager>
{
	private Dictionary<EPlayerInput, InputAction> playerInputs = new Dictionary<EPlayerInput, InputAction>();
	private Dictionary<EPlayerBuilding, InputAction> buildingInputs = new Dictionary<EPlayerBuilding, InputAction>();

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

	// Input Action Map  
	private InputActionMap playerInputMap;
	private InputActionMap buildingInputMap;

	// === Input Actions ===
	static public event Action<int> inputNumber; 
	static public InputAction Interaction => Instance.playerInputs[EPlayerInput.interaction];
	static public InputAction Inventory => Instance.playerInputs[EPlayerInput.Inventory];
	static public InputAction Move => Instance.playerInputs[EPlayerInput.move];
	static public InputAction Jump => Instance.playerInputs[EPlayerInput.Jump];
	static public InputAction ToggleBuilding => Instance.playerInputMap.enabled ? 
		Instance.playerInputs[EPlayerInput.ToggleBuildMode] : Instance.buildingInputs[EPlayerBuilding.ToggleBuildMode];
	static public InputAction RotateAction => Instance.buildingInputs[EPlayerBuilding.RotateObject];
	static public InputAction ToggleAction => Instance.buildingInputs[EPlayerBuilding.ToggleBuildMode];
	static public InputAction CancelAction => Instance.buildingInputs[EPlayerBuilding.CancelBuild];
	static public InputAction PlaceAction => Instance.buildingInputs[EPlayerBuilding.PlaceObject];
	static public InputAction BuildingAction => Instance.buildingInputs[EPlayerBuilding.StartBuilding];

	static public InputAction GetInput(EPlayerInput type) => Instance.playerInputs[type];
	static public InputAction GetInput(EPlayerBuilding type) => Instance.buildingInputs[type];  

	private bool[] numKeyDown;

	private void OnValidate()
	{
		BindAction(); 
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
	private void OnDestroy()
	{
		inputSystem.Disable();
	}

	private void Start()
    {
		if (ToggleBuilding != null)
		{
			ToggleBuilding.performed += ToggleBuildMode;
			ToggleBuilding.Enable();
		}
	}


	private void BindAction()
	{
		string mapName = typeof(EPlayerInput).Name;
		if (mapName[0] == 'E')
			mapName = mapName.Substring(1);

		playerInputMap = inputSystem.FindActionMap(mapName);
		foreach (EPlayerInput type in Enum.GetValues(typeof(EPlayerInput)))
			playerInputs.Add(type, playerInputMap.FindAction(type.ToString()));
		 
		 
		  
		mapName = typeof(EPlayerBuilding).Name;
		if (mapName[0] == 'E')
			mapName = mapName.Substring(1);

		buildingInputMap  = inputSystem.FindActionMap(mapName);
		foreach (EPlayerBuilding type in Enum.GetValues(typeof(EPlayerBuilding)))
		{
			string name = type.ToString();
			buildingInputs.Add(type, buildingInputMap.FindAction(name));
		}

	} 
	public static void SetActive(bool active)
	{
		if (active)
			Instance.inputSystem.Enable(); 
		else
			Instance.inputSystem.Disable();
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
