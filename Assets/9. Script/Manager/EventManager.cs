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

    public void BuildingModeChanged(bool isBuilding)
    {
        OnBuildingModeChanged?.Invoke(isBuilding);
    }

    public void RequestToggleBuildMode()
    {
        OnToggleBuildModeRequested?.Invoke();
    }

}
