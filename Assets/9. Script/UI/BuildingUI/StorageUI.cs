using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform listBG;  // 아이템 리스트를 표시할 부모 오브젝트
    [SerializeField] private Transform toggleParent; // 동적 생성할 토글 부모
    [SerializeField] private GameObject buttonPrefab; // 아이템 버튼 프리팹
    [SerializeField] private GameObject togglePrefab; // EItemType별 토글 버튼 프리팹

    [SerializeField] private BoxInventory boxInventory;

    private List<ItemDataSO> allItems = new List<ItemDataSO>(); // 전체 박스 아이템 목록
    private EItemType selectedCategory = EItemType.None; // 현재 선택된 카테고리
    private Dictionary<EItemType, Toggle> categoryToggles = new Dictionary<EItemType, Toggle>();


    private void Start()
    {
        if (boxInventory != null)
        {
            InitializeUI(boxInventory.GetStoredItem()); // 박스 아이템을 가져와 UI 초기화
            boxInventory.OnInventoryChanged += InitializeUI;
        }
    }

    public void InitializeUI(List<ItemDataSO> items)
    {
        allItems = items;
        InitializeItemTypeToggles();
        UpdateItemList();
    }

    private void InitializeItemTypeToggles()
    {
        foreach (EItemType itemType in System.Enum.GetValues(typeof(EItemType)))
        {
            GameObject toggleObj = Instantiate(togglePrefab, toggleParent);
            toggleObj.name = $"{itemType}";

            TextMeshProUGUI toggleText = toggleObj.GetComponentInChildren<TextMeshProUGUI>();
            if (toggleText != null)
            {
                toggleText.text = itemType.ToString(); // 토글 버튼의 이름을 EItemType에 맞게 설정
            }

            Toggle toggle = toggleObj.GetComponent<Toggle>();
            if (toggle != null)
            {
                categoryToggles[itemType] = toggle;
                toggle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        selectedCategory = itemType;
                        UpdateItemList();
                    }
                });
            }
        }
    }

    private void UpdateItemList()
    {
        int index = 0;

        Dictionary<ItemDataSO, int> itemStack = new Dictionary<ItemDataSO, int>();

        foreach (ItemDataSO item in allItems)
        {
            if (item.CanStackItems)
            {
                if (itemStack.ContainsKey(item))
                    itemStack[item]++;
                else
                    itemStack[item] = 1;
            }
            else
            {
                itemStack[item] = 1; // 스택 불가능한 아이템은 개수 1로 유지
            }
        }

        foreach (var pair in itemStack)
        {
            bool matchesCategory = (selectedCategory == EItemType.None || pair.Key.ItemType == selectedCategory);

            if (matchesCategory)
            {
                GameObject itemSlotUI;

                // 기존 UI가 있다면 재사용, 없으면 새로 생성
                if (index < listBG.childCount)
                {
                    itemSlotUI = listBG.GetChild(index).gameObject;
                    itemSlotUI.SetActive(true);
                }
                else
                {
                    itemSlotUI = Instantiate(buttonPrefab, listBG);
                }

                itemSlotUI.name = $"ItemSlot_{pair.Key.ItemName}";

                ItemSlot slot = itemSlotUI.GetComponent<ItemSlot>();
                if (slot != null)
                {
                    slot.SetIcon(pair.Key);
                    slot.StackAmount = pair.Value; // 스택 개수 업데이트
                }

                index++;
            }
        }

        // 남은 UI 아이템 숨기기
        for (int i = index; i < listBG.childCount; i++)
        {
            listBG.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void InitializeTooltip(GameObject item, ItemDataSO itemdata)
    {
        Transform tooltipTransform = item.transform.Find("Tooltip-Multiline");
        if (tooltipTransform == null)
        {
            Debug.Log($"툴팁 없음");

            return;
        }

        TextMeshProUGUI titleText = tooltipTransform.Find("Label-Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bodyText = tooltipTransform.Find("Label-Body").GetComponent<TextMeshProUGUI>();

        if (titleText != null)
        {
            titleText.text = itemdata.ItemName; // 아이템 이름 적용
        }

        if (bodyText != null)
        {
            bodyText.text = itemdata.ItemDescription; // 아이템 설명 적용
        }
    }
}
