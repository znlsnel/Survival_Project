using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [Header("UI OBJ")]
    [SerializeField] private GameObject buildingMenuUIOBJ; // 메뉴 ui
    [SerializeField] private GameObject buildingDesUIOBJ;  // 설명 ui (소비량)

    [Header("UI CS")]   // CS파일도 걍 오브젝트에서 FIND해도 될듯?
    [SerializeField] private BuildingResourceUI resourceUI;
    [SerializeField] private BuildingMenuUI buildingMenuUI;


    private BuildingUIManager buildingUIManager;


    public void InitializeUI(BuildingUIManager manager, List<BuildingData> allBuildings)
    {
        // UI 매니저 설정
        buildingUIManager = manager;

        // BuildingMenuUI 초기화 (전체 건축물 리스트 전달)
        buildingMenuUI.InitializeUI(allBuildings);

        // 기본적으로 UI 비활성화
        ToggleUI(false);
    }

    public void ToggleUI(bool isBuildingMode)
    {
        buildingMenuUIOBJ.SetActive(isBuildingMode);
        buildingDesUIOBJ.SetActive(isBuildingMode);
    }



    private void SelectBuilding(BuildingData building)
    {
        Debug.Log($"선택된 건축물: {building.buildingName}");

        BuildingManager.Instance.SetSelectedBuilding(building);

        resourceUI.UpdateResourceUI(building);
    }


}
