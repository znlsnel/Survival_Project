using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventory : MonoBehaviour, IInteractableObject
{
    private List<ItemDataSO> storedItem = new List<ItemDataSO>();
    public event Action<List<ItemDataSO>> OnInventoryChanged;

    public event Action OnInventoryUIOpen;

    public void StoreItem(List<ItemDataSO> items)
    {
        if (items == null || items.Count <= 0) return;

        foreach (var item in items)
        {
            storedItem.Add(item);
            Debug.Log($"박스에{item.ItemName}추가");
        }

        OnInventoryChanged?.Invoke(storedItem);
    }

    public List<ItemDataSO> GetStoredItem()
    {
        return storedItem;
    }

    public void StoreItemDirectly(List<ItemDataSO> items)
    {
        if (items == null || items.Count == 0) return;

        storedItem.AddRange(items);
        items.Clear();
    }


    public void Interaction()
    {
        Debug.Log("aa");
        // 여기 박스 ui 열기
        OnInventoryUIOpen?.Invoke();
    }


}
