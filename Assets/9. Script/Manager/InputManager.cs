using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	[Header("Input Info")]
	[SerializeField] private InputActionAsset inputSystem;
	[SerializeField] private InputActionReference move;
	[SerializeField] private InputActionReference jump;
	[SerializeField] private InputActionReference interaction;


	public InputActionReference Move => move; 
	public InputActionReference Jump => jump; 
	public InputActionReference Interaction => interaction;

	private void OnDestroy()
	{
		inputSystem.Disable();
	}

	protected override void Awake()
	{
		base.Awake();
		inputSystem.Enable();  
	}

	public static void SetActive(bool active)
	{
		if (active)
			Instance.inputSystem.Enable(); 
		else
			Instance.inputSystem.Disable();
	}
}
