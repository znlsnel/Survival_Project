using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ItemData", menuName = "My ScriptableObject/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string itemName; 
    [SerializeField] private string itemDescription;

    public Sprite ItemIcon => itemIcon;
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
}
 