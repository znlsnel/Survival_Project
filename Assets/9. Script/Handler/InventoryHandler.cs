using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

	// === Component ===
	private MessageUI messageUI;
	 
	private void OnValidate()
	{
		foreach (ESlotType type in Enum.GetValues(typeof(ESlotType)))
		{
			myItems.Add(type, new List<ItemDataSO>());
			itemSlots.Add(type, new List<ItemSlot>()); 
		}
	}
	private void Start()
	{
		messageUI = GetComponent<PlayerUIHandler>().MessageUI;
	}

	private (ESlotType, int) GetEmptySlotIdx()
    {
		for (int i = 0; i < QuickSlotItems.Count; i++)
			if (QuickSlotItems[i] == null)
				return (ESlotType.QuickSlot, i); 

		for (int i = 0; i < MyItems.Count; i++)
            if (MyItems[i] == null)
				return (ESlotType.InventorySlot, i);

		return (ESlotType.InventorySlot, -1);
	}

    private (ESlotType, int) FindItem(ItemDataSO item)
    {
        for (int i = 0; i < MyItems.Count; i++)
        {
			bool canStack = item.CanStackItems && item.MaxStackCount > itemSlots[ESlotType.InventorySlot][i].StackAmount;
            if (MyItems[i] == item && canStack)
				return (ESlotType.InventorySlot, i);
        }

		for (int i = 0; i < QuickSlotItems.Count; i++)
        {
			bool canStack = item.CanStackItems && item.MaxStackCount > itemSlots[ESlotType.QuickSlot][i].StackAmount;
			if (QuickSlotItems[i] == item && canStack)
				return (ESlotType.QuickSlot, i); 
		}
			

        return (ESlotType.None, -1);
	} 

    public bool AddItem(ItemDataSO item)
    {
		var (type, idx) = FindItem(item);
		if (idx > -1)
		{
			itemSlots[type][idx].StackAmount++;
		}
		else
		{
			(type, idx) = GetEmptySlotIdx();
			if (idx == -1)
				return false;
			else
				myItems[type][idx] = item;
		}

		onChangedSlot?.Invoke();
		messageUI.AddItem(item); 
		QuestManager.ProgressQuest(EQuestCategory.Pickup, item.ItemName); 

		
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
	public void RemoveItem(ItemDataSO data)
	{
		var (type, idx) = FindItem(data);
		if (idx == -1)
			return;

		RemoveItem(type, idx);
	}
	 
	public void RemoveItem(ESlotType type, int idx)
	{
		ItemSlot itemslot = itemSlots[type][idx];
		itemslot.StackAmount -= 1;

		if (itemslot.StackAmount <= 0)
		{
			myItems[type][idx] = null;
			itemslot.StackAmount = 1;
		}
		

		onChangedSlot?.Invoke();
	}

	public bool HasItem(ItemDataSO data)
	{
		foreach ( var item in MyItems)
			if (item == data)
				return true;
		
		foreach ( var item in QuickSlotItems)
			if (item == data)
				return true;


		return false;
	}

}
