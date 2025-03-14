using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{
	private List<ItemSlot> itemSlot;
	private List<ItemDataSO> itemDatas;

	private InventoryHandler inventory;
	private void Start()
	{
		inventory = FindFirstObjectByType<InventoryHandler>();	
		
	}
} 
