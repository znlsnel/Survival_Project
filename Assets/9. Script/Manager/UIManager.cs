using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private ObjectInfoUI objectInfoUI;
	public ObjectInfoUI ObjectInfoUI => objectInfoUI;

	protected override void Awake()
	{
		base.Awake();
		objectInfoUI = FindFirstObjectByType<ObjectInfoUI>();	
	}
	 
}
