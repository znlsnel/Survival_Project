using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ItemData", menuName = "My ScriptableObject/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [Header ("Item Data")]
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string itemName; 
    [SerializeField] private string itemDescription;
    [SerializeField] private GameObject dropItemPrefab;

    public Sprite ItemIcon => itemIcon;
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public GameObject DropItemPrefab => dropItemPrefab;

    
}
 