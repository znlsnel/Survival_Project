using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotHandler : MonoBehaviour
{
	private List<ItemDataSO> myItems;
	private EquipHandler equipHandler;
	private int selectItem = 0;

	private void Awake()
	{
		equipHandler = GetComponent<EquipHandler>();

		InputManager.inputNumber += SelectSlot; 
		InventoryHandler inventory = FindFirstObjectByType<InventoryHandler>();
		myItems = inventory.QuickSlotItems; 
	} 
	  
	public ItemDataSO GetSelectedItem() => myItems[selectItem];
	  
	public void SelectSlot(int num)
	{
		selectItem = num - 1;
		equipHandler.EquipItem(GetSelectedItem()); 
	}
}
