using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : Singleton<Util>
{
    
    public static Transform FindChildByName(Transform parent, string childName)
    {
		Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren)
		{
			if (child.name == childName)
			{
				return child;
			}
		}
		return null;
	}
}
