using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : BaseUI
{
	enum Images
	{
		Item,
		ItemTypeImage,
	}

	enum TextMeshPros
	{
		amountText
	}
	enum GameObjects
	{
		IndicatorAmount,
		IndicatorType
	}

	private ESlotType slotType; 
	private Image itemImage;
	private Image itemTypeImage;
	private TextMeshProUGUI amountText;
	private GameObject amountIndicator;
	private GameObject itemTypeBg;

	public ESlotType SlotType { get => slotType; set => slotType = value; }
	private void Awake()
	{
		Bind<Image>(typeof(Images));
		Bind<TextMeshProUGUI>(typeof(TextMeshPros));
		Bind<GameObject>(typeof(GameObjects));
		 
		itemImage = GetImage((int)Images.Item);
		itemTypeImage = GetImage((int)Images.ItemTypeImage);
		amountText = Get<TextMeshProUGUI>((int)TextMeshPros.amountText);
		amountIndicator = Get<GameObject>((int)GameObjects.IndicatorAmount);
		itemTypeBg = Get<GameObject>((int)GameObjects.IndicatorType);
	}  
	  
	public void SetIcon(ItemDataSO data)
	{
		itemImage.gameObject.SetActive(data != null);
		itemTypeBg.gameObject.SetActive(data != null); 
		amountIndicator.gameObject.SetActive(data != null && data.CanStackItems);

		if (data == null)
			return;

		
		itemImage.sprite = data.ItemIcon;
		itemTypeImage.sprite = data.ItemTypeIcon;
		amountText.text = data.MaxStackCount.ToString(); 
	}  
}
