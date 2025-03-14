using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickSlotUI : BaseUI
{
	enum GameObjects
	{
		slotParent
	}

	[SerializeField] private GameObject slotPrefab;

	private List<ItemSlot> quickSlot = new List<ItemSlot>();
	private List<ItemSlot> itemSlots;
	private List<ItemDataSO> itemDatas;

	private InventoryHandler inventory;

	private void OnValidate()
	{
		Bind<GameObject>(typeof(GameObjects));
		inventory = FindFirstObjectByType<InventoryHandler>();
	}

	private void Awake()
	{
		itemSlots = inventory.QuickSlots;
		itemDatas = inventory.QuickSlotItems;
		inventory.onChangedSlot += UpdateItemInfo;
	}
	private void Start()
	{
		InitItemList();
		UpdateItemInfo();
	}
	private void InitItemList()
	{
		Transform slotParent = Get<GameObject>((int)GameObjects.slotParent).transform;

		for (int i = 0; i < itemSlots.Count; i++)
		{
			var slot = Instantiate(slotPrefab);
			slot.transform.SetParent(slotParent, false);
		}

		foreach (Transform child in slotParent)
			quickSlot.Add(child.GetComponent<ItemSlot>());

	}
	private void UpdateItemInfo()
	{
		for (int i = 0; i < itemSlots.Count; i++)
		{
			if (itemDatas[i] == null)
				quickSlot[i].SetIcon(null);
			else
			{
				quickSlot[i].SetIcon(itemDatas[i]);
				quickSlot[i].StackAmount = itemSlots[i].StackAmount;
			}
		}
	}
} 
