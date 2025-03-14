using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [Header("UI OBJ")]
    [SerializeField] private GameObject buildingMenuUIOBJ; // �޴� ui
    [SerializeField] private GameObject buildingDesUIOBJ;  // ���� ui (�Һ�)

    [Header("UI CS")]   // CS���ϵ� �� ������Ʈ���� FIND�ص� �ɵ�?
    [SerializeField] private BuildingResourceUI resourceUI;
    [SerializeField] private BuildingMenuUI buildingMenuUI;


    private BuildingUIManager buildingUIManager;


    public void InitializeUI(BuildingUIManager manager, List<BuildingData> allBuildings)
    {
        // UI �Ŵ��� ����
        buildingUIManager = manager;

        // BuildingMenuUI �ʱ�ȭ (��ü ���๰ ����Ʈ ����)
        buildingMenuUI.InitializeUI(allBuildings);

        // �⺻������ UI ��Ȱ��ȭ
        ToggleUI(false);
    }

    public void ToggleUI(bool isBuildingMode)
    {
        buildingMenuUIOBJ.SetActive(isBuildingMode);
        buildingDesUIOBJ.SetActive(isBuildingMode);
    }



    private void SelectBuilding(BuildingData building)
    {
        Debug.Log($"���õ� ���๰: {building.buildingName}");

        BuildingManager.Instance.SetSelectedBuilding(building);

        resourceUI.UpdateResourceUI(building);
    }


}
