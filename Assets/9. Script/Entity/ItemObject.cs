using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractableObject
{
	[SerializeField] private ItemDataSO itemData;

	private InventoryHandler inventoryHandler;
	private void Awake()
	{
		inventoryHandler = FindFirstObjectByType<InventoryHandler>();
	}
	public void Interaction()
	{
		inventoryHandler.AddItem(itemData);
	}

	 
}
