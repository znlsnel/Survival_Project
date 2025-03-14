using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    private List<ItemDataSO> myItems = new List<ItemDataSO>();
    public List<ItemDataSO> MyItems => myItems; 

    public int GetEmptySlotIdx()
    {
        for (int i = 0; i < MyItems.Count; i++)
            if (MyItems[i] == null)
                return i;

        return -1;
    }

    public bool AddItem(ItemDataSO item)
    {
        int idx = GetEmptySlotIdx();
        if (idx == -1)
            return false;

        MyItems[idx] = item;
        return true; 
    }
    
}
