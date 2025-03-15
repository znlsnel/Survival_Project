using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform listBG;  // ������ ����Ʈ�� ǥ���� �θ� ������Ʈ
    [SerializeField] private Transform toggleParent; // ���� ������ ��� �θ�
    [SerializeField] private GameObject buttonPrefab; // ������ ��ư ������
    [SerializeField] private GameObject togglePrefab; // EItemType�� ��� ��ư ������

    [SerializeField] private BoxInventory boxInventory;

    private List<ItemDataSO> allItems = new List<ItemDataSO>(); // ��ü �ڽ� ������ ���
    private EItemType selectedCategory = EItemType.None; // ���� ���õ� ī�װ�
    private Dictionary<EItemType, Toggle> categoryToggles = new Dictionary<EItemType, Toggle>();


    private void Start()
    {
        if (boxInventory != null)
        {
            InitializeUI(boxInventory.GetStoredItem()); // �ڽ� �������� ������ UI �ʱ�ȭ
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
                toggleText.text = itemType.ToString(); // ��� ��ư�� �̸��� EItemType�� �°� ����
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
                itemStack[item] = 1; // ���� �Ұ����� �������� ���� 1�� ����
            }
        }

        foreach (var pair in itemStack)
        {
            bool matchesCategory = (selectedCategory == EItemType.None || pair.Key.ItemType == selectedCategory);

            if (matchesCategory)
            {
                GameObject itemSlotUI;

                // ���� UI�� �ִٸ� ����, ������ ���� ����
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
                    slot.StackAmount = pair.Value; // ���� ���� ������Ʈ
                }

                index++;
            }
        }

        // ���� UI ������ �����
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
            Debug.Log($"���� ����");

            return;
        }

        TextMeshProUGUI titleText = tooltipTransform.Find("Label-Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bodyText = tooltipTransform.Find("Label-Body").GetComponent<TextMeshProUGUI>();

        if (titleText != null)
        {
            titleText.text = itemdata.ItemName; // ������ �̸� ����
        }

        if (bodyText != null)
        {
            bodyText.text = itemdata.ItemDescription; // ������ ���� ����
        }
    }
}
