using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Toggle allItemsToggle;  // 전체보기
    [SerializeField] private Toggle funiToggle;  // 가구 버튼
    [SerializeField] private Toggle decoToggle;  // 장식 버튼
    [SerializeField] private Transform listBG;  // 건축물 리스트 배경
    [SerializeField] private GameObject buttonPrefab;  // 건축물 버튼 프리팹
    [SerializeField] private GameObject toolTip;  // 건축물 버튼 프리팹


    private BuildingUIManager buildingUIManager; 
    private GameObject lastSelectedButton;  // 마지막으로 선택된 버튼
    private List<BuildingData> allBuildings = new List<BuildingData>();  // 전체 건축물 데이터


    public void InitializeUI(List<BuildingData> buildings)
    {
        allBuildings = buildings;

        funiToggle.onValueChanged.AddListener((isOn) => UpdateBuildingList());
        decoToggle.onValueChanged.AddListener((isOn) => UpdateBuildingList());

        // 기본적으로 가구 카테고리 활성화
        funiToggle.isOn = true;
        decoToggle.isOn = false;
        UpdateBuildingList();
    }
    private void UpdateBuildingList()
    {
        int index = 0;
        bool showAllItems = allItemsToggle.isOn;

        // 현재 UI 아이템 목록 가져오기
        foreach (BuildingData building in allBuildings)
        {
            bool isFurniture = funiToggle.isOn && building.buildingType == BuildingType.Furniture;
            bool isDecoration = decoToggle.isOn && building.buildingType == BuildingType.Decoration;

            if (showAllItems || isFurniture || isDecoration)
            {
                GameObject item;

                // 기존 아이템이 있다면 재사용, 없으면 새로 생성
                if (index < listBG.childCount)
                {
                    item = listBG.GetChild(index).gameObject;
                    item.SetActive(true);
                }
                else
                {
                    item = Instantiate(buttonPrefab, listBG);
                }

                item.name = $"BuildingButton_{building.buildingName}";


                // 토글 그룹 설정
                ToggleGroup parentToggleGroup = listBG.GetComponentInParent<ToggleGroup>();
                if (parentToggleGroup == null)
                {
                    return;
                }

                Toggle itemToggle = item.GetComponent<Toggle>();
                if (itemToggle != null)
                {
                    itemToggle.group = parentToggleGroup; // 부모의 `ToggleGroup` 사용
                    itemToggle.isOn = false;  // 기본적으로 선택되지 않도록 설정

                    itemToggle.onValueChanged.AddListener((isSelected) =>
                    {
                        if (isSelected)
                        {
                            SelectBuilding(building);
                        }
                    });
                }

                Image itemItemIcon = item.transform.Find("Item").GetComponent<Image>();
                if (itemItemIcon != null)
                {
                    itemItemIcon.sprite = building.buildingIcon;
                }

                InitializeTooltip(item, building);

                index++;
            }
        }

        // 남은 아이템 숨기기
        for (int i = index; i < listBG.childCount; i++)
        {
            listBG.GetChild(i).gameObject.SetActive(false);
        }
    }


    private void SelectBuilding(BuildingData building)
    {
        Debug.Log($"선택된 건축물: {building.buildingName}");
        BuildingManager.Instance.SetSelectedBuilding(building);
    }


    private void InitializeTooltip(GameObject item, BuildingData building)
    {
        Transform tooltipTransform = item.transform.Find("Tooltip-Multiline");
        if (tooltipTransform == null)
        {
            Debug.Log($"툴팁 없음");

            return;
        }

        TextMeshProUGUI titleText = tooltipTransform.Find("Label-Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bodyText = tooltipTransform.Find("Label-Body").GetComponent<TextMeshProUGUI>();

        if (titleText != null)
        {
            titleText.text = building.buildingName; // 아이템 이름 적용
        }

        if (bodyText != null)
        {
            bodyText.text = building.description; // 아이템 설명 적용
        }
    }

}
