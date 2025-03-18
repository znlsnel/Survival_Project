using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsumableItem : ActiveItem
{
	[Header ("itemInfo")]
	[SerializeField] private GameObject particle;
	[SerializeField] private ItemDataSO data;


	protected override void UseItem()
	{
		controller.AnimationHandler.Drink();
	}

	public override void Trigger()
	{
		OnEffect(); 
	}

	private void OnEffect()
	{
		var go = Instantiate(particle);
		go.transform.position = controller.transform.position;
		Destroy(go, 3.0f); 

		if (controller.TryGetComponent(out PlayerCondition conditions))
		{
			conditions.Heal(data.Health);
			conditions.Eat(data.Hunger);
			conditions.Drink(data.Thirsty);
			conditions.Rest(data.Stamina);	
			conditions.Rest(data.Temperature);  
		}
		StartCoroutine(RemoveItem(0.2f));
	
	}

	private IEnumerator RemoveItem(float timer)
	{
		yield return new WaitForSeconds(timer);
		controller.GetComponent<InventoryHandler>().RemoveItem(data);
	}
}
 