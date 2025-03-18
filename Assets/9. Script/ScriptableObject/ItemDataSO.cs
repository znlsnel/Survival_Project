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

    [Header("ActiveItem")]
    [SerializeField] private bool isActiveItem = false;
    [SerializeField] private GameObject activeItemPrefab;

    [Header("Amountable")]
    [SerializeField] private bool canStackItems = false;
    [SerializeField] private int maxStackCount = 50;

    [Header ("Consumable")]
	[SerializeField] private float health;
	[SerializeField] private float hunger;
	[SerializeField] private float thirsty;
	[SerializeField] private float stamina;
	[SerializeField] private float temperature;

	public EItemType ItemType => itemType; 
    public GameObject DropItemPrefab => dropItemPrefab;
    public GameObject ActiveItemPrefab => activeItemPrefab; 
    public Sprite ItemIcon => itemIcon;
    public Sprite ItemTypeIcon { get => itemTypeIcon; set => itemTypeIcon = value; }

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public bool CanStackItems => canStackItems;
	public bool IsActiveItem => isActiveItem;
	public int MaxStackCount => maxStackCount;

	public float Health => health;
	public float Hunger => hunger;
	public float Thirsty => thirsty;
	public float Stamina => stamina;
	public float Temperature => temperature; 


}
 