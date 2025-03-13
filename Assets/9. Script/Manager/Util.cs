using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : Singleton<Util>
{
	public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
	{
		return FindChild<Transform>(go, name, recursive)?.gameObject;

	}

	public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		if (go == null)
			return null;

		if (recursive == false)
		{
			for (int i = 0; i < go.transform.childCount; i++)
			{
				var transform = go.transform.GetChild(i);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					if (transform.TryGetComponent(out T component))
						return component;
				}
			}
		}
		else
		{
			foreach (T component in go.GetComponentsInChildren<T>())
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
					return component;
			}

		}

		return null;
	}
}
