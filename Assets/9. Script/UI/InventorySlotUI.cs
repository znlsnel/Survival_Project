using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : UIEventHandler
{
	private ESlotType slotType;
	private Image itemImage;

	public ESlotType SlotType { get => slotType; set => slotType = value; }
	private void Awake()
	{
		itemImage = Util.FindChild<Image>(gameObject, "Item", true);
	}  
	  
	public void SetIcon(Sprite sprite)
	{
		itemImage.gameObject.SetActive(sprite != null); 
		itemImage.sprite = sprite;
	}  
}
