using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private PlayerController playerController;

	public PlayerController PlayerController => playerController;
	protected override void Awake()
	{
		base.Awake();
		playerController = FindFirstObjectByType<PlayerController>();	
	}


}
