using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EItemType
{ 
    Weapon,
    Consumable,
    Resource,
}


[CreateAssetMenu(fileName = "new ItemData", menuName = "My ScriptableObject/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [Header ("Item Info")]
	[SerializeField] private EItemType itemType;
	[SerializeField] private Sprite itemIcon;
    [SerializeField] private string itemName; 
    [SerializeField] private string itemDescription;
    [SerializeField] private GameObject dropItemPrefab;

	[Header("Weapon Info")]
	[SerializeField] private GameObject weaponPrefab;

	public EItemType ItemType => itemType;
    public Sprite ItemIcon => itemIcon;
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public GameObject DropItemPrefab => dropItemPrefab;
    public GameObject WeaponPrefab => weaponPrefab;

   
}

[CreateAssetMenu(fileName = "new ItemData", menuName = "My ScriptableObject/ItemData")]
public class WeaponItemSO : ItemDataSO
{
	
}
 