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
            Debug.LogError("StartPlacement: 전달된 BuildingData가 없음");
            return;
        }

        if (buildingData.prefab == null)
        {
            Debug.LogError($"StartPlacement: {buildingData.buildingName}의 프리팹이 설정되지 않음");
            return;
        }

        selectedBuilding = buildingData;

        // 선택된 건물의 프리팹으로 프리뷰 생성
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

        if (currentPreview.CanPlace()) // 설치 가능 여부 확인
        {
            Instantiate(selectedBuilding.prefab, currentPreview.transform.position, currentPreview.transform.rotation);
            Destroy(currentPreview.gameObject);
            currentPreview = null;
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
        }
    }

}
