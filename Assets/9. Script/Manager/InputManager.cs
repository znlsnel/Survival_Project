using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	[Header("Input Info")]
	[SerializeField] private InputActionAsset inputSystem;
	[SerializeField] private InputActionReference move;
	[SerializeField] private InputActionReference jump;


	public InputActionReference Move => move; 
	public InputActionReference Jump => jump; 


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
