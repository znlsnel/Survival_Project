using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private float nockback;

    // 무기의 위치를 고정 ( 캐릭터 손 Transform )
    public abstract void InitItem(Transform parent);

    // Attack 함수  -> PlayerAnimation Handler에 접근하여 공격 애님 실행
    public abstract void Attack();
}
 