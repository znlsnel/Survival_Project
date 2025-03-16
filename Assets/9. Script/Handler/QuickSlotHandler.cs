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
	private ActiveItem currentItem; 

	private void Awake()
	{
		InputManager.inputNumber += SelectSlot; 
		InventoryHandler inventory = FindFirstObjectByType<InventoryHandler>();
		myItems = inventory.QuickSlotItems; 
	} 
	  
	public ItemDataSO GetItemData() => myItems[selectItem];
	 
	public void SelectSlot(int num)
	{
		if (selectItem == num-1)
			return;

		selectItem = num-1; 
		if (currentItem != null) 
			Destroy(currentItem.gameObject);
		 
		if (myItems[selectItem] != null && myItems[selectItem].IsUsableItem)
		{
			GameObject go = Instantiate<GameObject>(myItems[selectItem].ActiveItemPrefab);
			currentItem = go.GetComponent<ActiveItem>();	 
			go.transform.position = transform.position + transform.forward * 2.0f;
		}
	}
}
