using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ActiveItem : MonoBehaviour
{
	[Header ("Active Item")]
	[SerializeField] protected float delay;

	public abstract void Trigger();
}
 