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

    public event Action<BuildingData> OnStartBuildingRequested;

    private BuildingData selectedBuilding;



    public void BuildingModeChanged(bool isBuilding)
    {
        OnBuildingModeChanged?.Invoke(isBuilding);
    }

    public void RequestToggleBuildMode()
    {
        Debug.Log("이벤트 요청");
        OnToggleBuildModeRequested?.Invoke();
    }

    public void SetSelectedBuilding(BuildingData buildingData)
    {
        selectedBuilding = buildingData;
    }

    public void RequestStartBuilding()
    {
        if (selectedBuilding == null)
        {
            return;
        }
        Debug.Log($"이벤트 요청: {selectedBuilding.buildingName} 빌딩 시작");
        OnStartBuildingRequested?.Invoke(selectedBuilding);
    }
}
