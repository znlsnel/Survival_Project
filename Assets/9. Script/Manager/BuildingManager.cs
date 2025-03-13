using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    private BuildingPreview currentPreview;
    private BuildingData selectedData;

    [SerializeField] private InventoryHandler InventoryHandler;

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

    public void StartPlacement()
    {
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

    }

    public void SetSelectedBuilding(BuildingData buildingData)
    {
        selectedData = buildingData;
    }

    private void OnPlaceBuilding(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (HasRequiredResource())
        {
            TryPlaceBuilding();
            ConsumeResources();
        }
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
            Instantiate(selectedData.prefab, currentPreview.transform.position, currentPreview.transform.rotation);
            Destroy(currentPreview.gameObject);
            currentPreview = null;
            selectedData = null;
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
