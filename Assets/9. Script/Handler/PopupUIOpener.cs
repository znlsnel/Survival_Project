using Ricimi;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ButtonInfo
{
	public string Label;
	public bool ClosePopupWhenClicked;
	public bool IgnoreButtonClickedEvent;
	public Button.ButtonClickedEvent OnClickedEvent;
}

public class PopupUIOpener : MonoBehaviour
{
	[Header("Popup")]
	[SerializeField] private GameObject popupPrefab;
	
	[Header("Text")] 
	[SerializeField] public string title;
	[SerializeField] public string subTitle;
	[SerializeField, TextArea(3, 10)] public string description;

	[Header("Image")]
	[SerializeField] public Sprite image;
	[SerializeField] public Color32 tintColor = Color.white;
	[SerializeField] public string Caption;

	[Header("Button")]
	[SerializeField] public List<ButtonInfo> buttons = new List<ButtonInfo>(); 

	public void OpenPopup()
	{
		var popup = Instantiate(popupPrefab, transform, false);
		popup.SetActive(true);

		popup.transform.SetParent(GameManager.Instance.UIParent.transform);
		popup.transform.localPosition = Vector3.zero;
		popup.transform.localScale = Vector3.one; 
		popup.transform.eulerAngles = Vector3.zero;


		var popupModularUI = popup.GetComponent<PopupModularUI>();
		popupModularUI.Open();
		popupModularUI.Initialize(this);
	}
}
