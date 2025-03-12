using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button funiButton;
    [SerializeField] private Button decoButton;
    [SerializeField] private GameObject buildingMenuUI;
    [SerializeField] private Transform listBG;      // ����Ʈ
    [SerializeField] private GameObject buttonPrefab;   // �Ǽ�ǰ ��ư

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
                    Debug.LogWarning($"�̹��� ���� ����: {building.buildingName}");
                }

                Button btn = newButton.GetComponent<Button>();
                if (btn != null)
                    btn.onClick.AddListener(() => SelectBuilding(building));
            }
        }
    }

    private void SelectBuilding(BuildingData building)
    {
        Debug.Log($"���õ� ���๰: {building.buildingName}");
    }
}
