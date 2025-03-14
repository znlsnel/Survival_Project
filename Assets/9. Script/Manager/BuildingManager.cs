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
            Debug.LogError("StartPlacement: 전달된 BuildingData가 없음");
            return;
        }

        if (selectedBuilding.prefab == null)
        {
            Debug.LogError($"StartPlacement: {selectedBuilding.buildingName}의 프리팹이 설정되지 않음");
            return;
        }

        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }

        // 선택된 건축물의 원본 프리팹 -> 프리뷰
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

        if (currentPreview.CanPlace()) // 설치 가능 여부 확인
        {
            Instantiate(selectedBuilding.prefab, currentPreview.transform.position, currentPreview.transform.rotation);
            Destroy(currentPreview.gameObject);
            currentPreview = null;
            selectedBuilding = null;
        }
        else
        {
            Debug.Log("설치할 수 없습니다.");
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
