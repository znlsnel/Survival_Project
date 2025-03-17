using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsumableItem : ActiveItem
{
	
	public override void Trigger()
	{
		Debug.Log("소모형 아이템");
	}
}
