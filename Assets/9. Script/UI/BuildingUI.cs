using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button funiButton;
    [SerializeField] private Button decoButton;
    [SerializeField] private GameObject buildingMenuUI; // 메뉴 ui
    [SerializeField] private GameObject buildingDesUI;  // 설명 ui (소비량)

    [SerializeField] private Transform listBG;      // 리스트
    [SerializeField] private GameObject buttonPrefab;   // 건설품 버튼
    [SerializeField] private Transform resourceListBG;  // 요구 자원 리스트

    [SerializeField] private BuildingResourceUI resourceUI;


    private BuildingUIManager buildingUIManager;

    private BuildingData selectedBuilding;      // 짐 선택된 건축물 데이터

    public void InitializeUI(BuildingUIManager manager)
    {
        buildingUIManager = manager;

        funiButton.onClick.AddListener(() => buildingUIManager.ShowBuildingsByCategory(BuildingType.Furniture));
        decoButton.onClick.AddListener(() => buildingUIManager.ShowBuildingsByCategory(BuildingType.Decoration));

        ToggleUI(false);
    }

    public void ToggleUI(bool isBuildingMode)
    {
        buildingMenuUI.SetActive(isBuildingMode);
        buildingDesUI.SetActive(isBuildingMode);
    }

    public void UpdateBuildingList(List<BuildingData> allBuildings, BuildingType category)
    {
        foreach (Transform child in listBG)
        {
            Destroy(child.gameObject);
        }

        foreach (BuildingData building in allBuildings)
        {
            if (building.buildingType == category)
            {
                GameObject newButton = Instantiate(buttonPrefab, listBG);

                Image buttonImage = newButton.GetComponent<Image>();

                if (buttonImage != null && building.buildingIcon != null)
                {
                    buttonImage.sprite = building.buildingIcon;
                }
                else
                {
                    Debug.LogWarning($"이미지 변경 실패: {building.buildingName}");
                }

                Button btn = newButton.GetComponent<Button>();
                if (btn != null)
                    btn.onClick.AddListener(() => SelectBuilding(building));
            }
        }
    }



    private void SelectBuilding(BuildingData building)
    {
        Debug.Log($"선택된 건축물: {building.buildingName}");

        selectedBuilding = building;
        BuildingManager.Instance.SetSelectedBuilding(building);

        resourceUI.UpdateResourceUI(building);
    }


}
