using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [Header("UI OBJ")]
    public GameObject buildingMenuUIOBJ; // 메뉴 ui
    public GameObject buildingDesUIOBJ;  // 설명 ui (소비량)

    [Header("UI CS")]   // CS파일도 걍 오브젝트에서 FIND해도 될듯?
    [SerializeField] private BuildingResourceUI resourceUI;
    [SerializeField] private BuildingMenuUI buildingMenuUI;


    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnCanBuildingRequested += OnlyDecOnUI;  //OnStartBuildingRequested가 오면 dec만 켜짐
        }
    }

    public void InitializeUI(List<BuildingData> allBuildings)
    {
        // BuildingMenuUI 초기화 (전체 건축물 리스트 전달)
        buildingMenuUI.InitializeUI(allBuildings);

        // 기본적으로 UI 비활성화
        buildingMenuUIOBJ.SetActive(false);
        buildingDesUIOBJ.SetActive(false);
    }

    public void ToggleUI(bool isBuildingMode)
    {
        if (isBuildingMode)
            buildingMenuUIOBJ.SetActive(isBuildingMode);
        else
        {
            buildingMenuUIOBJ.SetActive(isBuildingMode);

            buildingDesUIOBJ.SetActive(isBuildingMode);
        }

    }


    public void SelectBuilding(BuildingData building)
    {
        Debug.Log($"선택된 건축물: {building.buildingName}");
        resourceUI.UpdateResourceUI(building);

    }

    private void OnlyDecOnUI(bool canStart)
    {
        if (canStart)
        {
            buildingMenuUIOBJ.SetActive(false);
            buildingDesUIOBJ.SetActive(true);
        }
    }

    private void OnlyMenuUIOn(bool on)
    {
        buildingMenuUIOBJ.SetActive(on);
        buildingDesUIOBJ.SetActive(!on);
    }
}
