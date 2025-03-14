using System;
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
	private Dictionary<ESlotType, List<ItemSlot>> itemSlots = new Dictionary<ESlotType, List<ItemSlot>>();
	private Dictionary<ESlotType, List<ItemDataSO>> myItems = new Dictionary<ESlotType, List<ItemDataSO>>();

	// === Accessible Lists ===
	public List<ItemDataSO> MyItems => myItems[ESlotType.InventorySlot]; 
    public List<ItemDataSO> QuickSlotItems => myItems[ESlotType.QuickSlot];
	public List<ItemSlot> MyItemSlots => itemSlots[ESlotType.InventorySlot];
	public List<ItemSlot> QuickSlots => itemSlots[ESlotType.QuickSlot];

	// === Event ===
	public event Action onChangedSlot;
	private void Awake()
	{
		foreach (ESlotType type in Enum.GetValues(typeof(ESlotType)))
		{
			myItems.Add(type, new List<ItemDataSO>());
			itemSlots.Add(type, new List<ItemSlot>());
		}
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
        for (int i = 0; i < MyItems.Count; i++)
        {
            bool canStack = item.CanStackItems && item.MaxStackCount > GetSlotStackAmount(ESlotType.InventorySlot, i);
            if (MyItems[i] == item && canStack)
				return (ESlotType.InventorySlot, i);
        }

		for (int i = 0; i < QuickSlotItems.Count; i++)
        {
            bool canStack = item.CanStackItems && item.MaxStackCount > GetSlotStackAmount(ESlotType.QuickSlot, i);
			if (QuickSlotItems[i] == item && canStack)
				return (ESlotType.QuickSlot, i); 
		}
			

        return (ESlotType.None, -1);
	}

    public bool AddItem(ItemDataSO item)
    {
		var (type, idx) = FindItem(item);
		if (idx > -1)
			itemSlots[type][idx].StackAmount++;
		
		else if ((idx = GetEmptySlotIdx()) == -1)
			return false;

		else
			MyItems[idx] = item;
		
		onChangedSlot?.Invoke();
		return true;
	}

	public void SwitchSlot(SlotInfo slotA, SlotInfo slotB)
	{
		ItemDataSO temp = myItems[slotA.type][slotA.idx];
		myItems[slotA.type][slotA.idx] = myItems[slotB.type][slotB.idx];
		myItems[slotB.type][slotB.idx] = temp;

		int cnt = itemSlots[slotA.type][slotA.idx].StackAmount;
		itemSlots[slotA.type][slotA.idx].StackAmount = itemSlots[slotB.type][slotB.idx].StackAmount;
		itemSlots[slotB.type][slotB.idx].StackAmount = cnt;
		onChangedSlot?.Invoke();
	}

	public int GetSlotStackAmount(ESlotType type, int idx) => itemSlots[type][idx].StackAmount;
}
 