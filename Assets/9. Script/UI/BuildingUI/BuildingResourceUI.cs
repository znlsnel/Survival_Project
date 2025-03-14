using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingResourceUI : MonoBehaviour
{
    [SerializeField] private Transform resourceListBG;  // �䱸 �ڿ� ����Ʈ UI
    [SerializeField] private GameObject resourceItemPrefab; // ���� �ڿ� UI ������
    [SerializeField] private InventoryHandler InventoryHandler; // ���߿� �÷��̾ �װ� ����� �̰� �ʿ������

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
        // �ǹ� �ִ� ���� ���� ���� ���
        int maxBuildCount = GetMaxBuildCount(building);

        TextMeshProUGUI itemLabelCanCnt = TopContentPrefab.transform.Find("Label-CanCnt").GetComponent<TextMeshProUGUI>();
        if (itemLabelCanCnt != null)
        {
            itemLabelCanCnt.text = $"{maxBuildCount}";
        }

        // ������ �̸� ����
        TextMeshProUGUI itemNameLabel = TopContentPrefab.transform.Find("Label-Title").GetComponent<TextMeshProUGUI>();
        if (itemNameLabel != null)
        {
            itemNameLabel.text = building.buildingName;
        }

        // ������ Ÿ�� ����
        TextMeshProUGUI itemCreditLabel = TopContentPrefab.transform.Find("Label-Credit").GetComponent<TextMeshProUGUI>();
        if (itemCreditLabel != null)
        {
            itemCreditLabel.text = building.buildingType.ToString();
        }

        // ������ ������
        Image itemItemIcon = TopContentPrefab.transform.Find("Image-Item").GetComponent<Image>();
        if (itemItemIcon != null)
        {
            itemItemIcon.sprite = building.buildingIcon;
        }



    }

    private void ScrollRectUpdate(BuildingData building)
    {
        // ���� �ڿ� UI ����
        foreach (Transform child in resourceListBG)
        {
            Destroy(child.gameObject);
        }

        // ���ο� �ڿ� UI ����
        foreach (ResourceCost resource in building.cost)
        {
            GameObject newResourceItem = Instantiate(resourceItemPrefab, resourceListBG);

            // ������ ����
            Image icon = newResourceItem.transform.Find("Icon").GetComponent<Image>();
            if (icon != null && resource.resourceItem.ItemIcon != null)
            {
                icon.sprite = resource.resourceItem.ItemIcon;
            }

            // ������ �̸� ����
            TextMeshProUGUI itemLabel = newResourceItem.transform.Find("Label-Item").GetComponent<TextMeshProUGUI>();
            if (itemLabel != null)
            {
                itemLabel.text = resource.resourceItem.ItemName;
            }

            // ���� ���� �� ���� ����
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
            int possibleCount = playerAmount / resource.amount; // �÷��̾ ���� �������� �ʿ��� ���� ������

            if (possibleCount < maxBuildCount)
            {
                maxBuildCount = possibleCount;
            }
        }

        return maxBuildCount == int.MaxValue ? 0 : maxBuildCount;
    }
}
