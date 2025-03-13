using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	public event Action<PointerEventData> OnBeginDragHandler;
	public event Action<PointerEventData> OnDragHandler;
	public event Action<PointerEventData> onClick;
	public event Action<PointerEventData> onRelease;
	public event Action<PointerEventData> onHoverEnter;
	public event Action<PointerEventData> onHoverExit;

	public void OnBeginDrag(PointerEventData eventData) => OnBeginDragHandler?.Invoke(eventData);
	public void OnDrag(PointerEventData eventData) => OnDragHandler?.Invoke(eventData);
	public void OnPointerDown(PointerEventData eventData) => onClick?.Invoke(eventData);
	public void OnPointerUp(PointerEventData eventData) => onRelease?.Invoke(eventData);
	public void OnPointerEnter(PointerEventData eventData) => onHoverEnter?.Invoke(eventData);
	public void OnPointerExit(PointerEventData eventData) => onHoverExit?.Invoke(eventData);
}
