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



    public void BuildingModeChanged(bool isBuildingMode)
    {
        OnBuildingModeChanged?.Invoke(isBuildingMode);
    }

    public void RequestToggleBuildMode()
    {
        Debug.Log("이벤트 요청");
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
