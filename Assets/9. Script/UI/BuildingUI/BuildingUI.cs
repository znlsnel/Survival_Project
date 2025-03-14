using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [Header("UI OBJ")]
    public GameObject buildingMenuUIOBJ; // �޴� ui
    public GameObject buildingDesUIOBJ;  // ���� ui (�Һ�)

    [Header("UI CS")]   // CS���ϵ� �� ������Ʈ���� FIND�ص� �ɵ�?
    [SerializeField] private BuildingResourceUI resourceUI;
    [SerializeField] private BuildingMenuUI buildingMenuUI;


    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnCanBuildingRequested += OnlyDecOnUI;  //OnStartBuildingRequested�� ���� dec�� ����
        }
    }

    public void InitializeUI(List<BuildingData> allBuildings)
    {
        // BuildingMenuUI �ʱ�ȭ (��ü ���๰ ����Ʈ ����)
        buildingMenuUI.InitializeUI(allBuildings);

        // �⺻������ UI ��Ȱ��ȭ
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
        Debug.Log($"���õ� ���๰: {building.buildingName}");
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
