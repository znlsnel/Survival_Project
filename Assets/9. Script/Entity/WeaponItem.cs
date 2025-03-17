using Enemy.Chomper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

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
	private float lastTriggeTime = 0f;
	private void Start()
	{
		player = GameManager.Instance.PlayerController.gameObject;
	}  

	public override void Trigger() 
    { 

		Player.HitPoint hp = player.GetComponentInChildren<Player.HitPoint>();
		foreach (var resource in hp.GetTargetResources())
		{
			if (resource == null)
				continue;

			Vector3 normal = (transform.position - resource.transform.position).normalized;
			Vector3 hitPoint = resource.transform.position + normal * 0.3f;

			resource.Gather(hitPoint, normal);
			resource.StartHitAnim(-normal);

			var go = Instantiate(particlePrefab); 
			go.transform.position = hitPoint;
			Destroy(go, 2.0f);
		}

		
	}
}   
 