using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
	[SerializeField] GameObject ObjectInfoUIPrefab;

    private ObjectInfoUI objectInfoUI;
	public ObjectInfoUI ObjectInfoUI => objectInfoUI;

	protected override void Awake()
	{
		base.Awake();
		objectInfoUI = Instantiate(ObjectInfoUIPrefab).GetComponent<ObjectInfoUI>();	
	}
	 
}
