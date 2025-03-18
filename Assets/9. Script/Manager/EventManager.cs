using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventManager : Singleton<EventManager>
{
    public event Action<bool> OnBuildingModeChanged;

    public event Action OnToggleBuildModeRequested;

    public event Action<bool> OnCanBuildingRequested;   // bool값 필요없을지도?
    public event Action OnStartBuildingRequested;

    private bool isBuildMode = false;
    public bool IsBuildMode => isBuildMode;

	protected override void Awake()
	{
        base.Awake();
        InputManager.GetInput(EPlayerBuilding.ToggleBuildMode).started += ToggleBuildMode;
        InputManager.GetInput(EPlayerInput.ToggleBuildMode).started += ToggleBuildMode;

		BuildingModeChanged(false);
	}
     
	public void ToggleBuildMode(InputAction.CallbackContext context) => RequestToggleBuildMode();
	

	public void BuildingModeChanged(bool isBuildingMode)
    {
        isBuildMode = isBuildingMode;
		OnBuildingModeChanged?.Invoke(isBuildingMode);
    }

    public void RequestToggleBuildMode()
    {
        OnToggleBuildModeRequested?.Invoke();
    }

    public void RequestCanStartBuilding(bool canStart)
    {
        OnCanBuildingRequested?.Invoke(canStart);
    }

    public void RequestStartBuilding()
    {
        OnStartBuildingRequested?.Invoke();
    }
}
