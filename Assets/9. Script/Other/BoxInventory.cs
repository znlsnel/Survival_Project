using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventory : MonoBehaviour
{
    private List<ItemDataSO> storedItem = new List<ItemDataSO>();

    public void StoreItem(List<ItemDataSO> items)
    {
        if (items == null || items.Count <= 0) return;

        foreach (var item in items)
        {
            storedItem.Add(item);
            Debug.Log($"�ڽ���{item.ItemName}�߰�");
        }
    }

    public List<ItemDataSO> GetStoredItem()
    {
        return storedItem;
    }

}
