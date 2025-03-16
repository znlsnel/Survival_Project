using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PopupUI : MonoBehaviour
{
	[SerializeField] private Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);
    [SerializeField] private float destroyTime = 0.5f;
	private GameObject background;
	public void Open()
	{
		AddBackground();
	}

	public void Close()
	{
		var animator = GetComponent<Animator>();
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
		{
			animator.Play("Close");
		}

		RemoveBackground();
		StartCoroutine(RunPopupDestroy());
	}
	private IEnumerator RunPopupDestroy()
	{
		yield return new WaitForSeconds(destroyTime);
		Destroy(background);
		Destroy(gameObject);
	} 

	private void AddBackground()
	{
		var bgTex = new Texture2D(1, 1);
		bgTex.SetPixel(0, 0, backgroundColor);
		bgTex.Apply();

		background = new GameObject("PopupBackground");
		var image = background.AddComponent<Image>();
		var rect = new Rect(0, 0, bgTex.width, bgTex.height);
		var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
		// Clone the material, which is the default UI material, to avoid changing it permanently.
		image.material = new Material(image.material);
		image.material.mainTexture = bgTex;
		image.sprite = sprite;
		var newColor = image.color;
		image.color = newColor;
		image.canvasRenderer.SetAlpha(0.0f);
		image.CrossFadeAlpha(1.0f, 0.4f, false);

		var canvas = GetComponentInParent<Canvas>();
		background.transform.localScale = new Vector3(1, 1, 1);
		background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
		background.transform.SetParent(canvas.transform, false);
		background.transform.SetSiblingIndex(transform.GetSiblingIndex());

		var go =new GameObject("Module");
		go.AddComponent<PopupModularUI>();
	} 
	private void RemoveBackground()
	{
		var image = background.GetComponent<Image>();
		if (image != null)
		{
			image.CrossFadeAlpha(0.0f, 0.2f, false);
		}
	}
}
