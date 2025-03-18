using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;
using Sprite = UnityEngine.Sprite;

public class PopupModularUI : PopupUI
{
	[Header ("Text")]
	[SerializeField] private TextMeshProUGUI title;
	[SerializeField] private TextMeshProUGUI subTitle;
	[SerializeField] private TextMeshProUGUI Caption;
	[SerializeField, TextArea(3, 10)] private TextMeshProUGUI description;

	[Header ("Image")]
	[SerializeField] private Image image;

	[Header ("Button")]
	[SerializeField] private GameObject ButtonGroup;
	[SerializeField] private List<Button> buttons;

	public void Initialize(PopupUIOpener opener)
	{

		SetLabel(title, opener.title);
		SetLabel(subTitle, opener.subTitle);
		SetLabel(description, opener.description);

		SetImage(image, opener.image, opener.tintColor);
		SetLabel(Caption, opener.Caption);

		foreach (var button in buttons)
			button.gameObject.SetActive(false);

		if (opener.buttons.Count == 0)
			ButtonGroup.SetActive(false);

		for (var i = 0; i < opener.buttons.Count; i++) 
			SetButton(buttons[i], opener.buttons[i]);
		
	}

	private void SetLabel(TextMeshProUGUI label, string text)
	{
		label.gameObject.SetActive(string.IsNullOrEmpty(text) == false);

		if (label == null)
			return;
		 
		label.text = text;
		label.gameObject.SetActive(!string.IsNullOrEmpty(text));
	}


	private void SetImage(Image image, Sprite sprite, Color32 color)
	{
		image.gameObject.SetActive(sprite != null);

		if (image == null)
			return;

		image.sprite = sprite;
		image.color = color;

		image.gameObject.SetActive(sprite != null);
	}

	private void SetButton(Button button, ButtonInfo info)
	{
		button.gameObject.SetActive(info != null);
		 
		if (button == null) 
			return;

		button.gameObject.SetActive(true);

		var label = button.GetComponentInChildren<TextMeshProUGUI>();
		if (label != null)
			label.text = info.Label;
		
		if (!info.IgnoreButtonClickedEvent)
			button.onClick = info.OnClickedEvent;
		
		if (info.ClosePopupWhenClicked)
			button.onClick.AddListener(Close);
	} 
}
 