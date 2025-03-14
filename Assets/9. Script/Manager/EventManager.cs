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

    public event Action OnStartBuildingRequested;

    public void BuildingModeChanged(bool isBuildingMode)
    {
        OnBuildingModeChanged?.Invoke(isBuildingMode);
    }

    public void RequestToggleBuildMode()
    {
        Debug.Log("이벤트 요청");
        OnToggleBuildModeRequested?.Invoke();
    }

    public void RequestStartBuilding()
    {
        OnStartBuildingRequested?.Invoke();
    }


}
