using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    private BuildingPreview currentPreview;
    private BuildingData selectedBuilding;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.PlaceAction.performed += OnPlaceBuilding;
            InputManager.CancelAction.performed += OnCancelBuilding;
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
            InputManager.PlaceAction.performed -= OnPlaceBuilding;
            InputManager.CancelAction.performed -= OnCancelBuilding;
        }

        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnStartBuildingRequested -= StartPlacement;
        }
    }

    public void StartPlacement()
    {
        if (selectedBuilding == null)
        {
            Debug.LogError("StartPlacement: ���޵� BuildingData�� ����");
            return;
        }

        if (selectedBuilding.prefab == null)
        {
            Debug.LogError($"StartPlacement: {selectedBuilding.buildingName}�� �������� �������� ����");
            return;
        }

        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }

        // ���õ� ���๰�� ���� ������ -> ������
        GameObject previewObject = Instantiate(selectedBuilding.prefab);
        currentPreview = previewObject.AddComponent<BuildingPreview>();

        currentPreview.SetPreviewMode();

    }

    public void SetSelectedBuilding(BuildingData buildingData)
    {
        selectedBuilding = buildingData;
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
            selectedBuilding = null;
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
            selectedBuilding = null;
        }
    }


}
