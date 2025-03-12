using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	private Image itemImage;

	public event Action onClick;
	public event Action onRelease;
	public event Action onHoverEnter;
	public event Action onHoverExit;

	private void Awake()
	{
		bool hasChild = transform.childCount > 0;
		itemImage = hasChild ? transform.GetChild(0).GetComponent<Image>() : itemImage = GetComponent<Image>();
	}  

	public void SetIcon(Sprite sprite)
	{
		itemImage.gameObject.SetActive(sprite != null); 
		itemImage.sprite = sprite;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		onClick?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		onRelease?.Invoke(); 
	} 

	public void OnPointerEnter(PointerEventData eventData)
	{
		onHoverEnter?.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		onHoverExit?.Invoke();
	}
}
