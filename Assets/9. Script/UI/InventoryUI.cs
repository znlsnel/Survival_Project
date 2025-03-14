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
	enum GameObjects
	{
		movingSlot,
		InventoryScrollRect
	}
	enum Toggles
	{
		toggle_everything,
		toggle_weapon,
		toggle_consumableItem,
		toggle_resource,
	}
	#endregion
	 
	// === Item List ===
	private Dictionary<ESlotType, List<ItemSlot>> itemSlots = new Dictionary<ESlotType, List<ItemSlot>>();
	private Dictionary<ESlotType, List<ItemDataSO>> myItems = new Dictionary<ESlotType, List<ItemDataSO>>();
	  
	// === Handler ===
	private InventoryHandler inventory;
	private QuickSlotHandler quickSlot;

	// === Objects ===
	private GameObject movingSlot;
	private ScrollRect scrollRect;
	 
	// === Values ===
	private EItemType categoryType = EItemType.None;
	private Coroutine moveSlotCrt;
	private SlotInfo hoveredSlot;
	private SlotInfo clikedSlot; 
	private SlotInfo selectSlot;

	private void Start()
	{
		InputManager.Instance.Inventory.action.started += InputInventoryToggle;

		InitInventory();
		InitItemSlots();
		SetCategoryButton();
		CloseUI(); 
	}

	#region Inventory Function 
	private void InitInventory()
	{
		Bind<Transform>(typeof(Transforms));
		Bind<GameObject>(typeof(GameObjects));

		movingSlot = Get<GameObject>((int)GameObjects.movingSlot);
		scrollRect = Get<GameObject>((int)GameObjects.InventoryScrollRect).GetComponent<ScrollRect>();

	   // Find Components
	   inventory = FindFirstObjectByType<InventoryHandler>();
		quickSlot = FindFirstObjectByType<QuickSlotHandler>();

		// Bind List & List Clear
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
			SlotInit(slot, itemSlots[ESlotType.InventorySlot].Count, ESlotType.InventorySlot);

			itemSlots[ESlotType.InventorySlot].Add(slot);
			myItems[ESlotType.InventorySlot].Add(null);
		}

		// Quick Slot Setting
		foreach (Transform child in quickSlotParent)
		{
			ItemSlot slot = child.GetComponent<ItemSlot>();
			SlotInit(slot, itemSlots[ESlotType.QuickSlot].Count, ESlotType.QuickSlot);

			itemSlots[ESlotType.QuickSlot].Add(slot);
			myItems[ESlotType.QuickSlot].Add(null);
		}

		
	}
	private void SetCategoryButton()
	{
		Bind<Toggle>(typeof(Toggles));
		Bind<TextMeshProUGUI>(typeof(Texts));

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
	private void SlotInit(ItemSlot slot, int idx, ESlotType type)
	{
		slot.SetIcon(null); 
		slot.SlotType = type; 

		slot.onClick += (PointerEventData data) =>ClickSlot(idx, type);
		slot.onRelease += (PointerEventData data) => ReleaseSlot(idx, type);
		slot.onHoverEnter += (PointerEventData data) => HoverEnterSlot(idx, type);
		slot.onHoverExit += (PointerEventData data)=> HoverExitSlot(idx, type);
	} 
	private void InputInventoryToggle(InputAction.CallbackContext context)
	{
		if (gameObject.activeSelf)
			CloseUI(); 
		else
			OpenUI();
	}
	private void OpenUI()
	{
		UpdateItemInfo(); 
		gameObject.SetActive(true);
	}
	private void CloseUI()
	{
		gameObject.SetActive(false);
	}
	public void UpdateItemInfo()
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
	 
	#region Item Slot Control
	// Item Slot  
	private void ClickSlot(int idx, ESlotType type)
	{
		Debug.Log($"클릭 {idx} : 타입 : {type}");
		clikedSlot = new SlotInfo(idx, type);
		scrollRect.enabled = false;

		if (myItems[type][idx] != null)
			moveSlotCrt = StartCoroutine(MoveItem(0.5f));
	} 
	private void ReleaseSlot(int idx, ESlotType type)
	{
		Debug.Log($"클릭 취소 {idx} : 타입 : {type}");
		scrollRect.enabled = true;

		if (moveSlotCrt != null)
		{
			StopCoroutine(moveSlotCrt);
			moveSlotCrt = null;
		}  
		 
		// 아이템이 이동중이라면 슬롯의 아이템 데이터 교환
		if (movingSlot.activeSelf) 
		{
			if (hoveredSlot.type != ESlotType.None && clikedSlot.type != ESlotType.None)
				inventory.SwitchSlot(clikedSlot, hoveredSlot);


			movingSlot.SetActive(false);
			UpdateItemInfo(); 
		} 
		else
			selectSlot = clikedSlot;

		clikedSlot.type = ESlotType.None;
	}
	private void HoverEnterSlot(int idx, ESlotType type)
	{
		Debug.Log($"호버 {idx} : 타입 : {type}");
		hoveredSlot = new SlotInfo(idx, type); 
	}
	private void HoverExitSlot(int idx, ESlotType type)
	{
		hoveredSlot.type = ESlotType.None;
	}
	private IEnumerator MoveItem(float time)
	{
		yield return new WaitForSeconds(time);

		itemSlots[clikedSlot.type][clikedSlot.idx].SetIcon(null);
		movingSlot.transform.localPosition = movingSlot.transform.parent.InverseTransformPoint(Input.mousePosition);
		movingSlot.GetComponent<ItemSlot>().SetIcon(myItems[clikedSlot.type][clikedSlot.idx]);
		movingSlot.SetActive(true);

		while (true)
		{
			movingSlot.transform.localPosition = movingSlot.transform.parent.InverseTransformPoint(Input.mousePosition);
			yield return null;
		}
	}
	#endregion
}