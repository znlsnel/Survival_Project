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

    [SerializeField] private PlayerInput playerInput;	// 플레이어 인풋연결
    [SerializeField] private BuildingInputHandler buildingInputHandler;	// 건축에 쓰이는 핸들러


    public InputActionReference Move => move; 
	public InputActionReference Jump => jump; 


	protected override void Awake()
	{
		base.Awake();
		inputSystem.Enable();

        if (buildingInputHandler != null)
        {
            buildingInputHandler.enabled = true;
        }
    }

	public static void SetActive(bool active)
	{
		if (active)
			Instance.inputSystem.Enable(); 
		else
			Instance.inputSystem.Disable();
	}
}
