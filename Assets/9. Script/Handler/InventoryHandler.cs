using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESlotType
{
	None,
	InventorySlot,
	QuickSlot
}

public struct SlotInfo
{
	public int idx;
	public ESlotType type;
	public SlotInfo(int idx, ESlotType type)
	{
		this.idx = idx;
		this.type = type;
	}
}

public class InventoryHandler : MonoBehaviour
{
	private List<ItemDataSO> myItems = new List<ItemDataSO>();
    private List<ItemDataSO> quickSlotItems = new List<ItemDataSO>();

    public List<ItemDataSO> MyItems => myItems; 
    public List<ItemDataSO> QuickSlotItems => quickSlotItems;

    private InventoryUI inventoryUI;

	private void Awake()
	{
		inventoryUI = FindFirstObjectByType<InventoryUI>(); 
	}
	private int GetEmptySlotIdx()
    {
        for (int i = 0; i < MyItems.Count; i++)
            if (MyItems[i] == null)
                return i;

        return -1;
    }
    private (ESlotType, int) FindItem(ItemDataSO item)
    {
        for (int i = 0; i < myItems.Count; i++)
        {
            bool canStack = item.CanStackItems && item.MaxStackCount > inventoryUI.GetSlotStackAmount(ESlotType.InventorySlot, i);
            if (myItems[i] == item && canStack)
				return (ESlotType.InventorySlot, i);
        }

		for (int i = 0; i < quickSlotItems.Count; i++)
        {
            bool canStack = item.CanStackItems && item.MaxStackCount > inventoryUI.GetSlotStackAmount(ESlotType.QuickSlot, i);
			if (quickSlotItems[i] == item && canStack)
				return (ESlotType.QuickSlot, i); 
		}
			

        return (ESlotType.None, -1);
	}
    public bool AddItem(ItemDataSO item)
    {
        var (type, idx) = FindItem(item);
        if (idx > -1)
        {
            inventoryUI.AddItem(type, idx);
            return true;
		}
         
	    idx = GetEmptySlotIdx();
		if (idx == -1)
			return false;

		MyItems[idx] = item;
		return true;
		
        
    }
}
