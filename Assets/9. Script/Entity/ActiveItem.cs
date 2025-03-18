using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ActiveItem : MonoBehaviour
{
	[Header ("Active Item")]
	[SerializeField] protected float delay;

	protected Player.Controller controller;
	public void UseItem(Player.Controller controller)
	{
		this.controller = controller;
		UseItem();
	} 
	protected abstract void UseItem();

	public abstract void Trigger(); 
}
 