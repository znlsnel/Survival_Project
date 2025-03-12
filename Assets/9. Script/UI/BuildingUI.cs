using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button funiButton;
    [SerializeField] private Button decoButton;
    [SerializeField] private GameObject buildingMenuUI;
    [SerializeField] private Transform listBG;      // 리스트
    [SerializeField] private GameObject buttonPrefab;   // 건설품 버튼
    [SerializeField] private Transform resourceListBG;  // 요구 자원 리스트
    [SerializeField] private GameObject resourceItemPrefab;


    private BuildingUIManager buildingUIManager;

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

        foreach (Transform child in resourceListBG)
        {
            Destroy(child.gameObject);
        }

        foreach (ResourceCost resource in building.cost)
        {
            GameObject newResourceItem = Instantiate(resourceItemPrefab, resourceListBG);

            Image icon = newResourceItem.GetComponentInChildren<Image>();
            if (icon != null && resource.resourceItem.ItemIcon != null)
            {
                icon.sprite = resource.resourceItem.ItemIcon;
            }

            TextMeshProUGUI amountText = newResourceItem.GetComponentInChildren<TextMeshProUGUI>();
            if (amountText != null)
            {
                amountText.text = $"{resource.resourceItem.ItemName} - {resource.amount}";
            }
        }

    }


}
