using System.Collections.Generic;
using UnityEngine;

public class BuildingUIManager : MonoBehaviour
{
    [Header("Building Data")]
    [SerializeField] private List<BuildingData> allBuildings; // 모든 건설품 목록

    private BuildingUI buildingUI;

    private void Awake()
    {
        buildingUI = FindObjectOfType<BuildingUI>();
        LoadAllBuildings();
    }



    private void Start()
    {
        EventManager.Instance.OnBuildingModeChanged += HandleBuildingModeChange;

        if (buildingUI == null)
        {
            Debug.LogError("BuildingUI가 할당되지 않았습니다.");
            return;
        }

        buildingUI.InitializeUI(allBuildings);
    }

    private void HandleBuildingModeChange(bool isBuildingMode)
    {
        if (buildingUI != null)
            buildingUI.ToggleUI(isBuildingMode);
    }

    private void LoadAllBuildings() // 리소스 폴더에서 빌딩데이터들을 싸그리 가져와봅시더
    {
        allBuildings = new List<BuildingData>();

        BuildingData[] ladedBuildings = Resources.LoadAll<BuildingData>("BuildingData");

        allBuildings.AddRange(ladedBuildings);
    }
}
