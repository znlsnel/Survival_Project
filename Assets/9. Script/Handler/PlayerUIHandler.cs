using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHandler : MonoBehaviour
{
	[SerializeField] private GameObject inventoryPrefab;
	[SerializeField] private GameObject quickSlotPrefab;

	private InventoryUI inventory;
	private QuickSlotUI quickSlot;

	public InventoryUI Inventory => inventory;
	public QuickSlotUI QuickSlot => quickSlot; 
	private void Awake()
	{
		inventory = Instantiate(inventoryPrefab).GetComponent<InventoryUI>();	
		quickSlot = Instantiate(quickSlotPrefab).GetComponent<QuickSlotUI>();
	}
}
