using System.Collections.Generic;
using UnityEngine;

public class BuildingUIManager : MonoBehaviour
{
    [Header("Building Data")]
    [SerializeField] private List<BuildingData> allBuildings; // ��� �Ǽ�ǰ ���

    private BuildingUI buildingUI;

    private void Awake()
    {
        buildingUI = FindObjectOfType<BuildingUI>();
    }

    private void OnEnable()
    {
        EventManager.Instance.OnBuildingModeChanged += HandleBuildingModeChange;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBuildingModeChanged -= HandleBuildingModeChange;
    }

    private void Start()
    {
        if (buildingUI == null)
        {
            Debug.LogError("BuildingUI�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        buildingUI.InitializeUI(this);
        ShowBuildingsByCategory(BuildingType.Furniture);
    }

    private void HandleBuildingModeChange(bool isBuildingMode)
    {
        if (buildingUI != null)
            buildingUI.ToggleUI(isBuildingMode);
    }

    public void ShowBuildingsByCategory(BuildingType category)
    {
        if (buildingUI != null)
            buildingUI.UpdateBuildingList(allBuildings, category);
    }
}
