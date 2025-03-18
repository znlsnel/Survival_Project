using Enemy.Chomper;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeUI : MonoBehaviour
{
    [SerializeField] private Transform resourceListBG; // 요구 자원 리스트 UI
    [SerializeField] private GameObject resourceItemPrefab; // 개별 자원 UI 프리팹
    [SerializeField] private InventoryHandler inventoryHandler; // 인벤토리 (플레이어 아이템 데이터 관리)

    [SerializeField] private GameObject topContentPrefab;
    [SerializeField] private Button buttonMake;  // 교환 버튼
    [SerializeField] private Button buttonCancel; // 취소 버튼
    [SerializeField] private ItemDataSO engineItemData;
    
    private ExchangeDataSO currentExchangeData; // 현재 선택된 교환 데이터
    private List<ItemDataSO> playerItemList;

    private void OnValidate()
    {
        if (resourceListBG == null) resourceListBG = transform.Find("ScrollRect/Viewport/Content");
        if (topContentPrefab == null) topContentPrefab = transform.Find("Top-Content")?.gameObject; 
        if (buttonMake == null) buttonMake = transform.Find("Button-make")?.GetComponent<Button>();
        if (buttonCancel == null) buttonCancel = transform.Find("Button-cancel")?.GetComponent<Button>();
        if (buttonMake != null && buttonMake.onClick.GetPersistentEventCount() == 0)
        {
            buttonMake.onClick.AddListener(ProcessExchange);
        }
        if (buttonCancel != null && buttonCancel.onClick.GetPersistentEventCount() == 0)
        {
            buttonCancel.onClick.AddListener(CloseUI);
        }
        if(inventoryHandler == null) inventoryHandler = FindObjectOfType<InventoryHandler>();

    }

    private void Awake()
    {
        if (inventoryHandler == null)
        {
            Debug.LogError("InventoryHandler가 할당되지 않았습니다! Inspector에서 설정하세요.");
            return;
        }

        playerItemList = inventoryHandler.MyItems;

        buttonMake.onClick.AddListener(ProcessExchange);
        buttonCancel.onClick.AddListener(CloseUI);
        CloseUI();
    }


    public void UpdateExchangeUI(ExchangeDataSO exchangeData)
    {
        currentExchangeData = exchangeData;

        if (exchangeData == null)
        {
            Debug.LogError("선택된 교환 데이터가 없습니다.");
            return;
        }

        UpdateTop(exchangeData);
        ScrollRectUpdate(exchangeData);
    }

    private void UpdateTop(ExchangeDataSO exchangeData)
    {
        // 교환 아이템 이름 설정
        TextMeshProUGUI itemNameLabel = topContentPrefab.transform.Find("Label-Title").GetComponent<TextMeshProUGUI>();
        if (itemNameLabel != null)
        {
            itemNameLabel.text = exchangeData.ExchangeTitle;
        }

        // 교환 아이템 설명 설정
        TextMeshProUGUI itemDescLabel = topContentPrefab.transform.Find("Label-Desc").GetComponent<TextMeshProUGUI>();
        if (itemDescLabel != null)
        {
            itemDescLabel.text = exchangeData.ExchangeDesc;
        }

        // 아이템 아이콘
        Image itemIcon = topContentPrefab.transform.Find("Image-Item").GetComponent<Image>();
        if (itemIcon != null)
        {
            itemIcon.sprite = exchangeData.ExchangeIcon;
        }
    }

    private void ScrollRectUpdate(ExchangeDataSO exchangeData)
    {
        // 기존 자원 UI 삭제
        foreach (Transform child in resourceListBG)
        {
            Destroy(child.gameObject);
        }

        // 새로운 자원 UI 생성
        foreach (ExchangeDataSO.ExchangeRequirement requirement in exchangeData.ExchangeRequirements)
        {
            GameObject newResourceItem = Instantiate(resourceItemPrefab, resourceListBG);

            // 아이콘 설정
            Image icon = newResourceItem.transform.Find("Icon").GetComponent<Image>();
            if (icon != null && requirement.item.ItemIcon != null)
            {
                icon.sprite = requirement.item.ItemIcon;
            }

            // 아이템 이름 설정
            TextMeshProUGUI itemLabel = newResourceItem.transform.Find("Label-Item").GetComponent<TextMeshProUGUI>();
            if (itemLabel != null)
            {
                itemLabel.text = requirement.item.ItemName;
            }

            // 수량 설정 및 색상 변경
            TextMeshProUGUI amountText = newResourceItem.transform.Find("Resources-Amount/Label-Amount").GetComponent<TextMeshProUGUI>();
            if (amountText != null)
            {
                int playerAmount = GetItemAmount(requirement.item);
                amountText.text = $"{playerAmount} / {requirement.amount}";

                amountText.color = playerAmount >= requirement.amount ? Color.white : Color.red;
            }
        }
    }


    public int GetItemAmount(ItemDataSO item)
    {
        if (playerItemList == null)
        {
            Debug.LogError("playerItemList 가 null이다?");
            return 0;
        }

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
    private void ProcessExchange()
    {
        if (currentExchangeData == null)
        {
            Debug.LogError("교환할 아이템이 선택되지 않았습니다.");
            return;
        }

        // 교환 가능 여부 확인
        int maxExchangeCount = GetMaxExchangeCount(currentExchangeData);
        if (maxExchangeCount < 1)
        {
            Debug.Log("교환할 자원이 부족합니다.");
            return;
        }

        // 교환 진행 (아이템 제거 및 추가)
        foreach (ExchangeDataSO.ExchangeRequirement requiredItem in currentExchangeData.ExchangeRequirements)
        {
            RemoveItemFromInventory(requiredItem.item, requiredItem.amount);
        }

        // 교환 아이템 추가
        playerItemList.Add(currentExchangeData.ExchangeRewards);
        Debug.Log($"교환 완료! {currentExchangeData.ExchangeRewards.ItemName}을 획득했습니다.");

        var go = Instantiate(currentExchangeData.ExchangeRewards.DropItemPrefab);
        go.transform.position = GameManager.Instance.transform.position; 

        // UI 업데이트
        UpdateExchangeUI(currentExchangeData);
    }

    private void RemoveItemFromInventory(ItemDataSO item, int amount)
    {
        int removeCount = amount;

        for (int i = playerItemList.Count - 1; i >= 0; i--)
        {
            if (playerItemList[i] == item)
            {
                playerItemList.RemoveAt(i);
                removeCount--;

                if (removeCount <= 0) break; // 필요한 만큼 삭제 완료하면 종료
            }
        }
    }
    public int GetMaxExchangeCount(ExchangeDataSO exchangeData)
    {
        int maxExchangeCount = int.MaxValue;

        foreach (ExchangeDataSO.ExchangeRequirement requiredItem in exchangeData.ExchangeRequirements)
        {
            int playerAmount = GetItemAmount(requiredItem.item);
            int possibleCount = playerAmount / requiredItem.amount; // 필요 개수 반영

            if (possibleCount < maxExchangeCount)
            {
                maxExchangeCount = possibleCount;
            }
        }

        return maxExchangeCount == int.MaxValue ? 0 : maxExchangeCount;
    }
    private void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
