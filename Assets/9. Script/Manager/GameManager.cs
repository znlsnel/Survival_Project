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

	public void StartGame()
	{
		playerController.AnimationHandler.animator.speed = 1.0f; 

		
	}
	public void GameEnd()
	{

	}

}
