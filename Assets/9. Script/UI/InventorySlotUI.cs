using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	private ESlotType slotType;
	private Image itemImage;

	public event Action onClick;
	public event Action onRelease;
	public event Action onHoverEnter;
	public event Action onHoverExit;
	public ESlotType SlotType { get => slotType; set => slotType = value; }
	private void Awake()
	{
		bool hasChild = transform.childCount > 0;
		itemImage = Util.FindChild<Image>(gameObject, "Item", true);
	}  
	 
	public void SetIcon(Sprite sprite)
	{
		itemImage.gameObject.SetActive(sprite != null); 
		itemImage.sprite = sprite;
	}

	public void OnPointerDown(PointerEventData eventData) => onClick?.Invoke();
	public void OnPointerUp(PointerEventData eventData)=> onRelease?.Invoke(); 
	public void OnPointerEnter(PointerEventData eventData) => onHoverEnter?.Invoke();
	public void OnPointerExit(PointerEventData eventData) => onHoverExit?.Invoke();
	
}
