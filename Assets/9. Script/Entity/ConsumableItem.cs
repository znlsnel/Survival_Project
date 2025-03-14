using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConsumableItem : UsableItem
{
	public override void ActionItem()
	{
		Debug.Log("소모형 아이템");
	}
}
