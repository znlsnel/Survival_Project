using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	[SerializeField] private Image itemIcon;
	[SerializeField] private TextMeshProUGUI itemDescription;
	[SerializeField] private GameObject popup;


    private readonly string itemSlotParentName = "ItemSlotParent";
    private readonly string movingSlotName = "movingSlot";
    private readonly string dropButtonName = "DropButton";
    private readonly string cancelButtonName = "CancelButton";

	private List<InventorySlotUI> itemSlots = new List<InventorySlotUI>();
	private List<ItemDataSO> myItems;
	private GameObject movingSlot;

	private InventoryHandler inventory;

	private int selectItemIdx = -1;
	private int clikedButtonIdx = -1;
	private int hoveredButtonIdx = -1;
	private Coroutine moveSlotCrt;

	private void Awake()
	{
		InitInventory();
		InputManager.Instance.Inventory.action.started += InputInventoryKey;
		CloseUI();
	}

	void InitInventory()
	{
		movingSlot = Util.FindChildByName(transform, movingSlotName)?.gameObject;
		var throwButton = Util.FindChildByName(popup.transform, dropButtonName);
		var cancelButton = Util.FindChildByName(popup.transform, cancelButtonName);
		Transform itemSlotParent = Util.FindChildByName(transform, itemSlotParentName);


		throwButton.GetComponent<Button>().onClick.AddListener(DropItem);
		cancelButton.GetComponent<Button>().onClick.AddListener(ClosePopup);

		itemIcon.gameObject.SetActive(false);
		itemDescription.text = ""; 

		inventory = FindFirstObjectByType<InventoryHandler>();
		myItems = inventory.MyItems;
		myItems.Clear();

		foreach (Transform child in itemSlotParent) 
		{
			InventorySlotUI slot = child.GetComponent<InventorySlotUI>();
			SlotInit(slot, itemSlots.Count);

			itemSlots.Add(slot);
			myItems.Add(null);  
		} 
	}

	void InputInventoryKey(InputAction.CallbackContext context)
	{
		if (gameObject.activeSelf)
			CloseUI(); 
		else
			OpenUI();
	}
	void OpenUI()
	{
		for (int i = 0; i < itemSlots.Count; i++)
			itemSlots[i].SetIcon(myItems[i] == null ? null : myItems[i].ItemIcon);

		gameObject.SetActive(true);
		movingSlot.SetActive(false);
		popup.SetActive(false);	
	}
	void CloseUI()
	{
		gameObject.SetActive(false);

	}

	void SlotInit(InventorySlotUI slot, int idx)
	{
		slot.SetIcon(null);
		slot.onClick += ()=>ClickSlot(idx);
		slot.onRelease += ()=> ReleaseSlot(idx);
		slot.onHoverEnter += ()=> HoverEnterSlot(idx);
		slot.onHoverExit += ()=> HoverExitSlot(idx);
	}



	void ClickSlot(int idx)
	{
		Debug.Log($"클릭 {idx}");
		clikedButtonIdx = idx;
		
		if (myItems[idx] != null)
			moveSlotCrt = StartCoroutine(MoveItem(0.5f));
	} 

	void ReleaseSlot(int idx)
	{
		Debug.Log($"클릭 취소 {idx}");

		if (moveSlotCrt != null)
		{
			StopCoroutine(moveSlotCrt);
			moveSlotCrt = null;
		} 
		 
		// if 아이템을 이동중이라면
		if (movingSlot.activeSelf)
		{
			if (hoveredButtonIdx != -1 && clikedButtonIdx != -1)
			{
				ItemDataSO temp = myItems[clikedButtonIdx];
				myItems[clikedButtonIdx] = myItems[hoveredButtonIdx];
				myItems[hoveredButtonIdx] = temp;
			}

			OpenUI();
		}
		else
		{
			selectItemIdx = clikedButtonIdx;
			popup.SetActive(true);
		}


		clikedButtonIdx = -1;
	}

	void HoverEnterSlot(int idx)
	{
		Debug.Log($"호버 {idx}");
		hoveredButtonIdx = idx;

		if (myItems[idx] == null)
			return;

		itemIcon.sprite = myItems[idx].ItemIcon;
		itemDescription.text = myItems[idx].ItemName + "\n" + myItems[idx].ItemDescription;
		itemIcon.gameObject.SetActive(true);
	} 

	void HoverExitSlot(int idx)
	{
		hoveredButtonIdx = -1;

		itemIcon.gameObject.SetActive(false);	
		itemDescription.text = "";
	}

	IEnumerator MoveItem(float time)
	{
		yield return new WaitForSeconds(time);
		itemSlots[clikedButtonIdx].SetIcon(null); 
		movingSlot.transform.localPosition = movingSlot.transform.parent.InverseTransformPoint(Input.mousePosition);
		movingSlot.SetActive(true);
		movingSlot.GetComponent<InventorySlotUI>().SetIcon(myItems[clikedButtonIdx].ItemIcon);
		while (true)
		{
			movingSlot.transform.localPosition = movingSlot.transform.parent.InverseTransformPoint(Input.mousePosition);
			yield return null;
		}
	}

	void DropItem()
	{
		// 플레이어 앞으로 옮기는걸로 수정
		Vector3 dropPos = inventory.transform.position + inventory.transform.forward * 1.0f;
		var go = Instantiate(myItems[selectItemIdx].DropItemPrefab);
		go.transform.position = dropPos;

		myItems[selectItemIdx] = null;
		itemSlots[selectItemIdx].SetIcon(null);
		ClosePopup();
		selectItemIdx = -1;
	}

	void ClosePopup()
	{
		popup.gameObject.SetActive(false);
		selectItemIdx = -1;
	}
}

