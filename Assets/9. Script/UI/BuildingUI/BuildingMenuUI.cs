using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Toggle allItemsToggle;  // ��ü����
    [SerializeField] private Toggle funiToggle;  // ���� ��ư
    [SerializeField] private Toggle decoToggle;  // ��� ��ư
    [SerializeField] private Transform listBG;  // ���๰ ����Ʈ ���
    [SerializeField] private GameObject buttonPrefab;  // ���๰ ��ư ������
    [SerializeField] private GameObject toolTip;  // ���๰ ��ư ������


    private BuildingUIManager buildingUIManager; 
    private GameObject lastSelectedButton;  // ���������� ���õ� ��ư
    private List<BuildingData> allBuildings = new List<BuildingData>();  // ��ü ���๰ ������


    public void InitializeUI(List<BuildingData> buildings)
    {
        allBuildings = buildings;

        funiToggle.onValueChanged.AddListener((isOn) => UpdateBuildingList());
        decoToggle.onValueChanged.AddListener((isOn) => UpdateBuildingList());

        // �⺻������ ���� ī�װ� Ȱ��ȭ
        funiToggle.isOn = true;
        decoToggle.isOn = false;
        UpdateBuildingList();
    }
    private void UpdateBuildingList()
    {
        int index = 0;
        bool showAllItems = allItemsToggle.isOn;

        // ���� UI ������ ��� ��������
        foreach (BuildingData building in allBuildings)
        {
            bool isFurniture = funiToggle.isOn && building.buildingType == BuildingType.Furniture;
            bool isDecoration = decoToggle.isOn && building.buildingType == BuildingType.Decoration;

            if (showAllItems || isFurniture || isDecoration)
            {
                GameObject item;

                // ���� �������� �ִٸ� ����, ������ ���� ����
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


                // ��� �׷� ����
                ToggleGroup parentToggleGroup = listBG.GetComponentInParent<ToggleGroup>();
                if (parentToggleGroup == null)
                {
                    return;
                }

                Toggle itemToggle = item.GetComponent<Toggle>();
                if (itemToggle != null)
                {
                    itemToggle.group = parentToggleGroup; // �θ��� `ToggleGroup` ���
                    itemToggle.isOn = false;  // �⺻������ ���õ��� �ʵ��� ����

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

        // ���� ������ �����
        for (int i = index; i < listBG.childCount; i++)
        {
            listBG.GetChild(i).gameObject.SetActive(false);
        }
    }


    private void SelectBuilding(BuildingData building)
    {
        Debug.Log($"���õ� ���๰: {building.buildingName}");
        BuildingManager.Instance.SetSelectedBuilding(building);
    }


    private void InitializeTooltip(GameObject item, BuildingData building)
    {
        Transform tooltipTransform = item.transform.Find("Tooltip-Multiline");
        if (tooltipTransform == null)
        {
            Debug.Log($"���� ����");

            return;
        }

        TextMeshProUGUI titleText = tooltipTransform.Find("Label-Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bodyText = tooltipTransform.Find("Label-Body").GetComponent<TextMeshProUGUI>();

        if (titleText != null)
        {
            titleText.text = building.buildingName; // ������ �̸� ����
        }

        if (bodyText != null)
        {
            bodyText.text = building.description; // ������ ���� ����
        }
    }

}
