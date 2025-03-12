using UnityEngine;

public class BuildingUIManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingMenuUI;
    [SerializeField] private GameObject buildingDecUI;


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
        buildingMenuUI.SetActive(false);
    }

    private void HandleBuildingModeChange(bool isBuildingMode)
    {
        buildingMenuUI.SetActive(isBuildingMode);
    }
}