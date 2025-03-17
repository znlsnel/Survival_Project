using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDisplayHandler : MonoBehaviour
{
    [SerializeField] private string objName;
    [SerializeField] private string description;

    private Player.Controller controller;
    private ObjectInfoUI objInfo;
	private void Start() 
	{
        controller = GameManager.Instance.PlayerController;
        objInfo = controller.gameObject.GetComponentInChildren<PlayerUIHandler>().ObjectInfoUI;
	}
	public void ShowInfo() 
    { 
		objInfo.OpenUI(objName, description);
    }
}
  