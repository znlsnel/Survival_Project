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
            EventManager.Instance.OnStartBuildingRequested += StartPlacement; // 1. z���Ծ� üũ�غ�~
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
            Debug.LogError("StartPlacement: ���޵� BuildingData�� ����");
            return;
        }

        if (selectedData.prefab == null)
        {
            Debug.LogError($"StartPlacement: {selectedData.buildingName}�� �������� �������� ����");
            return;
        }

        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }


        // ���õ� ���๰�� ���� ������ -> ������

        GameObject previewObject = Instantiate(selectedData.prefab);
        currentPreview = previewObject.AddComponent<BuildingPreview>();

        currentPreview.SetPreviewMode();

        //���� �� ����
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

        if (currentPreview.CanPlace()) // ��ġ ���� ���� Ȯ��
        {
            Instantiate(selectedData.prefab, currentPreview.transform.position, currentPreview.transform.rotation);
            Destroy(currentPreview.gameObject);
            currentPreview = null;

            StartPlacement();
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
            selectedData = null;
        }
    }

    private void UpdatePreviewColor()
    {
        if (currentPreview == null) return;
        currentPreview.SetPreviewColor(HasRequiredResource());
        Debug.Log($"�ֳ�? - {HasRequiredResource()}");
    }

    private bool HasRequiredResource()  // �ֳ� ���� üũ
    {
        if (selectedData == null) return false;

        List<ItemDataSO> playerItemList = InventoryHandler.MyItems; // ������ �޾ƿ�


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

        List<ItemDataSO> playerItemList = InventoryHandler.MyItems; // ������ �޾ƿ�

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
