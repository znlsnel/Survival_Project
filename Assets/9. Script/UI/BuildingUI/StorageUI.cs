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

        foreach (ItemDataSO item in allItems)
        {
            bool matchesCategory = (selectedCategory == EItemType.None || item.ItemType == selectedCategory);

            if (matchesCategory)
            {
                GameObject itemUI;

                // ���� �������� �ִٸ� ����, ������ ���� ����
                if (index < listBG.childCount)
                {
                    itemUI = listBG.GetChild(index).gameObject;
                    itemUI.SetActive(true);
                }
                else
                {
                    itemUI = Instantiate(buttonPrefab, listBG);
                }

                itemUI.name = $"{item.ItemName}";

                Image itemIcon = itemUI.transform.Find("Item").GetComponent<Image>();
                if (itemIcon != null)
                {
                    itemIcon.sprite = item.ItemIcon;
                }

                InitializeTooltip(itemUI, item);

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
