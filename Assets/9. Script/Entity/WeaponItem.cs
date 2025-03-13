using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private float nockback;

    // ������ ��ġ�� ���� ( ĳ���� �� Transform )
    public abstract void InitItem(Transform parent);

    // Attack �Լ�  -> PlayerAnimation Handler�� �����Ͽ� ���� �ִ� ����
    public abstract void Attack();
}
 