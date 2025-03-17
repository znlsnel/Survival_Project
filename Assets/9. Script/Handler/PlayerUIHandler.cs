using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHandler : MonoBehaviour
{
	[SerializeField] private GameObject inventoryPrefab;
	[SerializeField] private GameObject quickSlotPrefab;
	[SerializeField] private GameObject ObjectInfoUIPrefab;

	private InventoryUI inventory;
	private QuickSlotUI quickSlot;
	private ObjectInfoUI objectInfoUI;

	public InventoryUI Inventory => inventory;
	public QuickSlotUI QuickSlot => quickSlot;
	public ObjectInfoUI ObjectInfoUI => objectInfoUI;

	private void Awake()
	{
		inventory = Instantiate(inventoryPrefab).GetComponent<InventoryUI>();	
		quickSlot = Instantiate(quickSlotPrefab).GetComponent<QuickSlotUI>();
		objectInfoUI = Instantiate(ObjectInfoUIPrefab).GetComponent<ObjectInfoUI>(); 
	}
}
