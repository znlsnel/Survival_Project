using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHandler : MonoBehaviour
{

	private ObjectInfoUI objectInfoUI;
	private MessageUI messageUI;

	public ObjectInfoUI ObjectInfoUI => objectInfoUI;
	public MessageUI MessageUI => messageUI;
	 
	private void Awake()
	{
		objectInfoUI = FindFirstObjectByType<ObjectInfoUI>();
		messageUI = FindFirstObjectByType<MessageUI>(); 

	}
}
