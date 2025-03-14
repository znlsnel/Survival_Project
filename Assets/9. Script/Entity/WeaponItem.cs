using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : UsableItem
{
    [Header("Item Info")]
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private float nockback;

    public override void ActionItem()
    {
        Debug.Log("무기 사용");
    }
} 
 