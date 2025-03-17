using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private Player.Controller playerController;

	public Player.Controller PlayerController => playerController;
	protected override void Awake()
	{
		base.Awake();
		playerController = FindFirstObjectByType<Player.Controller>();	
	}


}
