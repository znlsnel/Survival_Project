using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingResourceUI : MonoBehaviour
{
    [SerializeField] private Transform resourceListBG;  // 요구 자원 리스트 UI
    [SerializeField] private GameObject resourceItemPrefab; // 개별 자원 UI 프리팹
    [SerializeField] private InventoryHandler InventoryHandler; // 나중에 플레이어에 그거 생기면 이거 필요없을듯

    [SerializeField] private GameObject TopContentPrefab;


    List<ItemDataSO> playerItemList;

    private void Awake()
    {

        playerItemList = InventoryHandler.MyItems;
    }

    public void UpdateResourceUI(BuildingData building)
    {
        UpdateTop(building);
        ScrollRectUpdate(building);
    }

    private void UpdateTop(BuildingData building)
    {
        // 건물 최대 생성 가능 개수 계산
        int maxBuildCount = GetMaxBuildCount(building);

        TextMeshProUGUI itemLabelCanCnt = TopContentPrefab.transform.Find("Label-CanCnt").GetComponent<TextMeshProUGUI>();
        if (itemLabelCanCnt != null)
        {
            itemLabelCanCnt.text = $"{maxBuildCount}";
        }

        // 아이템 이름 설정
        TextMeshProUGUI itemNameLabel = TopContentPrefab.transform.Find("Label-Title").GetComponent<TextMeshProUGUI>();
        if (itemNameLabel != null)
        {
            itemNameLabel.text = building.buildingName;
        }

        // 아이템 타입 설정
        TextMeshProUGUI itemCreditLabel = TopContentPrefab.transform.Find("Label-Credit").GetComponent<TextMeshProUGUI>();
        if (itemCreditLabel != null)
        {
            itemCreditLabel.text = building.buildingType.ToString();
        }

        // 아이템 아이콘
        Image itemItemIcon = TopContentPrefab.transform.Find("Image-Item").GetComponent<Image>();
        if (itemItemIcon != null)
        {
            itemItemIcon.sprite = building.buildingIcon;
        }



    }

    private void ScrollRectUpdate(BuildingData building)
    {
        // 기존 자원 UI 삭제
        foreach (Transform child in resourceListBG)
        {
            Destroy(child.gameObject);
        }

        // 새로운 자원 UI 생성
        foreach (ResourceCost resource in building.cost)
        {
            GameObject newResourceItem = Instantiate(resourceItemPrefab, resourceListBG);

            // 아이콘 설정
            Image icon = newResourceItem.transform.Find("Icon").GetComponent<Image>();
            if (icon != null && resource.resourceItem.ItemIcon != null)
            {
                icon.sprite = resource.resourceItem.ItemIcon;
            }

            // 아이템 이름 설정
            TextMeshProUGUI itemLabel = newResourceItem.transform.Find("Label-Item").GetComponent<TextMeshProUGUI>();
            if (itemLabel != null)
            {
                itemLabel.text = resource.resourceItem.ItemName;
            }

            // 수량 설정 및 색상 변경
            TextMeshProUGUI amountText = newResourceItem.transform.Find("Resources-Amount/Label-Amount").GetComponent<TextMeshProUGUI>();
            if (amountText != null)
            {
                int playerAmount = GetItemAmount(resource.resourceItem);
                amountText.text = $"{playerAmount} / {resource.amount}";

                amountText.color = playerAmount >= resource.amount ? Color.white : Color.red;
            }
        }
    }


    public int GetItemAmount(ItemDataSO item)
    {
        int count = 0;
        foreach (ItemDataSO inventoryItem in playerItemList)
        {
            if (inventoryItem == item)
            {
                count++;
            }
        }
        return count;
    }

    public int GetMaxBuildCount(BuildingData building)
    {
        int maxBuildCount = int.MaxValue;

        foreach (ResourceCost resource in building.cost)
        {
            int playerAmount = GetItemAmount(resource.resourceItem);
            int possibleCount = playerAmount / resource.amount; // 플레이어가 가진 개수에서 필요한 개수 나누기

            if (possibleCount < maxBuildCount)
            {
                maxBuildCount = possibleCount;
            }
        }

        return maxBuildCount == int.MaxValue ? 0 : maxBuildCount;
    }
}
