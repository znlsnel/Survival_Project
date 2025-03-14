using System.Collections.Generic;
using UnityEngine;

// do: 상태와 달리 프리팹만 변경하도록 처리
[RequireComponent(typeof(MeleeWeaponAudioHandler))]
public class MeleeWeaponController: MonoBehaviour
{
    // melee resource handler
    public float power = 0f;
    public float knockBackForce = 10f;
    
    public bool isAttacking = false;
    public List<GameObject> hitObjects = new List<GameObject>(); // 한번의 공격에 당한 에너미들

    public void ClearAttacking()
    {
        isAttacking = false;
        hitObjects.Clear();
    }
    
    // audio handler
    [HideInInspector] public MeleeWeaponAudioHandler audioHandler;

    private void Awake()
    {
        audioHandler = GetComponent<MeleeWeaponAudioHandler>();
    }

    public void SetMeleeAttackAvailable(bool isAvailable)
    {
        isAttacking = isAvailable;
        if(!isAttacking) ClearAttacking();
    }
}