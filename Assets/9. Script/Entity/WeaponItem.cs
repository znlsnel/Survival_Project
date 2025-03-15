using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IDamagable
{
	void TakePhysicalDamage(int damageAmout);
}


public class WeaponItem : ActiveItem
{
    [Header("Weapon Item")]
    [SerializeField] private int damage;
    [SerializeField] private float nockback;
    [SerializeField] private float attackDistance;
    [SerializeField] private GameObject particlePrefab;

	[Header("Combat")]
	[SerializeField] private bool doesDealDamage;

	[Header("Resource Gathering")] 
	[SerializeField] private bool doesGatherResource;

	private GameObject player;

	private void Awake()
	{
		player = GameManager.Instance.PlayerController.gameObject;
		InputManager.LeftMouse.started += (InputAction.CallbackContext context) => { Trigger(); };
	}
	public override void Trigger()
    {
        Debug.Log("무기 사용");
		OnHit();
	}

	private void OnHit()
	{
		// 타격 위치를 임시로 설정함 차후 변경 
		Vector3 startPos = player.transform.position + Vector3.up * 1.0f;
		Ray ray = new Ray(startPos, player.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, attackDistance))
		{

			if (doesGatherResource && hit.collider.TryGetComponent(out Resource resource))
			{
				resource.Gather(hit.point, hit.normal);
				resource.StartHitAnim(hit.point - startPos); 
			}  

			if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable damagable))
			{
				damagable.TakePhysicalDamage(damage);
			}

			var go =Instantiate(particlePrefab);
			go.transform.position = hit.point;	
			Destroy(go, 2.0f);
		} 
	} 
}   
 