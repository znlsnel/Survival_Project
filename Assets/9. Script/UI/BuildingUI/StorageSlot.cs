using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class StorageSlot : ItemSlot, IPointerClickHandler
{
    public InventoryHandler playerInventory;
    public ItemDataSO itemData;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            MoveItemToPlayerInventory();
        }
    }
    private void Start()
    {
        playerInventory = FindObjectOfType<InventoryHandler>();
    }

    public void SetItem(ItemDataSO data, int amount)
    {
        itemData = data;
        StackAmount = amount;
        SetIcon(data);
    }

    //TODO: 제발 좀 쳐 돼씅면 좋겟따.
    private void MoveItemToPlayerInventory()
    {
        if (playerInventory == null || itemData == null) return;

        Debug.Log($"뭐가 없니?");


        // 플레이어 인벤토리에 아이템 추가
        bool added = playerInventory.AddItem(itemData);

        if (added)
        {
            Debug.Log($"플레이어 인벤토리에 {itemData.ItemName} {StackAmount}개 추가됨!");
            RemoveItem(); // 스토리지에서 아이템 삭제
        }

    }

    private void RemoveItem()
    {
        StackAmount = 0;
        SetIcon(null); // 아이콘 제거
        gameObject.SetActive(false); // 슬롯 비활성화
    }
}
