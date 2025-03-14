using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class InventoryUI : BaseUI
{
	#region Binding Enum
	enum Texts
	{
		LabelTitle,
		LabelAmount
	}
	enum Transforms
	{
		itemSlotParent,
		quickSlotParent,
	} 

	enum Toggles
	{
		toggle_everything,
		toggle_weapon,
		toggle_consumableItem,
		toggle_resource,
	}
	enum GameObjects
	{
		PanelInventory
	}

	#endregion
	 
	// === Item List ===
	private Dictionary<ESlotType, List<ItemSlot>> itemSlots = new Dictionary<ESlotType, List<ItemSlot>>();
	private Dictionary<ESlotType, List<ItemDataSO>> myItems = new Dictionary<ESlotType, List<ItemDataSO>>();

	// === component ===
	private ItemSlotHandler itemSlotHandler;
	private InventoryHandler inventory; 
	 
	// === Values ===
	private EItemType categoryType = EItemType.None;
	private GameObject mainPanel;

	private void OnValidate()
	{
		Bind<Transform>(typeof(Transforms));
		Bind<Toggle>(typeof(Toggles));
		Bind<TextMeshProUGUI>(typeof(Texts));
		Bind<GameObject>(typeof(GameObjects));
		 
		inventory = FindFirstObjectByType<InventoryHandler>();
		itemSlotHandler = GetComponent<ItemSlotHandler>();
		mainPanel = Get<GameObject>((int)GameObjects.PanelInventory);
	}
	private void Awake()
	{
		InitItemList();
		InitItemSlots();
		SetCategoryButton();
	}
	private void Start()
	{
		InputManager.Instance.Inventory.action.started += InputInventoryToggle;
		inventory.onChangedSlot += UpdateItemInfo;
		CloseUI();
	}

	#region Inventory Function 
	private void InitItemList()
	{
		itemSlots.Add(ESlotType.InventorySlot, inventory.MyItemSlots);
		itemSlots.Add(ESlotType.QuickSlot, inventory.QuickSlots);
		myItems.Add(ESlotType.InventorySlot, inventory.MyItems);
		myItems.Add(ESlotType.QuickSlot, inventory.QuickSlotItems); 
	}

	private void InitItemSlots()
	{
		// Find Objects
		Transform itemSlotParent = Get<Transform>((int)Transforms.itemSlotParent);
		Transform quickSlotParent = Get<Transform>((int)Transforms.quickSlotParent);

		// Item Slot Setting
		foreach (Transform child in itemSlotParent)
		{
			ItemSlot slot = child.GetComponent<ItemSlot>();
			UIEventHandler evt = child.GetComponent<UIEventHandler>();
			SlotInit(slot, evt, itemSlots[ESlotType.InventorySlot].Count, ESlotType.InventorySlot);

			itemSlots[ESlotType.InventorySlot].Add(slot);
			myItems[ESlotType.InventorySlot].Add(null);
		}

		// Quick Slot Setting
		foreach (Transform child in quickSlotParent)
		{
			ItemSlot slot = child.GetComponent<ItemSlot>();
			UIEventHandler evt = child.GetComponent<UIEventHandler>();

			SlotInit(slot, evt,itemSlots[ESlotType.QuickSlot].Count, ESlotType.QuickSlot);

			itemSlots[ESlotType.QuickSlot].Add(slot);
			myItems[ESlotType.QuickSlot].Add(null);
		}

		
	}

	private void SetCategoryButton()
	{
		Action<EItemType, bool> action = (eItemType, active) => 
		{ 
			if (active) 
			{ 
				categoryType = eItemType;
				string nameText = "Everything";
				if (categoryType != EItemType.None)
					nameText = categoryType.ToString();

				Get<TextMeshProUGUI>((int)Texts.LabelTitle).text = nameText; 
				UpdateItemInfo(); 
			} 
		};

		Get<Toggle>((int)Toggles.toggle_consumableItem).onValueChanged.AddListener((active) => action.Invoke(EItemType.Consumable, active));
		Get<Toggle>((int)Toggles.toggle_everything).onValueChanged.AddListener((active) => action.Invoke(EItemType.None, active));
		Get<Toggle>((int)Toggles.toggle_resource).onValueChanged.AddListener((active) => action.Invoke(EItemType.Resource, active));
		Get<Toggle>((int)Toggles.toggle_weapon).onValueChanged.AddListener((active) => action.Invoke(EItemType.Weapon, active));
	}

	private void SlotInit(ItemSlot slot, UIEventHandler slotEvent, int idx, ESlotType type)
	{
		slot.SetIcon(null);

		slotEvent.onClick += (PointerEventData data) => itemSlotHandler.ClickSlot(idx, type);
		slotEvent.onRelease += (PointerEventData data) => itemSlotHandler.ReleaseSlot(idx, type);
		slotEvent.onHoverEnter += (PointerEventData data) => itemSlotHandler.HoverEnterSlot(idx, type);
		slotEvent.onHoverExit += (PointerEventData data)=> itemSlotHandler.HoverExitSlot(idx, type);
	} 

	private void InputInventoryToggle(InputAction.CallbackContext context)
	{
		if (mainPanel.activeSelf)
			CloseUI(); 
		else
			OpenUI();
	}

	private void OpenUI()
	{
		UpdateItemInfo();
		mainPanel.SetActive(true);
	}

	private void CloseUI()
	{
		mainPanel.SetActive(false);
	}

	private void UpdateItemInfo()
	{
		var textAmount = Get<TextMeshProUGUI>((int)Texts.LabelAmount);

		int cnt = 0;
		foreach (var slots in itemSlots)
		{
			for (int i = 0; i < slots.Value.Count; i++)
			{
				bool hasItem = myItems[slots.Key][i] != null;
				slots.Value[i].SetIcon(hasItem ? myItems[slots.Key][i] : null);
				slots.Value[i].gameObject.SetActive(true);

				if (hasItem)
					cnt++;
			}
		}

		if (categoryType != EItemType.None)
		{
			cnt = 0;
			for (int i = 0; i < itemSlots[ESlotType.InventorySlot].Count; i++)
			{
				ItemSlot slot = itemSlots[ESlotType.InventorySlot][i];
				ItemDataSO item = myItems[ESlotType.InventorySlot][i];

				bool usable = item != null && item.ItemType == categoryType;
				slot.gameObject.SetActive(usable);
				if (!usable)
					continue;

				cnt++;
				slot.SetIcon(item);
			}
		}

		textAmount.text = cnt.ToString();
	} 

	#endregion
}