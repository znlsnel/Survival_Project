using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    private BuildingPreview currentPreview;
    private BuildingData selectedData;

    [SerializeField] private BuildingUI buildingUI;


    [SerializeField] private InventoryHandler InventoryHandler;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.PlaceAction.performed += OnPlaceBuilding;
            InputManager.CancelAction.performed += OnCancelBuilding;
        }

        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnStartBuildingRequested += StartPlacement; // 1. z들어왔어 체크해봐~
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
        if(!HasRequiredResource()) return;
        if (selectedData == null)
        {
            Debug.LogError("StartPlacement: 전달된 BuildingData가 없음");
            return;
        }

        if (selectedData.prefab == null)
        {
            Debug.LogError($"StartPlacement: {selectedData.buildingName}의 프리팹이 설정되지 않음");
            return;
        }

        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }


        // 선택된 건축물의 원본 프리팹 -> 프리뷰

        GameObject previewObject = Instantiate(selectedData.prefab);
        currentPreview = previewObject.AddComponent<BuildingPreview>();

        currentPreview.SetPreviewMode();

        //ㅇㅋ 됨 ㄱㄱ
        EventManager.Instance.RequestCanStartBuilding(true);



    }

    public void SetSelectedBuilding(BuildingData buildingData)
    {
        selectedData = buildingData;
        buildingUI.SelectBuilding(buildingData);
    }

    private void OnPlaceBuilding(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

        if (HasRequiredResource())
        {
            TryPlaceBuilding();
            ConsumeResources();
            UpdatePreviewColor();
        }
    }

    private void OnCancelBuilding(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        CancelPlacement();
        buildingUI.ToggleUI(true);
    }

    private void TryPlaceBuilding()
    {
        if (currentPreview == null) return;

        if (currentPreview.CanPlace()) // 설치 가능 여부 확인
        {
            Instantiate(selectedData.prefab, currentPreview.transform.position, currentPreview.transform.rotation);
            Destroy(currentPreview.gameObject);
            currentPreview = null;

            StartPlacement();
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
            selectedData = null;
        }
    }

    private void UpdatePreviewColor()
    {
        if (currentPreview == null) return;
        currentPreview.SetPreviewColor(HasRequiredResource());
        Debug.Log($"있냐? - {HasRequiredResource()}");
    }

    private bool HasRequiredResource()  // 있냐 없냐 체크
    {
        if (selectedData == null) return false;

        List<ItemDataSO> playerItemList = InventoryHandler.MyItems; // 아이템 받아옴


        foreach (var resouce in selectedData.cost)
        {
            if(resouce==null) continue;
            int itemCount = playerItemList.FindAll(item => item == resouce.resourceItem).Count;

            if (itemCount < resouce.amount) return false;

        }
        return true;
    }

    private void ConsumeResources()
    {
        if (selectedData == null) return;

        List<ItemDataSO> playerItemList = InventoryHandler.MyItems; // 아이템 받아옴

        foreach (var resouce in selectedData.cost)
        {
            int removeCount = resouce.amount;

            for (int i = 0; i < playerItemList.Count; i++)
            {
                if (playerItemList[i] == resouce.resourceItem)
                {
                    playerItemList.RemoveAt(i);
                    removeCount--;

                    if (removeCount == 0) break;
                }
            }
        }
    }

}
