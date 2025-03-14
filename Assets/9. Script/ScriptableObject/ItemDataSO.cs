using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public enum EItemType
{ 
    None,
    Weapon,
    Consumable,
    Resource,
}


[CreateAssetMenu(fileName = "new ItemData", menuName = "My ScriptableObject/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [Header ("Item Image")]
	[SerializeField] private Sprite itemIcon;
	[SerializeField] private Sprite itemTypeIcon;

    [Header ("Item Info")]
	[SerializeField] private EItemType itemType; 
    [SerializeField] private string itemName; 
    [SerializeField] private string itemDescription;
    [SerializeField] private GameObject dropItemPrefab;

    [Header("UsableItem Info")]
    [SerializeField] private bool isUsableItem = false;

    [Header("Amountable")]
    [SerializeField] private bool canStackItems = false;
    [SerializeField] private int maxStackCount = 50;


	public EItemType ItemType => itemType; 
    public GameObject DropItemPrefab => dropItemPrefab;
    public Sprite ItemIcon => itemIcon;
    public Sprite ItemTypeIcon { get => itemTypeIcon; set => itemTypeIcon = value; }
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public bool CanStackItems => canStackItems;
    public int MaxStackCount => maxStackCount;
}
 