using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] private GameObject endingCutScene;
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
		endingCutScene.gameObject.SetActive(true);
	}

}
