using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button funiButton;
    [SerializeField] private Button decoButton;

    [SerializeField] private GameObject buildingMenuUI;
    [SerializeField] private GameObject buildingDecUI;
    [SerializeField] private Transform listBG;  // ����Ʈ
    [SerializeField] private GameObject buttonPrefab;   // �Ǽ�ǰ ��ư


    [Header("Building Data")]
    [SerializeField] private List<BuildingData> allBuildings;   // ��� �Ǽ�ǰ�� ���


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
        funiButton.onClick.AddListener(() => ShowBuildingsByCategory(BuildingType.Furniture));
        decoButton.onClick.AddListener(() => ShowBuildingsByCategory(BuildingType.Decoration));

        ShowBuildingsByCategory(BuildingType.Furniture);
        buildingMenuUI.SetActive(false);
    }

    private void HandleBuildingModeChange(bool isBuildingMode)
    {
        buildingMenuUI.SetActive(isBuildingMode);
    }

    private void ShowBuildingsByCategory(BuildingType category)
    {
        if (listBG == null || buttonPrefab == null)
        {
            Debug.LogError("listBG �Ǵ� buttonPrefab�� �������� �ʾҽ��ϴ�.");
            return;
        }

        foreach (Transform child in listBG)
        {
            Destroy(child.gameObject);
        }

        foreach (BuildingData building in allBuildings)
        {
            if (building == null)
            {
                Debug.LogError("BuildingData�� null�Դϴ�.");
                continue;
            }

            if (building.buildingType == category)
            {
                GameObject newButton = Instantiate(buttonPrefab, listBG);
                Text buttonText = newButton.GetComponentInChildren<Text>();

                if (buttonText != null)
                    buttonText.text = building.buildingName;

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