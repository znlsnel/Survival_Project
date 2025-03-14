using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotHandler : BaseUI
{
	enum GameObjects
	{
		movingSlot,
		InventoryScrollRect
	}

	// === Item List === 
	private Dictionary<ESlotType, List<ItemSlot>> itemSlots = new Dictionary<ESlotType, List<ItemSlot>>();
	private Dictionary<ESlotType, List<ItemDataSO>> myItems = new Dictionary<ESlotType, List<ItemDataSO>>();

	// === Objects ===
	private GameObject movingSlot;
	private ScrollRect scrollRect;

	// === Slot ===
	private Coroutine moveSlotCrt;
	private SlotInfo hoveredSlot;
	private SlotInfo clikedSlot;
	private SlotInfo selectSlot;

	// === copmonent ===
	private InventoryHandler inventory;

	private void OnValidate()
	{
		Bind<GameObject>(typeof(GameObjects));
	}

	private void Awake()
	{
		movingSlot = Get<GameObject>((int)GameObjects.movingSlot);
		scrollRect = Get<GameObject>((int)GameObjects.InventoryScrollRect).GetComponent<ScrollRect>();
		inventory = FindFirstObjectByType<InventoryHandler>();
	} 
	private void Start()
	{
		itemSlots.Add(ESlotType.InventorySlot, inventory.MyItemSlots);
		itemSlots.Add(ESlotType.QuickSlot, inventory.QuickSlots);
		myItems.Add(ESlotType.InventorySlot, inventory.MyItems);
		myItems.Add(ESlotType.QuickSlot, inventory.QuickSlotItems);
	}

	public void ClickSlot(int idx, ESlotType type)
	{
		clikedSlot = new SlotInfo(idx, type);
		scrollRect.enabled = false;

		if (myItems[type][idx] != null)
			moveSlotCrt = StartCoroutine(MoveItem(0.5f));
	}

	public void ReleaseSlot(int idx, ESlotType type)
	{
		scrollRect.enabled = true;

		if (moveSlotCrt != null)
		{
			StopCoroutine(moveSlotCrt);
			moveSlotCrt = null;
		}

		// 아이템이 이동중이라면 슬롯의 아이템 데이터 교환
		if (movingSlot.activeSelf)
		{ 
			if (hoveredSlot.type != ESlotType.None)
				inventory.SwitchSlot(clikedSlot, hoveredSlot);
			else
				itemSlots[clikedSlot.type][clikedSlot.idx].SetIcon(myItems[clikedSlot.type][clikedSlot.idx]);

			movingSlot.SetActive(false);
		}
		else
			selectSlot = clikedSlot;

		clikedSlot.type = ESlotType.None;
	}

	public void HoverEnterSlot(int idx, ESlotType type)
	{
		hoveredSlot = new SlotInfo(idx, type);
		itemSlots[type][idx].SelectSlot(true);
	}

	public void HoverExitSlot(int idx, ESlotType type)
	{
		hoveredSlot.type = ESlotType.None;
		itemSlots[type][idx].SelectSlot(false);
	} 

	private IEnumerator MoveItem(float time)
	{
		yield return new WaitForSeconds(time);

		itemSlots[clikedSlot.type][clikedSlot.idx].SetIcon(null);
		movingSlot.transform.localPosition = movingSlot.transform.parent.InverseTransformPoint(Input.mousePosition);
		movingSlot.GetComponent<ItemSlot>().SetIcon(myItems[clikedSlot.type][clikedSlot.idx]);
		movingSlot.SetActive(true);

		while (true)
		{
			movingSlot.transform.localPosition = movingSlot.transform.parent.InverseTransformPoint(Input.mousePosition);
			yield return null;
		}
	}
}
