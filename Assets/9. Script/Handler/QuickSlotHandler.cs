using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotHandler : MonoBehaviour
{
	// === List ===
	private List<ItemDataSO> myItems;

	// === Value ===
	private int selectItem = 0;
	
	private void Awake()
	{
		InputManager.inputNumber += (num) => selectItem = num;
		InventoryHandler inventory = FindFirstObjectByType<InventoryHandler>();
		myItems = inventory.QuickSlotItems; 
	} 
	  
	public ItemDataSO GetItemData() => myItems[selectItem];
}
