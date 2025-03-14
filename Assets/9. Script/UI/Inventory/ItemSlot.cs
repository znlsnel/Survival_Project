using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class ItemSlot : BaseUI
{
	enum CanvasGroups
	{
		ItemSelected
	}

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

	// === Component === 
	private TextMeshProUGUI amountText;
	private CanvasGroup selected;
	private Image itemImage;
	private Image itemTypeImage;

	// === Objects ===
	private GameObject amountIndicator;
	private GameObject itemTypeBg;

	// === Coroutine ===
	private Coroutine fadeEffect;

	// === Value ===
	private int stackAmount = 1;
	public int StackAmount
	{
		get => stackAmount; 
		set
		{
			stackAmount = value;
			UpdateStackAmount();
		}
	}


	private void Awake()
	{
		Bind<Image>(typeof(Images));
		Bind<TextMeshProUGUI>(typeof(TextMeshPros));
		Bind<GameObject>(typeof(GameObjects));
		Bind<CanvasGroup>(typeof(CanvasGroups));

		itemImage = GetImage((int)Images.Item);
		itemTypeImage = GetImage((int)Images.ItemTypeImage);
		amountText = Get<TextMeshProUGUI>((int)TextMeshPros.amountText);
		amountIndicator = Get<GameObject>((int)GameObjects.IndicatorAmount);
		itemTypeBg = Get<GameObject>((int)GameObjects.IndicatorType);
		selected = Get<CanvasGroup>((int)CanvasGroups.ItemSelected);
		UpdateStackAmount();
	}  

	private void UpdateStackAmount()
	{
		amountText.text = stackAmount.ToString();
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
	}  

	public void SelectSlot(bool active)
	{
		if (fadeEffect != null)
		{
			StopCoroutine(fadeEffect); 
			fadeEffect = null;
		}

		fadeEffect = StartCoroutine(Fade(0.1f, selected.alpha, active ? 1f : 0f));
	}
	 
	IEnumerator Fade(float duration, float start, float target)
	{
		float t = 0f;
		while(t < 1.0f)
		{
			selected.alpha = Mathf.Lerp(start, target, t);
			t += Time.deltaTime / duration;
			yield return null;
		}

		fadeEffect = null;
	}
} 
