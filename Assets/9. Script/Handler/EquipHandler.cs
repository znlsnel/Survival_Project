using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipHandler : MonoBehaviour
{
	[SerializeField] private Transform equipPos;

	private ItemDataSO currentItem;
	private GameObject prevItem;

	public void EquipItem(ItemDataSO data) 
	{
		if (currentItem == data)
			return;

		if (prevItem !=null)
			Destroy(prevItem); 

		if (data == null || !data.IsActiveItem) 
		{
			currentItem = null;
			return;
		} 

		currentItem = data;
		prevItem = Instantiate(data.ActiveItemPrefab);
		prevItem.transform.SetParent(equipPos, false);
		prevItem.transform.localPosition = Vector3.zero; 
	}

	public ItemDataSO GetEquipedItem() => currentItem;
	
}
