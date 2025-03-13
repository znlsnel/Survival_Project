using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private BuildingPreview currentPreview;
    private BuildingData selectedBuilding;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.PlaceBuilding.performed += OnPlaceBuilding;
            InputManager.Instance.CancelBuild.performed += OnCancelBuilding;
        }

        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnStartBuildingRequested += StartPlacement;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.PlaceBuilding.performed -= OnPlaceBuilding;
            InputManager.Instance.CancelBuild.performed -= OnCancelBuilding;
        }

        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnStartBuildingRequested -= StartPlacement;
        }
    }

    public void StartPlacement(BuildingData buildingData)
    {
        if (buildingData == null)
        {
            Debug.LogError("StartPlacement: ���޵� BuildingData�� ����");
            return;
        }

        if (buildingData.prefab == null)
        {
            Debug.LogError($"StartPlacement: {buildingData.buildingName}�� �������� �������� ����");
            return;
        }

        selectedBuilding = buildingData;

        // ���õ� �ǹ��� ���������� ������ ����
        GameObject previewObject = Instantiate(buildingData.prefab);
        currentPreview = previewObject.AddComponent<BuildingPreview>();

    }


    private void OnPlaceBuilding(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        TryPlaceBuilding();
    }

    private void OnCancelBuilding(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        CancelPlacement();
    }

    private void TryPlaceBuilding()
    {
        if (currentPreview == null) return;

        if (currentPreview.CanPlace()) // ��ġ ���� ���� Ȯ��
        {
            Instantiate(selectedBuilding.prefab, currentPreview.transform.position, currentPreview.transform.rotation);
            Destroy(currentPreview.gameObject);
            currentPreview = null;
        }
        else
        {
            Debug.Log("��ġ�� �� �����ϴ�.");
        }
    }

    private void CancelPlacement()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
            currentPreview = null;
        }
    }

}
